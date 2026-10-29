using RestaurantDataAccess.Entities;
using RestaurantShared.DTOs.DashboardDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantDataAccess.InterfaceRepository
{
    public interface IDashboardRepository
    {
        public Task<DashboardStatisticsDto> Statistics();
        public Task<List<RecentOrderDto>> RecentOrders();
    }
}
