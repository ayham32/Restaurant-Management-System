
using RestaurantBusiness.InterfaceServices;
using RestaurantDataAccess.Entities;
using RestaurantShared.DTOs.UserRolesDTOs;
using RestaurantShared.DTOs.UsersDTOs;
using RestaurantShared.Pagination;
using RestaurantShared.Parameters;
using RestaurantShared.Results;
using RestuarantDataAccess.UintOfWork;

namespace RestaurantBusiness.Services
{
    public class UsersService: IUsersService
    {
        private readonly IUnitOfWork _uintOfWork;
        private readonly ICurrentUserService _currentUserService;

        public UsersService(IUnitOfWork uintOfWork, ICurrentUserService currentUserService)
        {
            _uintOfWork = uintOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<int>> AddNewUser(CreateUserDto newUser)
        {
            if (newUser == null)
                return Result<int>.Failure("must be contins a value");

            if (!await _uintOfWork.peopleRepository.isExistAsync(newUser.PersonId))
                return Result<int>.Failure($"No exist person with id: {newUser.PersonId}");
            
            if(await _uintOfWork.usersRepository.isUserHasPersonId(newUser.PersonId))
                return Result<int>.Failure($"This Id: {newUser.PersonId} ,is already in use");

            if (await isUserExist(newUser.Username))
                return Result<int>.Failure("Username is already exists.");

            try
            {
                await _uintOfWork.BeginTransactionAsync();
                var User = new User
                {
                    PersonId = newUser.PersonId,
                    Username = newUser.Username,
                    HashPassword = BCrypt.Net.BCrypt.HashPassword(newUser.Pass),
                    IsActive = newUser.IsActive,
                };

                await _uintOfWork.usersRepository.AddAsync(User);
                await _uintOfWork.SaveChangesAsync();

                var userRole = new UserRole
                {
                    UserId = User.UserId,
                    RoleId = newUser.RoleId
                };

                await _uintOfWork.usersRepository.AssignRole(userRole);
                await _uintOfWork.SaveChangesAsync();
                await _uintOfWork.CommitTransactionAsync();
                return   Result<int>.Success(User.UserId);
            }
            catch  {
                await _uintOfWork.RollbackTransactionAsync();
                return Result<int>.Failure("Something is wrong, try again");
            }
        }

        public async Task<Result> DeleteUser(int UserId)
        {
            if (UserId <= 0)
                return Result.Failure("Id must be a positive");

            var User = await _uintOfWork.usersRepository.GetByIdAsync(UserId);

            if (User == null)
                return Result.Failure("User is null");

            _uintOfWork.usersRepository.DeleteAsync(User);

            return await _uintOfWork.SaveChangesAsync() >0?Result.Success():Result.Failure("Something is wrong");
        }

        public async Task<Result<PagedResult<ReadUsersDto>>> GetAllUsersAsync(UserParameter parameter)
        {
            var Users = await _uintOfWork.usersRepository.GetAllUsersAsync(parameter);

            if (Users == null)
                return Result<PagedResult<ReadUsersDto>>.Failure("No users exists");

            var UsersDto = Users.Items.Select(u => new ReadUsersDto
            {
                UserId = u.UserId,
                PersonId = u.PersonId,
                FullName = u.Person.FirstName+ " "+u.Person.LastName,
                NationalNo = u.Person.NationalNo,
                Username = u.Username,
                IsActive = u.IsActive
            }).ToList();

            var pagedResult = new PagedResult<ReadUsersDto>
            {
                Items = UsersDto,
                PageNumber = parameter.PageNumber,
                PageSize = parameter.PageSize,
                TotalCount = Users.TotalCount


            };

            return Result<PagedResult<ReadUsersDto>>.Success(pagedResult);
        }

        public async Task<Result<ReadUsersDto>?> GetUserAsync(int UserId)
        {
            if (UserId <= 0)
                return Result<ReadUsersDto>.Failure("Id must be a postive");

            var User = await _uintOfWork.usersRepository.GetUserAsync(UserId);

            if (User == null)
                return Result<ReadUsersDto>.Failure("User is not exist"); 

            var UserDto = new ReadUsersDto
            {
                UserId = User.UserId,
                PersonId = User.PersonId,
                NationalNo = User.Person.NationalNo,
                FullName = User.Person.FirstName+" "+User.Person.LastName,
                Username = User.Username,
                IsActive = User.IsActive

            };

            return  Result<ReadUsersDto>.Success(UserDto);

        }

        public async Task<Result<ReadUsersDto>> GetUserAsync(string Username)
        {
            if (string.IsNullOrWhiteSpace(Username))
                return Result<ReadUsersDto>.Failure("Username must`nt be a empty");

            var User = await _uintOfWork.usersRepository.GetUserAsync(Username);

            if (User == null)
                return Result<ReadUsersDto>.Failure("User is not exist");

            var UserDto = new ReadUsersDto
            {
                UserId = User.UserId,
                PersonId = User.PersonId,
                NationalNo = User.Person.NationalNo,
                FullName = User.Person.FirstName + " " + User.Person.LastName,
                Username = User.Username,
                IsActive = User.IsActive

            };

            return Result<ReadUsersDto>.Success(UserDto);
        }

        public async Task<bool> isUserExist(int UserId)
        {
            if (UserId <= 0)
                return false;

            return await _uintOfWork.usersRepository.isExistAsync(UserId);

        }

        public async Task<bool> isUserExist(string Username)
        {
            if (string.IsNullOrWhiteSpace(Username))
                return false;

            return await _uintOfWork.usersRepository.isUserExistAsync(Username);
        }

        public async Task<Result> UpdateUser(int UserId, UpdateUserDto user)
        {
            if (UserId <= 0)
                return Result.Failure("Id must be a postive");

            if (user == null)
                return Result.Failure("User must`nt be a null");
            try {
                await _uintOfWork.BeginTransactionAsync();
                var UserToUpdate = await _uintOfWork.usersRepository.GetByIdAsync(UserId);
                var userRole = await _uintOfWork.usersRepository.GetUserRoles(UserId);
                if (UserToUpdate == null)
                    return Result.Failure("No user with Id");

                if (UserToUpdate.Username != user.Username)
                    if (await isUserExist(user.Username))
                        return Result.Failure("This username belongs to someone else");
                userRole[0].RoleId = user.roleId;
                UserToUpdate.Username = user.Username;
                UserToUpdate.IsActive = user.IsActive;

                await _uintOfWork.SaveChangesAsync();
                await _uintOfWork.CommitTransactionAsync();
                return Result.Success();
                }
            catch {
                await _uintOfWork.RollbackTransactionAsync();
                return Result.Failure("Something is wrong");
            }
            }

        public async Task<Result> AssignRole(CreateUserRolesDto userRoles)
        {
            if (userRoles == null || !await isUserExist(userRoles.UserId) ||! await isRoleExist(userRoles.RoleId))
                return Result.Failure("Make sure userId, roleId");

            var UserRoles = new UserRole
            {
                UserId = userRoles.UserId,
                RoleId = userRoles.RoleId
            };

            await  _uintOfWork.usersRepository.AssignRole(UserRoles);
            return await _uintOfWork.SaveChangesAsync() > 0? Result.Success():Result.Failure("Something is wrong");
        }

        public async Task<Result> RemoveRole(int userId , int roleId)
        {
            if (userId <= 0|| roleId<=0)
                return Result.Failure("Id must be a postive");

            var UserRoles = await _uintOfWork.usersRepository.GetUserRole(userId,roleId);

            if (UserRoles == null)
                return Result.Failure("No any exist role to this user");

            _uintOfWork.usersRepository.RemoveRole(UserRoles);

            return await _uintOfWork.SaveChangesAsync() > 0 ? Result.Success() : Result.Failure("Something is wrong");
        }

        public async Task<Result<List<ReadUserRolesDto>>> GetUserRoles(int userId)
        {
            if (userId <= 0)
                return Result<List<ReadUserRolesDto>>.Failure("Id must be a postive");


            var userRoles = await _uintOfWork.usersRepository.GetUserRoles(userId);

            if(userRoles == null)
                return Result<List<ReadUserRolesDto>>.Failure("No exist user roles");

            var userRolesDto = userRoles.Select(ur => new ReadUserRolesDto
            {
                UserRolesId = ur.UserRolesId,
                RoleId = ur.RoleId,
                UserId = ur.UserId
            }).ToList();

            return Result<List<ReadUserRolesDto>>.Success(userRolesDto);
        }

        public async Task<Result<ReadUserRolesDto>> GetUserRole(int userId, int roleId)
        {
            if (userId <= 0 || roleId <= 0)
                return Result<ReadUserRolesDto>.Failure("Id must be a postive");

            var userRole = await _uintOfWork.usersRepository.GetUserRole(userId, roleId);

            if (userRole == null)
                return Result<ReadUserRolesDto>.Failure("No exist user role");

            var userRoleDto = new ReadUserRolesDto
            {
                UserRolesId = userRole.UserRolesId,
                RoleId = roleId,
                UserId = userId
            };

            return Result<ReadUserRolesDto>.Success(userRoleDto);
        }

        public async Task<bool> isUserRoleExist(int UserRolesId)
        {
            if (UserRolesId <= 0)
                return false;

            return await _uintOfWork.usersRepository.isUserRoleExist(UserRolesId);
        }

        public async Task<bool> isRoleExist(int RoleId)
        {
            if (RoleId <= 0)
                return false;

            return await _uintOfWork.usersRepository.isRoleExist(RoleId);
        }

        public async Task<Result<ReadUsersDto>> GetUserByPersonId(int PersonId)
        {
            var User = await _uintOfWork.usersRepository.GetUserByPersonId(PersonId);

            if (User == null)
                return Result<ReadUsersDto>.Failure("User is not exist");

            var UserDto = new ReadUsersDto
            {
                UserId = User.UserId,
                PersonId = User.PersonId,
                NationalNo = User.Person.NationalNo,
                FullName = User.Person.FirstName + " " + User.Person.LastName,
                Username = User.Username,
                IsActive = User.IsActive

            };

            return Result<ReadUsersDto>.Success(UserDto);
        }
        public async Task<Result> changePassword(ChangePasswordDto changePassword)
        {
            var userId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("The user is not authorized, or the session has expired.");
            }

            var User = await _uintOfWork.usersRepository.GetByIdAsync(int.Parse(userId));

            if (User == null)
                return Result.Failure("user is null");
            
            if (!BCrypt.Net.BCrypt.Verify(changePassword.oldPassword, User.HashPassword))
                return Result.Failure("Old password is incorrect");

            User.HashPassword = BCrypt.Net.BCrypt.HashPassword(changePassword.newPassword);
            return await _uintOfWork.SaveChangesAsync() > 0 ? Result.Success() : Result.Failure("Somethings is wrong");
        }
    }
}
