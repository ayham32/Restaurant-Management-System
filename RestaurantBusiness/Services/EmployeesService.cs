
using RestaurantBusiness.InterfaceServices;
using RestaurantDataAccess.Entities;
using RestaurantShared.DTOs.EmployeesDTOs;
using RestaurantShared.DTOs.UsersDTOs;
using RestaurantShared.Enums;
using RestaurantShared.Pagination;
using RestaurantShared.Parameters;
using RestaurantShared.Results;
using RestuarantDataAccess.UintOfWork;

namespace RestaurantBusiness.Services
{
    public class EmployeesService: IEmployeesService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
        }

        public async Task<Result<int>> AddNewEmployee(CreateEmployeeDto newEmployee)
        {
            if (!await _unitOfWork.usersRepository.isExistAsync(newEmployee.UserId))
                return Result<int>.Failure("User is not exist");
            if(await _unitOfWork.empolyeesRepository.isUserIdHasEmployee(newEmployee.UserId))
                return Result<int>.Failure("This user id is alredy used");

            try
            {

                await _unitOfWork.BeginTransactionAsync();
                
                var employee = new Employee
                {
                    UserId = newEmployee.UserId,
                    ManagerId = newEmployee.ManagerId == 0 ? null : newEmployee.ManagerId,
                    Salary = newEmployee.Salary,
                    StartTime = newEmployee.StartTime,
                    EndTime = newEmployee.EndTime

                };


                await _unitOfWork.empolyeesRepository.AddAsync(employee);

                //var employeeRole = new UserRole
                //{
                //    UserId = employee.UserId,
                //    RoleId = (int)enRoles.Employee
                //};

                //await _unitOfWork.usersRepository.AssignRole(employeeRole);


                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();
                return Result<int>.Success(employee.EmployeeId);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Result<int>.Failure("Smoething is wrong");
            }
        }

        public async Task<Result> DeleteEmployee(int EmployeeId)
        {
            if (EmployeeId <= 0)
                return Result.Failure("Id must be a postive");


            var Employee = await _unitOfWork.empolyeesRepository.GetByIdAsync(EmployeeId);

            _unitOfWork.empolyeesRepository.DeleteAsync(Employee);
            
            return await _unitOfWork.SaveChangesAsync() >0? Result.Success():Result.Failure("Something is wrong");
        
        }

        public async Task<Result<ReadEmployeesDto>>? GetEmployeeAsync(int EmployeeId)
        {
            if (EmployeeId <= 0)
                return Result<ReadEmployeesDto>.Failure("Id must be a positve");

            var Employee =await _unitOfWork.empolyeesRepository.GetEmployeeAsync(EmployeeId);

            if (Employee == null)
                return Result<ReadEmployeesDto>.Failure("No exist employee with this id");

            var EmployeeDto = new ReadEmployeesDto
            {
                EmployeeId = EmployeeId,

                User = new ReadUsersDto
                {
                    UserId = Employee.UserId,
                    PersonId = Employee.User.PersonId,
                    Username = Employee.User.Username,
                    FullName = Employee.User.Person.FirstName + " " + Employee.User.Person.LastName,
                    NationalNo = Employee.User.Person.NationalNo,
                    IsActive = Employee.User.IsActive
                },

                ManagerBy = Employee.ManagerId,

                Salary = Employee.Salary,
                StartTime = Employee.StartTime,
                EndTime = Employee.EndTime
            };

            return Result<ReadEmployeesDto>.Success(EmployeeDto);
        }

        public async Task<Result<PagedResult<ReadEmployeesDto>>> GetEmployeesAsync(EmployeeParameter parameter)
        {

            var Employees = await _unitOfWork.empolyeesRepository.GetEmployeesAsync(parameter);

            if (Employees == null )
                return Result<PagedResult<ReadEmployeesDto>>.Failure("There are no employees");



            var EmployeesDto =  Employees.Items.Select( e => new ReadEmployeesDto
            {
                EmployeeId = e.EmployeeId,

                User = new ReadUsersDto
                {
                    UserId = e.UserId,
                    PersonId = e.User.PersonId,

                    Username = e.User.Username,
                    FullName = e.User.Person.FirstName + " " + e.User.Person.LastName,
                    NationalNo = e.User.Person.NationalNo,
                    IsActive = e.User.IsActive
                },

                ManagerBy = e.ManagerId,
                Salary = e.Salary,
                StartTime = e.StartTime,
                EndTime = e.EndTime

            }).ToList();

            var pagedResult = new PagedResult<ReadEmployeesDto>
            {
                Items = EmployeesDto,
                TotalCount = Employees.TotalCount,
                PageNumber = parameter.PageNumber,
                PageSize = parameter.PageSize,
            };

            return Result<PagedResult<ReadEmployeesDto>>.Success(pagedResult);
        }

        public async Task<bool> isEmployeeExistAsync(int EmployeeId)
        {
            if (EmployeeId <= 0)
                return false;

            return await _unitOfWork.empolyeesRepository.isExistAsync(EmployeeId);
        }

        public async Task<Result> UpdateEmployee(int EmployeeId, UpdateEmployeeDto employee)
        {
            if (EmployeeId<=0 )
                return Result.Failure("Id must be a postive");

            var EmployeeToUpdate = await _unitOfWork.empolyeesRepository.GetByIdAsync(EmployeeId);

            if (EmployeeToUpdate == null)
                return Result.Failure("No exist employee wiht this id");

            EmployeeToUpdate.ManagerId = employee.ManagerId;
            EmployeeToUpdate.Salary = employee.Salary;
            EmployeeToUpdate.StartTime = employee.StartTime;
            EmployeeToUpdate.EndTime = employee.EndTime;

            
            return await _unitOfWork.SaveChangesAsync() >0 ? Result.Success(): Result.Failure("Something is wrong");
        
        }
    }
}
