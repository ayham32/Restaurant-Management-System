using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantBusiness.InterfaceServices;
using RestaurantShared.DTOs.OrderItemsDTOs;
using RestaurantShared.DTOs.OrdersDTOs;
using RestaurantShared.Enums;

namespace RestaurantApi.Controllers
{
    [Authorize]
    [Route("api/Orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
         
        private readonly IOrdersService _order;

        public OrdersController(IOrdersService order)
        {
            _order = order;
        }

        [Authorize(Roles = "Admin,Employee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("AllOrders")]
        public async Task<ActionResult<IEnumerable<ReadOrdersDto>>> GetAllOrders()
        {
            var orders = await _order.GetOrdersAsync();

            return !orders.IsSuccess  ? NotFound(orders.ErrorMessage) : Ok(orders.Data);
        }

        [Authorize(Roles = "Admin,Employee")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("Order/{Id}")]
        public async Task<ActionResult<ReadOrdersDto>> GetOrder(int Id)
        {
            

           var order = await _order.GetOrderAsync(Id);

           

            return !order.IsSuccess ? NotFound(order.ErrorMessage) : Ok(order.Data);


        }

        [Authorize(Roles = "Admin,Employee")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("isOrderExists/{Id}")]
        public async Task<IActionResult> isOrderExist(int Id)
        {
            if (Id <= 0)
                return BadRequest("You cannot enter a id less than one.");

            return await _order.isOrderExistAsync(Id)?Ok("Yes is exist") : NotFound($"No order with Id: {Id}");
        }

        [Authorize(Roles = "Admin,Employee")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPost("AddNewOrder")]
        public async Task<IActionResult> AddNewOrder(CreateOrderDto newOrder)
        {
            var result = await _order.AddNewOrderAsync(newOrder);

            return result.IsSuccess? Created("" ,result.Data) : BadRequest(result.ErrorMessage);
        }

        [Authorize(Roles = "Admin,Employee")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut("UpdateOrder/{Id}")]
        public async Task<IActionResult> UpdateOrder(int Id,UpdateOrderDto order)
        {

            var result = await _order.UpdateOrderAsync(Id, order);

            return result.IsSuccess ? Ok("Updated successfully.") : BadRequest(result.ErrorMessage);
        }


        [Authorize(Roles = "Admin,Employee")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpDelete("DeleteOrder/{Id}")]
        public async Task<IActionResult> DeleteOrder(int Id)
        {
            var result = await _order.DeleteOrder(Id);

            return result.IsSuccess ? Ok("Deleted successfully.") : BadRequest(result.ErrorMessage);
        }

        [Authorize(Roles = "Admin,Employee")]
       [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut("CancelledOrder/{Id}")]
        public async Task<IActionResult> CancelledOrder(int Id)
        {
           var res = await _order.changeOrderStatus(Id,enOrderStatus.Cancelled);

            return res.IsSuccess ? Ok() : BadRequest(res.ErrorMessage);
        }

        [Authorize(Roles = "Admin,Employee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut("CompletedOrder/{Id}")]
        public async Task<IActionResult> CompletedOrder(int Id)
        {
            var res = await _order.changeOrderStatus(Id, enOrderStatus.Completed);
            return res.IsSuccess ? Ok() : BadRequest(res.ErrorMessage);

        }

        [Authorize(Roles = "Admin,Employee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut("CookingOrder/{Id}")]
        public async Task<IActionResult> CookingOrder(int Id)
        {
            var res = await _order.changeOrderStatus(Id, enOrderStatus.Cooking);
            return res.IsSuccess ? Ok() : BadRequest(res.ErrorMessage);

        }

        [Authorize(Roles = "Admin,Employee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut("PendingOrder/{Id}")]
        public async Task<IActionResult> PendingOrder(int Id)
        {
            var res = await _order.changeOrderStatus(Id, enOrderStatus.Pending);
            return res.IsSuccess ? Ok() : BadRequest(res.ErrorMessage);

        }

        [Authorize(Roles = "Admin,Employee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("{OrderId}/AddNewItem")]
        public async Task<IActionResult> AddNewItem(int OrderId, CreateOrderItemDto dto)
        {   
            var res = await _order.AddNewOrderItem(OrderId, dto);

            return res.IsSuccess ? Created("", res.Data) : BadRequest(res.ErrorMessage);
        }

        [Authorize(Roles = "Admin,Employee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("{OrderId}/UpdateItem/{itemId}")]
        public async Task<IActionResult> UpdateItem(int OrderId,int itemId, UpdateOrderItemDto dto)
        {
            var res = await _order.UpdateOrderItemQuantity(OrderId,itemId, dto);

            return res.IsSuccess ? Ok("Updated successfully.") : BadRequest(res.ErrorMessage);
        }
       
        [Authorize(Roles = "Admin,Employee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{OrderId}/DeleteItem/{itemId}")]
        public async Task<IActionResult> DeleteItem(int OrderId, int itemId)
        {
            var res = await _order.DeleteOrderItem(OrderId, itemId);

            return res.IsSuccess ? Ok("Deleted successfully.") : BadRequest(res.ErrorMessage);
        }

    }
}
