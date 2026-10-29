using RestaurantDataAccess.Data;
using RestaurantDataAccess.InterfaceRepository;
using RestaurantDataAccess.Repository;
using RestuarantDataAccess.UintOfWork;


namespace RestaurantDataAccess.UintOfWork
{
    public class UnitOfWorkService : IUnitOfWork
    {
        private readonly RestaurantDbContext _context;
        
        public IPeopleRepository peopleRepository { get; }
        public IUsersRepository usersRepository { get; }
        public ICustomerRepository customerRepository { get; }
        public IEmployeeRepository empolyeesRepository { get; }
       public ICategoriesRepository categoriesRepository { get; }
        public IOrdersRepository ordersRepository { get; }
        public IOrderItemsRepository orderItemsRepository { get; }
        public IProductRepository productRepository { get; }
        public IBillRepository billRepository { get; }

        public IRefreshTokenRepository refreshTokenRepository { get; }
        public IDashboardRepository dashboardRepository { get; }
        public IDashboardEmployeeRepository dashboardEmployeeRepository { get; }
        public UnitOfWorkService(RestaurantDbContext context)
        {
            _context = context;
            peopleRepository = new PeopleRepository(context);
            usersRepository = new UsersRepository(context);
            customerRepository = new CustomerRepository(context);
            empolyeesRepository = new EmployeeRepository(context);
            categoriesRepository=new CategoriesRepository(context);
            orderItemsRepository=new OrderItemsRepository(context);
            ordersRepository=new OrdersRepository(context);
            productRepository =new ProductRepository(context);
            billRepository  = new BillRepository(context);
            refreshTokenRepository = new RefreshTokenRepository(context);
            dashboardRepository = new DashboardRepository(context);
            dashboardEmployeeRepository = new DashboardEmployeeRepository(context);
        }

        
        public async Task<int> SaveChangesAsync()
           => await _context.SaveChangesAsync();

        public async Task BeginTransactionAsync()
            => await _context.Database.BeginTransactionAsync();
    
        public async Task CommitTransactionAsync()
            => await _context.Database.CommitTransactionAsync();

        public async Task RollbackTransactionAsync()
            => await _context.Database.RollbackTransactionAsync();
    }
}
