using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantBusiness.InterfaceServices;
using RestaurantShared.DTOs.CustomersDTOs;

namespace RestaurantApi.Controllers
{
    [Authorize]
    [Route("api/Customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {

        private readonly ICustomersService _customer;

        public CustomersController(ICustomersService customer)
        {
            _customer = customer;
        }


        [Authorize(Roles = "Admin,Employee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("AllCustomers")]
        public async Task<ActionResult<IEnumerable<ReadCustomersDto>>> GetAllCustomers()
        {
            var customers = await _customer.GetCustomersAsync();

            return !customers.IsSuccess ? NotFound(customers.ErrorMessage) : Ok(customers.Data);
        }



        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Admin,Employee")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("Customer/{Id}")]
        public async Task<ActionResult<ReadCustomersDto>> GetCustomer(int Id)
        {
          
            var Customer = await _customer.GetCustomerAsync(Id);

           
            return !Customer.IsSuccess? NotFound(Customer.ErrorMessage) : Ok(Customer.Data);

        }

        [Authorize(Roles = "Admin,Employee")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("isCustomerExists/{Id}")]
        public async Task<ActionResult<ReadCustomersDto>> isCustomerExists(int Id)
        {
            if (Id <= 0)
                return BadRequest("You cannot enter a id less than one.");

            var Customer = await _customer.isCustomerExistAsync(Id);

            return !Customer ? NotFound($"No customer with id: {Id}") : Ok("Yse is exist");

        }



        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPost("AddNewCustomer")]
        public async Task<IActionResult> AddNewCustomer(CreateCustomerDto newCustomer)
        {

            var result = await _customer.AddNewCustomer(newCustomer);

            return result.IsSuccess ? Created("" , result.Data) : BadRequest(result.ErrorMessage);
        }



        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut("UpdateCustomer/{Id}")]
        public async Task<IActionResult> UpdateCustmoer (int Id,UpdateCustomerDto customer)
        {

            var result = await _customer.UpdateCustomerAsync(Id, customer);

            return  result.IsSuccess ? Ok("Updated is successfully.") : BadRequest(result.ErrorMessage);
        
        }


        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpDelete("DeleteCustomer/{Id}")]
        public async Task<IActionResult> DeleteCustomer(int Id)
        {

            var result = await _customer.DeleteCustomer(Id);

            return result.IsSuccess ? Ok("Deleted is successfully.") : BadRequest(result.ErrorMessage);

        }

          
    }
}
