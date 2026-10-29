using Microsoft.EntityFrameworkCore;
using RestaurantDataAccess.Data;
using RestaurantDataAccess.Entities;
using RestaurantDataAccess.InterfaceRepository;
using RestaurantShared.DTOs.DashboardEmployeeDTOs;
using RestaurantShared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantDataAccess.Repository
{
    public class DashboardEmployeeRepository : IDashboardEmployeeRepository
    {
        private readonly RestaurantDbContext _context;

        public DashboardEmployeeRepository(RestaurantDbContext context)
        {
            _context = context;
        }

        
        public async Task<StatisticsDto> Statistics()
        {
            int cancelledOrder =await _context.Orders.CountAsync(o => o.Status == enOrderStatus.Cancelled);
            int completedOrder = await _context.Orders.CountAsync(o => o.Status == enOrderStatus.Completed);
            int cookingOrder = await _context.Orders.CountAsync(o => o.Status == enOrderStatus.Cooking);
            int pendingOrder = await _context.Orders.CountAsync(o => o.Status == enOrderStatus.Pending);
            List<string> productsNameIsntAvilable = await _context.Products.Where(p=>!p.IsAvailable).Select(p => p.Name).ToListAsync();
            int isntAvilableProductsCount = productsNameIsntAvilable.Count;
            DateTime today = DateTime.Today;
            DateTime tomorrow = today.AddDays(1);
            decimal todaysSalesPrices = await _context.Bills
            .Where(b =>
                b.IssuedAt >= today &&
                b.IssuedAt < tomorrow)
            .SumAsync(b => (decimal?)b.FinalAmount) ?? 0;

            return new StatisticsDto
            {
                cancelledOrderCount = cancelledOrder,
                completedOrderCount = completedOrder,
                cookingOrderCount = cookingOrder,
                pendingOrderCount = pendingOrder,
                isntAvilableProductsCount = isntAvilableProductsCount,
                productNameIsntAvilable = productsNameIsntAvilable,
                todaysSalesPrices = todaysSalesPrices,
            };
        }
    }
}
