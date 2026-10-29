
using RestaurantBusiness.InterfaceServices;
using RestaurantDataAccess.Entities;
using RestaurantShared.DTOs.CustomersDTOs;
using RestaurantShared.DTOs.UsersDTOs;
using RestaurantShared.Enums;
using RestaurantShared.Results;
using RestuarantDataAccess.UintOfWork;

namespace RestaurantBusiness.Services
{
    public class CustomersService: ICustomersService
    {
        private readonly IUnitOfWork _uintOfWork;

        public CustomersService(IUnitOfWork uintOfWork)
        {
            _uintOfWork = uintOfWork;
        }

        public async Task<Result<int>> AddNewCustomer(CreateCustomerDto newCustomer)
        {
            
            
            try
            {
                await _uintOfWork.BeginTransactionAsync();

                var Customer = new Customer
                {
                    Name=newCustomer.Name,
                    Phone = newCustomer.Phone,
                     Address = newCustomer.Address

                };

                await _uintOfWork.customerRepository.AddAsync(Customer);

               

                
                await _uintOfWork.SaveChangesAsync();

                await _uintOfWork.CommitTransactionAsync();
                
                return Result<int>.Success(Customer.CustomerId);
            }

            catch
            {
                await _uintOfWork.RollbackTransactionAsync();
                return Result<int>.Failure("Something is wrong");
            }


        }

        public async Task<Result> DeleteCustomer(int CustomerId)
        {
            if (CustomerId <= 0)
                return Result.Failure("Id must be is postive");

            var customer = await _uintOfWork.customerRepository.GetByIdAsync(CustomerId);

            if(customer == null)
                return Result.Failure("No custome with this id");

            try{
               
                _uintOfWork.customerRepository.DeleteAsync(customer);

           
                               
                await _uintOfWork.SaveChangesAsync();

                
                return Result.Success();
            }

            catch
            {
                
                return Result.Failure("Something is wrong");
            }
            
        }

        public async Task<Result<ReadCustomersDto>?> GetCustomerAsync(int CustomerId)
        {
            if (CustomerId <= 0)
                return Result<ReadCustomersDto>.Failure("Id must be is a postive");

            var Customer = await _uintOfWork.customerRepository.GetCustomerAsync(CustomerId);

            if (Customer == null)
                return Result<ReadCustomersDto>.Failure("no customer with this id");

            var CustomerDto = new ReadCustomersDto
            {
                CustomerId = CustomerId,
                Name = Customer.Name,
                Phone = Customer.Phone,
                Address = Customer.Address
            };


            return Result<ReadCustomersDto>.Success(CustomerDto);
        }

        public async Task<Result<List<ReadCustomersDto>>> GetCustomersAsync()
        {
            var Customers = await _uintOfWork.customerRepository.GetAllCustomersAsync();

            if (Customers == null)
                return Result<List<ReadCustomersDto>>.Failure("No there are customes");

            var CustomersDto = Customers.Select(c => new ReadCustomersDto
            {
                CustomerId = c.CustomerId,
                Name=c.Name,
                Phone=c.Phone,
                Address = c.Address

            }).ToList();

            return Result<List<ReadCustomersDto>>.Success(CustomersDto);
        }

        public async Task<bool> isCustomerExistAsync(int CustomerId)
        {
            if (CustomerId <= 0)
                return false;

            return await _uintOfWork.customerRepository.isExistAsync(CustomerId);
        }

        public async Task<Result> UpdateCustomerAsync(int customerId, UpdateCustomerDto customer)
        {
            if (customerId <= 0 )
                return Result.Failure("Id must be a postive");

            var customerToUpdate = await _uintOfWork.customerRepository.GetByIdAsync(customerId);

            if (customerToUpdate == null)
                return Result.Failure("No custome with this id");

            customerToUpdate.Name=customer.Name;
            customerToUpdate.Phone = customer.Phone;
            customerToUpdate.Address = customer.Address;

            return await _uintOfWork.SaveChangesAsync() >0? Result.Success():Result.Failure("Something is wrong");

        }
    }
}
