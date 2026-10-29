using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantBusiness.InterfaceServices;

namespace RestaurantApi.Controllers
{
    [Authorize]
    [Route("api/Dashboard")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("Statistics")]
        public async Task<IActionResult> Statistics()
        {
            var result = await _dashboardService.Statistics();

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("RecentOrders")]
        public async Task<IActionResult> RecentOrders()
        {
            var result = await _dashboardService.RecentOrders();

            return Ok(result);
        }
    }
}

