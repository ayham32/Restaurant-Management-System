using RestaurantDataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantDataAccess.InterfaceRepository
{
    public interface IBillRepository: IGenericRepository<Bill>
    {

        public Task<List<Bill>> GetBillsAsync();

        public Task<Bill> GetBillAsync(int BillId);
        public Task< bool> IsExistByOrderIdAsync(int OrderId);


    }
}
