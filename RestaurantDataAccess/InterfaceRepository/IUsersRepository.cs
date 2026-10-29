using RestaurantDataAccess.Entities;
using RestaurantShared.DTOs.UsersDTOs;
using RestaurantShared.Pagination;
using RestaurantShared.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantDataAccess.InterfaceRepository
{
    public interface IUsersRepository:IGenericRepository<User>
    {

        public Task<PagedResult<User>> GetAllUsersAsync(UserParameter parameter);
        
        public Task<User> GetUserAsync(string Username);
        public Task<User> GetUserAsync(int UserId);

        public Task<bool> isUserExistAsync(string Username);

        public Task AssignRole(UserRole userRoles);

        public void RemoveRole(UserRole userRole);
        
        public Task<List<UserRole>> GetUserRoles(int userId);
        
        public Task<UserRole> GetUserRole(int userId, int roleId);

        public Task<bool> isUserRoleExist(int UserRolesId);

        public Task<bool> isRoleExist(int RoleId);

        public Task<User> GetUserByPersonId(int personId);

        public Task<bool> isUserHasPersonId(int personId);
       
    }
}
