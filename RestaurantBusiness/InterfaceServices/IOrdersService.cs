
using RestaurantShared.DTOs.OrderItemsDTOs;
using RestaurantShared.DTOs.OrdersDTOs;
using RestaurantShared.Enums;
using RestaurantShared.Results;

namespace RestaurantBusiness.InterfaceServices
{
    public interface IOrdersService
    {

        public Task<Result<List<ReadOrdersDto>>> GetOrdersAsync();

        public Task<Result<ReadOrdersDto>> GetOrderAsync(int OrderId);

        public Task<bool> isOrderExistAsync(int OrderId);

        public Task<Result<int>> AddNewOrderAsync(CreateOrderDto newOrder);

        public Task<Result> UpdateOrderAsync(int orderId, UpdateOrderDto order);

        public Task<Result> DeleteOrder(int OrderId);
        public Task<Result> changeOrderStatus(int orderId,enOrderStatus newStatus);
        public  Task<Result<int>> AddNewOrderItem(int orderId, CreateOrderItemDto newOrderItem);
        public Task<Result> UpdateOrderItemQuantity(int orderId, int itemId, UpdateOrderItemDto dto);
        public Task<Result> DeleteOrderItem(int orderId, int itemId);
    }
}
