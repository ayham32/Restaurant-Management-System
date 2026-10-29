using RestaurantShared.DTOs.DashboardDTOs;
using RestaurantShared.DTOs.DashboardEmployeeDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantBusiness.InterfaceServices
{
    public interface IDashboardEmployeeService
    {
        Task<StatisticsDto> Statistics();

        Task<List<RecentOrderDto>> RecentOrders();
    }
}
