
using RestaurantDataAccess.InterfaceRepository;

namespace RestuarantDataAccess.UintOfWork
{
    public interface IUnitOfWork
    {
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
        public Task<int> SaveChangesAsync();

        public Task BeginTransactionAsync();

        public Task CommitTransactionAsync();

        public Task RollbackTransactionAsync();
    }
}
