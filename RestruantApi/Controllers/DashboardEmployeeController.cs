using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantBusiness.InterfaceServices;

namespace RestaurantApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardEmployeeController : ControllerBase
    {
        private readonly IDashboardEmployeeService _dashboardService;

        public DashboardEmployeeController(IDashboardEmployeeService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [Authorize(Roles = "Employee")]
        [HttpGet("Statistics")]
        public async Task<IActionResult> Statistics()
        {
            var result = await _dashboardService.Statistics();

            return Ok(result);
        }

        [Authorize(Roles = "Employee")]
        [HttpGet("RecentOrders")]
        public async Task<IActionResult> RecentOrders()
        {
            var result = await _dashboardService.RecentOrders();

            return Ok(result);
        }
    }
}
