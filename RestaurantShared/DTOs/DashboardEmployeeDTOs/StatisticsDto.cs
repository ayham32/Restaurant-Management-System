using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantShared.DTOs.DashboardEmployeeDTOs
{
    public class StatisticsDto
    {
        public int cancelledOrderCount { get; set; }
        public int completedOrderCount { get; set; }
        public int cookingOrderCount { get; set; }
        public int pendingOrderCount { get; set; }
        public int isntAvilableProductsCount { get; set; }
        public List<string>? productNameIsntAvilable {  get; set; }
        public decimal todaysSalesPrices { get; set; }
    }
}
