
using RestaurantDataAccess.Entities;

namespace RestaurantDataAccess.InterfaceRepository
{
    public interface IOrdersRepository:IGenericRepository<Order>
    {
        public Task<List<Order>> GetOrdersAsync();

        public Task<Order> GetOrderAsync(int OrderId);

   
      
    }
}
