using RestaurantShared.DTOs.DashboardEmployeeDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantDataAccess.InterfaceRepository
{
    public interface IDashboardEmployeeRepository
    {
        public Task<StatisticsDto> Statistics();

    }
}
