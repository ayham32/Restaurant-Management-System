using Microsoft.EntityFrameworkCore;
using RestaurantDataAccess.Data;
using RestaurantDataAccess.InterfaceRepository;
using RestaurantShared.DTOs.DashboardDTOs;

public class DashboardRepository : IDashboardRepository
{
    private readonly RestaurantDbContext _context;

    public RestaurantDbContext Context => _context;

    public DashboardRepository(RestaurantDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardStatisticsDto> Statistics()
    {
        DateTime today = DateTime.Today;
        DateTime tomorrow = today.AddDays(1);

        int productsCount = await _context.Products.CountAsync();

        int todayOrdersCount = await _context.Orders
            .CountAsync(o =>
                o.OrderDate >= today &&
                o.OrderDate < tomorrow);

        int activeEmployeesCount = await _context.Employees.Include(u=>u.User)
            .CountAsync(e => e.User.IsActive);

        decimal todayRevenue = await _context.Bills
            .Where(b =>
                b.IssuedAt >= today &&
                b.IssuedAt < tomorrow)
            .SumAsync(b => (decimal?)b.FinalAmount) ?? 0;

        return new DashboardStatisticsDto
        {
            ProductsCount = productsCount,
            TodayOrdersCount = todayOrdersCount,
            ActiveEmployeesCount = activeEmployeesCount,
            TodayRevenue = todayRevenue
        };
    }

    public async Task<List<RecentOrderDto>> RecentOrders()
    {
        return await _context.Orders
            .OrderByDescending(o => o.OrderDate)
            .Take(5)
            .Select(o => new RecentOrderDto
            {
                Id = o.OrderId,
                customerName = o.Customer.Name,
                totalPrice = o.TotalAmount,
                status = o.Status.ToString()
            })
            .ToListAsync();
    }
}