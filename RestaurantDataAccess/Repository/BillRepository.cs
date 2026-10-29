using Microsoft.EntityFrameworkCore;
using RestaurantDataAccess.Data;
using RestaurantDataAccess.Entities;
using RestaurantDataAccess.InterfaceRepository;


namespace RestaurantDataAccess.Repository
{
    public class BillRepository:GenericRepository<Bill>,  IBillRepository
    {

        private readonly RestaurantDbContext _context;

        public BillRepository(RestaurantDbContext context):base (context)
        {
            _context = context;
        }

        public async Task<Bill> GetBillAsync(int BillId)
            => await _context.Bills
                             .Include(b => b.Order)
                             .Include(e => e.CreatedByNavigation)
                             .ThenInclude(u => u.User)
                             .SingleOrDefaultAsync(b=>b.BillId==BillId);
        
        public async Task<List<Bill>> GetBillsAsync()
            => await _context.Bills
                             .Include(b=>b.Order)
                             .Include(e=>e.CreatedByNavigation)
                             .ThenInclude(u=>u.User)
                             .AsNoTracking()
                             .ToListAsync();

        public async Task<bool> IsExistByOrderIdAsync(int OrderId) =>
            await _context.Bills.AnyAsync(b => b.OrderId == OrderId);



    }
}

