using RestaurantShared.DTOs.UserRolesDTOs;
using RestaurantShared.DTOs.UsersDTOs;
using RestaurantShared.Pagination;
using RestaurantShared.Parameters;
using RestaurantShared.Results;
using System.Security.Claims;

namespace RestaurantBusiness.InterfaceServices
{
    public interface IUsersService
    {
        public Task<Result<PagedResult<ReadUsersDto>>> GetAllUsersAsync(UserParameter parameter);
        
        public Task<Result<ReadUsersDto>> GetUserAsync(int UserId);
        
        public Task<Result<ReadUsersDto>> GetUserAsync(string Username);

        public Task<bool> isUserExist(int UserId);
        
        public Task<bool> isUserExist(string Username);

        public Task<Result<int>> AddNewUser(CreateUserDto newUser);

        public Task<Result> UpdateUser(int UserId,UpdateUserDto user);

        public Task<Result> DeleteUser(int UserId);

        public Task<Result> AssignRole(CreateUserRolesDto userRoles);

        public Task<Result> RemoveRole(int userId, int roleId);

        public Task<Result<List<ReadUserRolesDto>>> GetUserRoles(int userId);

        public Task<Result<ReadUserRolesDto>> GetUserRole(int userId, int roleId);
        
        public Task<bool> isUserRoleExist(int UserRolesId);

        public Task<bool> isRoleExist(int RoleId);

        public Task<Result<ReadUsersDto>> GetUserByPersonId(int PersonId);
        public Task<Result> changePassword(ChangePasswordDto changePassword);
        public static async Task<bool> isAllow(int Id , ClaimsPrincipal User) 
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier);


            var userRole = User.FindFirst(ClaimTypes.Role);
             

            int authenticatedStudentId = int.Parse(userId.Value);


            bool isAdmin = userRole.Value == "Admin";


            return !isAdmin && authenticatedStudentId != Id;
        }
    }

}
