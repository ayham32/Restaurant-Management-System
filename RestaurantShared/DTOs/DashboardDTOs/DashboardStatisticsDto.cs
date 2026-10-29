using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantShared.DTOs.DashboardDTOs
{
    public class DashboardStatisticsDto
    {
        public int ProductsCount { get; set; }
        public int TodayOrdersCount { get; set; }
        public int ActiveEmployeesCount { get; set; }
        public decimal TodayRevenue { get; set; }
    }
}
