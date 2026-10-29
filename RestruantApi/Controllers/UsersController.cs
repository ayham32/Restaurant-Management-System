
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantBusiness.InterfaceServices;
using RestaurantDataAccess.Entities;
using RestaurantShared.DTOs.UserRolesDTOs;
using RestaurantShared.DTOs.UsersDTOs;
using RestaurantShared.Pagination;
using RestaurantShared.Parameters;
using RestaurantShared.Results;

namespace RestaurantApi.Controllers
{
    [Authorize]
    [Route("api/Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _user;
        public UsersController(IUsersService user) => _user = user;

        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<Result<PagedResult<ReadUsersDto>>>> GetAllUsers([FromQuery] UserParameter parameter)
        {
            var allUsers = await _user.GetAllUsersAsync(parameter);

            return !allUsers.IsSuccess ? NotFound(allUsers.ErrorMessage) : Ok(allUsers);

        }


        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("GetUserWithId/{id}")]
        public async Task<ActionResult<Result<ReadUsersDto>>> GetUser(int id)
        {
            
            var user = await _user.GetUserAsync(id);

            if (user == null)
                return NotFound(user.ErrorMessage);
            
            bool isntAllow = await IUsersService.isAllow(id, User);

            if (isntAllow) 
                return Forbid();

            return !user.IsSuccess ? NotFound(user.ErrorMessage) : Ok(user.Data);

        }

    
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("GetUserWithUsername/{Username}")]
        public async Task<ActionResult<ReadUsersDto>> GetUser(string Username)
        {
            

            var user = await _user.GetUserAsync(Username);

            if (user == null)
                return NotFound(user.ErrorMessage) ;

            bool isntAllow = await IUsersService.isAllow(user.Data.UserId, User);

            if (isntAllow)
                return Forbid();


            return  Ok(user.Data);

        }

        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("isUserExistsWithId/{id}")]
        public async Task<IActionResult> isUserExists(int id)
        {
            if (id <= 0)
                return BadRequest("id must be postive integer.");

            var User = await _user.isUserExist(id);


            return !User ? NotFound($"No user with id: {id}") : Ok("Yes is exists");

        }


        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("isUserExistsWithUsername/{Username}")]
        public async Task<IActionResult> isUserExists(string Username)
        {
            if (string.IsNullOrEmpty(Username))
                return BadRequest("Somthing is wrong about username.");

            var User = await _user.isUserExist(Username);


            return !User ? NotFound($"No user with username: {Username}") : Ok("Yes is exists");

        }

        [Authorize(Roles = "Admin,Employee")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("AddNewUser")]
        public async Task<IActionResult> AddNewUser(CreateUserDto newUser)
        {
            if (newUser == null)
                return BadRequest("is requerid.");

            var result = await _user.AddNewUser(newUser);

            return result.IsSuccess ?Created("" , result.Data) : BadRequest(result.ErrorMessage);

        }

        [Authorize(Roles = "Admin,Employee")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("UpdateUser/{id}")]
        public async Task<IActionResult> UpdateUser(int id,UpdateUserDto userDto)
        {

            var result = await _user.UpdateUser(id, userDto);

            return result.IsSuccess ? Ok("Updated Successfully.") : BadRequest(result.ErrorMessage);

        }

        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {

            var result = await _user.DeleteUser(id);
            return result.IsSuccess ? Ok("Deleted Successfully.") : BadRequest(result.ErrorMessage);
        }

        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("AddNewRole")]
        public async Task<IActionResult> AssignRole(CreateUserRolesDto newUserRole)
        {

            var result = await _user.AssignRole(newUserRole);

            return result.IsSuccess
                ?Created()
                : BadRequest(result.ErrorMessage);
        }

        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpDelete("Remove/{userId}/roles/{roleId}")]
        public async Task<IActionResult> RemoveRole(int userId , int roleId)
        {
            var result = await _user.RemoveRole(userId, roleId);


            return result.IsSuccess ? Ok("Deleted Successfully.") : BadRequest(result.ErrorMessage);



        }


        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("Role/{userId}")]
        public async Task<ActionResult< Result<IEnumerable<ReadUserRolesDto>>>> GetUserRoles(int userId)
        {
          
            var userRoles = await _user.GetUserRoles(userId);
            
            return !userRoles.IsSuccess? BadRequest(userRoles.ErrorMessage) : Ok(userRoles.Data); 

        }

        [HttpPut("ChangePassword")]
        public async Task<ActionResult<Result>> ChangePassword(ChangePasswordDto changePassword)
        {
            

            return await _user.changePassword(changePassword);
          
        } 
    }
}
