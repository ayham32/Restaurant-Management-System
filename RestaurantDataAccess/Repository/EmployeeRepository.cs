using Microsoft.EntityFrameworkCore;
using RestaurantDataAccess.Data;
using RestaurantDataAccess.Entities;
using RestaurantDataAccess.InterfaceRepository;
using RestaurantShared.Pagination;
using RestaurantShared.Parameters;

namespace RestaurantDataAccess.Repository
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly RestaurantDbContext _context;

        public EmployeeRepository(RestaurantDbContext context) : base(context)
        {
            _context = context;
        }




        public async Task<Employee?> GetEmployeeAsync(int EmployeeId)
            => await _context.Employees
                             .Include(e => e.User)
                             .ThenInclude(e => e.Person)
                             .SingleOrDefaultAsync(e => e.EmployeeId == EmployeeId);

        public async Task<Employee?> GetEmployeeByUserIdAsync(int UserId)
           => await _context.Employees
                            .Include(e => e.User)
                            .ThenInclude(e => e.Person)
                            .SingleOrDefaultAsync(e => e.UserId == UserId);

        public async Task<PagedResult<Employee>> GetEmployeesAsync(EmployeeParameter parameter)
        {
            var query = _context.Employees
                             .AsNoTracking()
                             .Include(e => e.User)
                             .ThenInclude(e => e.Person);

            var totalCount = await query.CountAsync();


            var employees = await query
                        .Skip((parameter.PageNumber - 1)
                            * parameter.PageSize)
                        .Take(parameter.PageSize)
                        .ToListAsync();

            return new PagedResult<Employee>
            {
                Items = employees,
                TotalCount = totalCount,
                PageNumber = parameter.PageNumber,
                PageSize = parameter.PageSize
            };

        }
        public async Task<bool> isUserIdHasEmployee(int userId)
            => await _context.Employees.AnyAsync(e => e.UserId == userId);
            




    }
}
