using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantBusiness.InterfaceServices;
using RestaurantShared.DTOs.EmployeesDTOs;
using RestaurantShared.Parameters;

namespace RestaurantApi.Controllers
{
    [Authorize]
    [Route("api/Employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {

        private readonly IEmployeesService _employee;

        public EmployeesController(IEmployeesService employee)
        {
            _employee = employee;
        }

        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("AllEmployees")]
        public async Task<ActionResult<IEnumerable<ReadEmployeesDto>>> GetAllEmployees([FromQuery] EmployeeParameter parameter)
        {
            var employees = await _employee.GetEmployeesAsync(parameter);

            return !employees.IsSuccess? NotFound(employees.ErrorMessage) : Ok(employees.Data);
        }


        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("Employee/{Id}")]
        public async Task<ActionResult<ReadEmployeesDto>> GetEmployee(int Id)
        {
            
            var employee = await _employee.GetEmployeeAsync(Id);
            
            if (employee == null)
                return NotFound(employee.ErrorMessage);

            var isntAllow = await IUsersService.isAllow(employee.Data.User.UserId, User);

            if (isntAllow)
                return Forbid();

            return Ok(employee.Data);
        }


        [Authorize(Roles ="Admin")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
       

        [HttpGet("isEmployeeExists/{Id}")]
        public async Task<IActionResult> isEmployeeExist(int Id)
        {
            if (Id <= 0)
                return BadRequest("You cannot enter a id less than one.");


            return await _employee.isEmployeeExistAsync(Id) ? Ok("Employee is exists") : NotFound($"No employee with id: {Id}");

        }

        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPost("AddNewEmployee")]
        public async Task<IActionResult> AddNewEmployee([FromBody] CreateEmployeeDto newEmployee)
        {

            var result = await _employee.AddNewEmployee(newEmployee);

            return result.IsSuccess? Created("" , result.Data) : BadRequest(result.ErrorMessage);
        }

        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut("UpdateEmployee/{Id}")]
        public async Task<IActionResult> UpdateEmployee(int Id , [FromBody] UpdateEmployeeDto employee)
        {
            var result = await _employee.UpdateEmployee(Id, employee);

            return result.IsSuccess ? Ok("Updated successfully.") : BadRequest(result.ErrorMessage);
        }


        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpDelete("DeleteEmployee/{Id}")]
        public async Task<IActionResult> DeleteEmployee(int Id)
        {
            var result = await _employee.DeleteEmployee(Id);

            return result.IsSuccess? Ok("Deleted successfully.") : BadRequest(result.ErrorMessage);
        }

    }


}
