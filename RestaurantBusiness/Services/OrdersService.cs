
using RestaurantBusiness.InterfaceServices;
using RestaurantDataAccess.Entities;
using RestaurantShared.DTOs.OrderItemsDTOs;
using RestaurantShared.DTOs.OrdersDTOs;
using RestaurantShared.Entities;
using RestaurantShared.Enums;
using RestaurantShared.Results;
using RestuarantDataAccess.UintOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantBusiness.Services
{
    public class OrdersService: IOrdersService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly ICurrentUserService _currentUserService;
        public OrdersService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<int>> AddNewOrderAsync(CreateOrderDto newOrder)
        {


            var userId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(userId))
            {
                return Result<int>.Failure("The user is not authorized, or the session has expired.");
            }


            var employee = await _unitOfWork.empolyeesRepository.GetEmployeeByUserIdAsync(int.Parse(userId));
            if (employee == null)
                return Result<int>.Failure("No employee");
             if(newOrder.OrderType ==enOrderType.DineIn && newOrder.TableNumber == null)
                return Result<int>.Failure("You cannont place an order inside the restaurant,and there are no table number.");
            if ((newOrder.OrderType == enOrderType.Takeaway|| newOrder.OrderType == enOrderType.Delivery) && newOrder.CustomerId == null)
                return Result<int>.Failure("You cannont place an order outside the restaurant,and there are no customer.");

            try
            {
               
                var order = new Order
                {
                    CustomerId = newOrder.CustomerId,
                    CreatedBy = employee.EmployeeId,
                    OrderDate = DateTime.Now,
                    Status = enOrderStatus.Pending,
                    OrderType = newOrder.OrderType,
                    TableNumber = newOrder.TableNumber,
                    TotalAmount = 0,
                };
                 
                await _unitOfWork.ordersRepository.AddAsync(order);
                await _unitOfWork.SaveChangesAsync();
                return Result<int>.Success(order.OrderId);
            }
            catch (Exception error)
            { return Result<int>.Failure(error.Message);
            }
        }

        public async Task<Result> DeleteOrder(int OrderId)
        {
            if (OrderId <= 0)
                return Result.Failure("Id must be a postiver");


            var Order = await _unitOfWork.ordersRepository.GetByIdAsync(OrderId);

            if (Order == null)
                return Result.Failure("No exist order with this id");

            _unitOfWork.ordersRepository.DeleteAsync(Order);

            return await _unitOfWork.SaveChangesAsync() > 0? Result.Success(): Result.Failure("Something is wrong");
        }

        public async Task<Result<ReadOrdersDto>> GetOrderAsync(int OrderId)
        {
            if (OrderId <= 0)
                return Result<ReadOrdersDto>.Failure("Id must be a postive");

            var order = await _unitOfWork.ordersRepository.GetOrderAsync(OrderId);

            if (order == null)
                return Result<ReadOrdersDto>.Failure("No exist order with this id");
          
           var orderDto = new ReadOrdersDto
            {
                OrderId = order.OrderId,
                CreatedBy = order.CreatedBy,
                CreatedByName = order.CreatedByNavigation?.User?.Username,
                CustomerId = order.CustomerId ?? 0,
                CustomerName = order.Customer?.Name,
                OrderDate = order.OrderDate,
                OrderType = order.OrderType,
                Status = order.Status,
                TableNumber = order.TableNumber,
                TotalAmount = order.TotalAmount,
                Items = order.OrderItems.Select(oi=>new ReadOrderItemsDto
                {
                    OrderItemId = oi.OrderItemId,
                    ProductId = oi.ProductId,
                    ProductName = oi.Product.Name,
                    UnitPrice = oi.UnitPrice,
                    Quantity = oi.Quantity,
                }).ToList(),
            };

            return Result<ReadOrdersDto>.Success(orderDto);
        
        }

        public async Task<Result<List<ReadOrdersDto>>> GetOrdersAsync()
        {
            var Orders = await _unitOfWork.ordersRepository.GetOrdersAsync();

            if (Orders == null)
                return Result<List<ReadOrdersDto>>.Failure("There are no orders");

            var OrdersDto = Orders.Select(o => new ReadOrdersDto
            {
                OrderId = o.OrderId,
                CreatedBy = o.CreatedBy,
                CreatedByName = o.CreatedByNavigation?.User?.Username,
                CustomerId = o.CustomerId,
                CustomerName = o.Customer?.Name,
                Status = o.Status,
                OrderType = o.OrderType,
                OrderDate = o.OrderDate,
                TableNumber = o.TableNumber,
                TotalAmount = o.TotalAmount,
                Items = o.OrderItems.Select(oi => new ReadOrderItemsDto
                {
                    OrderItemId = oi.OrderItemId,
                    ProductId = oi.ProductId,
                    ProductName = oi.Product.Name,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice
                }).ToList()
            }).ToList();
        
            return Result<List<ReadOrdersDto>>.Success(OrdersDto);
        }

        public async Task<bool> isOrderExistAsync(int OrderId)
        {
            if (OrderId <= 0)
                return false;

            return await _unitOfWork.ordersRepository.isExistAsync(OrderId);

        }

        public async Task<Result> UpdateOrderAsync(int orderId, UpdateOrderDto order)
        {
            if (orderId <= 0)
                return Result.Failure("Id must be a postive");

            var orderToUpdate = await _unitOfWork.ordersRepository.GetByIdAsync(orderId);
            
            if(orderToUpdate == null)
                return Result.Failure("No exist order with this id");

            if (orderToUpdate.Status == enOrderStatus.Completed|| orderToUpdate.Status == enOrderStatus.Cancelled)
                return Result.Failure("You cannot modify the order status once it is completed or cancelled");

            if (order.OrderType == enOrderType.DineIn && order.TableNumber == null)
                return Result.Failure("You cannont place an order inside the restaurant,and there are no table number.");
            if ((order.OrderType == enOrderType.Takeaway || order.OrderType == enOrderType.Delivery) && order.CustomerId == null)
                return Result.Failure("You cannont place an order outside the restaurant,and there are no customer.");


            orderToUpdate.CustomerId = order.CustomerId; 
            orderToUpdate.OrderType = order.OrderType;
            orderToUpdate.TableNumber = order.TableNumber;

            return await _unitOfWork.SaveChangesAsync() > 0? Result.Success(): Result.Failure("Something is wrong");
        }

        public async Task<Result> changeOrderStatus(int orderId ,enOrderStatus newStatus)
        {
            if (!await _unitOfWork.ordersRepository.isExistAsync(orderId))
                return Result.Failure("Order not found");
            var order = await _unitOfWork.ordersRepository.GetOrderAsync(orderId);
           if(order.Status == enOrderStatus.Completed || order.Status == enOrderStatus.Cancelled)
                return Result.Failure("You cannot modify the order status once it is completed or cancelled");

            if (newStatus == enOrderStatus.Cancelled )
            {
                foreach (var item in order.OrderItems)
                {
                    var product = await _unitOfWork.productRepository.GetByIdAsync(item.ProductId);
                    product.Quantity += item.Quantity;
                    product.IsAvailable = true;
                }
            }
            order.Status = newStatus;
            return await _unitOfWork.SaveChangesAsync() > 0
                ? Result.Success()
                : Result.Failure("Something went wrong");
        }

        public async Task<Result<int>> AddNewOrderItem(int orderId,CreateOrderItemDto newOrderItem)
        {
            if (!await _unitOfWork.ordersRepository.isExistAsync(orderId) || !await _unitOfWork.productRepository.isExistAsync(newOrderItem.ProductId))
                return Result<int>.Failure("order / product invalid");

            var Product = await _unitOfWork.productRepository.GetByIdAsync(newOrderItem.ProductId);

            if (Product == null)
                return Result<int>.Failure("Product is not exist");

            if (!Product.IsAvailable)
                return Result<int>.Failure("Product is not available");

            if (Product.Quantity < newOrderItem.Quantity)
                return Result<int>.Failure("this quantity requested is currently unavaliable");

            var Order = await _unitOfWork.ordersRepository.GetByIdAsync(orderId);

            if (Order == null)
                return Result<int>.Failure("Order is not exist");
            
            if (Order.Status != enOrderStatus.Pending)
                return Result<int>.Failure("Cannot add items to an order that is not pending");


            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var OrderItem = new OrderItem
                {
                    OrderId = orderId,
                    ProductId = newOrderItem.ProductId,
                    UnitPrice = Product.Price,
                    Quantity = newOrderItem.Quantity
                };

                Product.Quantity -= newOrderItem.Quantity;

                Order.TotalAmount += (Product.Price * newOrderItem.Quantity);

                if (Product.Quantity == 0)
                    Product.IsAvailable = false;

                await _unitOfWork.orderItemsRepository.AddAsync(OrderItem);

                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();

                return Result<int>.Success(OrderItem.OrderItemId);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();

                return Result<int>.Failure("Something is wrong, try again");
            }
        }

       public async Task<Result> UpdateOrderItemQuantity(int orderId, int itemId, UpdateOrderItemDto dto)
        {
            var orderItem = await _unitOfWork.orderItemsRepository.GetByIdAsync(itemId);
            if (orderItem == null || orderItem.OrderId != orderId)
                return Result.Failure("Order item not found for this order");

            var order = await _unitOfWork.ordersRepository.GetByIdAsync(orderId);
            if (order.Status != enOrderStatus.Pending)
                return Result.Failure("Cannot modify items of an order that is not pending");

            var product = await _unitOfWork.productRepository.GetByIdAsync(orderItem.ProductId);

            int delta = dto.Quantity - orderItem.Quantity;  

            if (delta > 0 && product.Quantity < delta)
                return Result.Failure("Not enough stock for this increase");

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                product.Quantity -= delta;             
                product.IsAvailable = product.Quantity > 0;

                order.TotalAmount += orderItem.UnitPrice * delta;
                orderItem.Quantity = dto.Quantity;

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return Result.Success();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Result.Failure("Something went wrong, try again");
            }
        }
      
        public async Task<Result> DeleteOrderItem(int orderId, int itemId)
        {
            var orderItem = await _unitOfWork.orderItemsRepository.GetByIdAsync(itemId);
            if (orderItem == null || orderItem.OrderId != orderId)
                return Result.Failure("Order item not found for this order");

            var order = await _unitOfWork.ordersRepository.GetByIdAsync(orderId);
            if (order.Status != enOrderStatus.Pending)
                return Result.Failure("Cannot remove items from an order that is not pending");

            var product = await _unitOfWork.productRepository.GetByIdAsync(orderItem.ProductId);

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                product.Quantity += orderItem.Quantity;   
                product.IsAvailable = true;               
                order.TotalAmount -= orderItem.UnitPrice * orderItem.Quantity;

                _unitOfWork.orderItemsRepository.DeleteAsync(orderItem);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return Result.Success();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Result.Failure("Something went wrong, try again");
            }
        }
    }
}
