
using RestaurantBusiness.InterfaceServices;
using RestaurantDataAccess.Entities;
using RestaurantShared.DTOs.BillsDTOs;
using RestaurantShared.Enums;
using RestaurantShared.Results;
using RestuarantDataAccess.UintOfWork;

namespace RestaurantBusiness.Services
{
    public class BillServices : IBillServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        public BillServices(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<int>> AddNewBill(CreateBillDto newBill)
        {
            var userId =_currentUserService.UserId;
            if (string.IsNullOrEmpty(userId))
            {
                return Result<int>.Failure("The user is not authorized, or the session has expired.");
            }

            if (! await _unitOfWork.ordersRepository.isExistAsync(newBill.OrderId))
                return Result<int>.Failure("No exist order");
          
            if (await _unitOfWork.billRepository.IsExistByOrderIdAsync(newBill.OrderId))
                return Result<int>.Failure("This order already has a bill");
            
            var Order = await _unitOfWork.ordersRepository.GetByIdAsync(newBill.OrderId);

            if (Order == null)
                return Result<int>.Failure("No exist order");

            if (Order.Status == enOrderStatus.Cancelled)
                return Result<int>.Failure("Cannot create a bill for a cancelled order");

            var employee = await _unitOfWork.empolyeesRepository.GetEmployeeByUserIdAsync(int.Parse(userId));
            if (employee == null)
                return Result<int>.Failure("No employee");

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var bill = new Bill
                {
                    CreatedBy = employee.EmployeeId,
                    IssuedAt = DateTime.Now,
                    Discount = newBill.Discount,
                    OrderId = newBill.OrderId,
                    PaymentMethod = newBill.PaymentMethod,
                    PaymentStatus = newBill.PaymentStatus,
                    SubTotal = Order.TotalAmount??0,
                    TaxAmount = newBill.TaxAmount,
                };



                var totalBeforeDiscount = bill.SubTotal + bill.TaxAmount;

                var discountAmount =
                    totalBeforeDiscount * (bill.Discount / 100);

                bill.FinalAmount = totalBeforeDiscount - discountAmount;
                await _unitOfWork.billRepository.AddAsync(bill);

                Order.Status = enOrderStatus.Completed;

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
                return Result<int>.Success(bill.BillId);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Result<int>.Failure("Something is wrong");
                
            }
        }

        public async Task<Result> DeleteBill(int billId)
        {
            if (billId <= 0)
                return Result.Failure("Id must ba a postive");

            var bill = await _unitOfWork.billRepository.GetByIdAsync(billId);

            if (bill == null)
                return Result.Failure("Bill is not exist");

            _unitOfWork.billRepository.DeleteAsync(bill);

            return await _unitOfWork.SaveChangesAsync()>0 ? Result.Success():Result.Failure("Something is wrong");
            
        }

        public async Task<Result<ReadBillsDto>> GetBillAsync(int BillId)
        {
            if (BillId <= 0)
                return Result<ReadBillsDto>.Failure("Id must be a postive");

            var bill = await _unitOfWork.billRepository.GetBillAsync(BillId);


            if (bill == null)
                return Result<ReadBillsDto>.Failure("Bill is not exist");

            var billDto = new ReadBillsDto
            {
                BillId = BillId,
                CreatedBy = bill.CreatedByNavigation?.User?.Username,
                Discount = bill.Discount,
                FinalAmount = bill.FinalAmount,
                IssuedAt = bill.IssuedAt,
                OrderId = bill.OrderId,
                PaymentMethod = bill.PaymentMethod,
                PaymentStatus = bill.PaymentStatus,
                SubTotal = bill.SubTotal,
                TaxAmount = bill.TaxAmount
            };

            return Result<ReadBillsDto>.Success(billDto);
        
        }

        public async Task<Result<List<ReadBillsDto>>> GetBillsAsync()
        {
            var bills = await _unitOfWork.billRepository.GetBillsAsync();

            if (bills == null)
                return Result<List<ReadBillsDto>>.Failure("There are no bills");

            var billsDto = bills.Select(b => new ReadBillsDto
            {
                BillId = b.BillId,
                CreatedBy = b.CreatedByNavigation?.User?.Username,
                Discount = b.Discount,
                FinalAmount = b.FinalAmount,
                IssuedAt = b.IssuedAt,
                OrderId = b.OrderId,
                PaymentMethod = b.PaymentMethod,
                PaymentStatus = b.PaymentStatus,
                SubTotal = b.SubTotal,
                TaxAmount = b.TaxAmount

            }).ToList();

            return Result<List<ReadBillsDto>>.Success(billsDto);
        
        }

        public async Task<Result> UpdateBill(int billId, UpdateBillDto bill)
        {
            if (billId <= 0)
                return Result.Failure("Id must be a postive");

            var billToUpdate = await _unitOfWork.billRepository.GetByIdAsync(billId);

            if (billToUpdate == null)
                return Result.Failure("No exist bill with this id");

            billToUpdate.PaymentStatus= bill.PaymentStatus;
            billToUpdate.PaymentMethod= bill.PaymentMethod;

            return await _unitOfWork.SaveChangesAsync() > 0 ? Result.Success(): Result.Failure("Something is wrong") ;
        
        }
    }
}
