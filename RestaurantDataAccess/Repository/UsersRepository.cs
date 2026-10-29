using Microsoft.EntityFrameworkCore;
using RestaurantDataAccess.Data;
using RestaurantDataAccess.Entities;
using RestaurantDataAccess.InterfaceRepository;
using RestaurantShared.Pagination;
using RestaurantShared.Parameters;


namespace RestaurantDataAccess.Repository
{
    public class UsersRepository : GenericRepository<User>, IUsersRepository
    {

        private readonly RestaurantDbContext _context;

        public UsersRepository(RestaurantDbContext context) : base(context)
        {
            _context = context;
        }




        public async Task<PagedResult<User>> GetAllUsersAsync(UserParameter parameter)
        {
            var query = _context.Users.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(parameter.UserName))
            {
                query = query.Where(u => u.Username.Contains(parameter.UserName));
            }

            if (parameter.isActive.HasValue)
                query = query.Where(u => u.IsActive == parameter.isActive);

            var totalCount = await query.CountAsync();

            var users = await query
                .Skip((parameter.PageNumber - 1)
                    * parameter.PageSize)
                .Take(parameter.PageSize)
                .Include(u => u.Person)
                .ToListAsync();

            return new PagedResult<User>
            {
                Items = users,
                TotalCount = totalCount,
                PageNumber = parameter.PageNumber,
                PageSize = parameter.PageSize
            };
        }

        public async Task<User> GetUserAsync(int UserId)
            => await _context.Users
                             .Include(u => u.Person)
                             .SingleOrDefaultAsync(u => u.UserId == UserId);

        public async Task<User> GetUserAsync(string Username)
            => await _context.Users
                             .Include(u => u.Person)
                             .SingleOrDefaultAsync(u => u.Username == Username);

        public async Task<UserRole> GetUserRole(int userId, int roleId)
            => await _context.UserRoles
                             .Include(ur => ur.User)
                             .Include(ur => ur.Role)
                             .AsNoTracking()
                             .SingleOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);


        public async Task<List<UserRole>> GetUserRoles(int userId)
            => await _context.UserRoles.AsNoTracking()
                                       .Include(ur => ur.User)
                                       .Include(ur => ur.Role)
                                       .Where(ur => ur.UserId == userId)
                                       .ToListAsync();


        public async Task<bool> isUserExistAsync(int UserId)
            => await _context.Users.AnyAsync(u => u.UserId == UserId);

        public async Task<bool> isUserExistAsync(string Username)
            => await _context.Users.AnyAsync(u => u.Username == Username);

        public async Task<bool> isUserRoleExist(int UserRolesId)
            => await _context.UserRoles.AnyAsync(ur => ur.UserRolesId == UserRolesId);

        public void RemoveRole(UserRole userRole)
            => _context.UserRoles.Remove(userRole);


        public async Task<bool> isRoleExist(int RoleId)
            => await _context.Roles.AnyAsync(r => r.RoleId == RoleId);

        public async Task AssignRole(UserRole userRoles)
            => await _context.UserRoles.AddAsync(userRoles);

        public async Task<User> GetUserByPersonId(int personId)
            => await _context.Users
                             .Include(u => u.Person)
                             .SingleOrDefaultAsync(u => u.PersonId == personId);
        public async Task<bool> isUserHasPersonId(int personId) => 
            await _context.Users.AnyAsync(u=>u.PersonId== personId);
     
    }
}
