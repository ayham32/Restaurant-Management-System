using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantBusiness.InterfaceServices;
using RestaurantShared.DTOs.ProductsDTOs;
using RestaurantShared.Pagination;
using RestaurantShared.Parameters;

namespace RestaurantApi.Controllers
{
    [Authorize]
    [Route("api/Products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly IProductsService _product;

        public ProductsController(IProductsService product)
        {
            _product = product;
        }


        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("AllProducts")]
        public async Task<ActionResult<PagedResult<ReadProductsDto>>> GetProducts([FromQuery] ProductParameter parameter)
        {
            var Products = await _product.GetProductsAsync(parameter);

            return !Products.IsSuccess ? NotFound(Products.ErrorMessage) : Ok(Products);
        }

        [AllowAnonymous]
        
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("Product/{Id}")]
        public async Task<ActionResult<ReadProductsDto>> GetProduct(int Id)
        {
           
            var product = await _product.GetProductAsync(Id);


            return !product.IsSuccess ? NotFound(product.ErrorMessage) : Ok(product.Data);
        }

        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("isProductExist/{Id}")]
        public async Task<IActionResult> isProductExist(int Id)
        {
            if (Id <= 0)
                return BadRequest("You cannot enter a id less than one.");

            return await _product.isProductExist(Id) ? Ok("Yse is exist") : NotFound($"No product with Id: {Id}");
        }


        [Authorize(Roles = "Admin,Employee")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPost("AddNewProduct")]
        public async Task<IActionResult> AddNewProduct([FromForm] CreateProductDto newProduct)
        {
            var result = await _product.AddNewProduct(newProduct);

            return result.IsSuccess ? Created("",result.Data) : BadRequest(result.ErrorMessage);
        }


        [Authorize(Roles = "Admin,Employee")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut("UpdateProduct/{Id}")]
        public async Task<IActionResult> UpdateProduct(int Id , [FromForm] UpdateProductDto product)
        {
            var result = await _product.UpdateProduct(Id, product);

            return result.IsSuccess ? Ok("Updated successfully.") : BadRequest(result.ErrorMessage);
        }


        [Authorize(Roles = "Admin,Employee")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpDelete("DeleteProduct/{Id}")]
        public async Task<IActionResult> DeleteProduct(int Id)
        {

            var result = await _product.DeleteProduct(Id);

            return result.IsSuccess? Ok("Deleted successfully.") : BadRequest(result.ErrorMessage);
        }

    }
}
