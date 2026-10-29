using RestaurantDataAccess.Entities;
using RestaurantShared.Pagination;
using RestaurantShared.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantDataAccess.InterfaceRepository
{
    public interface IEmployeeRepository:IGenericRepository<Employee>
    {

        public Task<PagedResult<Employee>> GetEmployeesAsync(EmployeeParameter parameter);

        public Task<Employee> GetEmployeeAsync(int EmployeeId);
        public Task<bool> isUserIdHasEmployee(int userId);
        public Task<Employee?> GetEmployeeByUserIdAsync(int UserId);
    }
}
