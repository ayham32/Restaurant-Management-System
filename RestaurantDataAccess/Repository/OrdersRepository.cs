

using Microsoft.EntityFrameworkCore;
using RestaurantDataAccess.Data;
using RestaurantDataAccess.Entities;
using RestaurantDataAccess.InterfaceRepository;

namespace RestaurantDataAccess.Repository
{
    public class OrdersRepository:GenericRepository<Order> ,IOrdersRepository
    {
        private readonly RestaurantDbContext _context;

        public OrdersRepository(RestaurantDbContext context):base(context)
        {
            _context = context;
        }

        
        
        public async Task<Order> GetOrderAsync(int OrderId)
            => await _context.Orders
                             .Include(o=>o.Customer)
                             .Include(o=>o.CreatedByNavigation).ThenInclude(u=>u.User)
                             .Include(o=>o.OrderItems).ThenInclude(oi=>oi.Product)
                             .SingleOrDefaultAsync(x=> x.OrderId == OrderId);  

        public async Task<List<Order>> GetOrdersAsync()
            => await _context.Orders
                             .Include(o => o.Customer)
                             .Include(o => o.CreatedByNavigation).ThenInclude(u => u.User)
                             .Include(o => o.OrderItems)
                             .ThenInclude(oi => oi.Product)
                             .AsNoTracking()
                             .ToListAsync();

        

    }
}
