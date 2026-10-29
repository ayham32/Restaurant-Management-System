using RestaurantBusiness.InterfaceServices;
using RestaurantShared.DTOs.DashboardDTOs;
using RestaurantShared.DTOs.DashboardEmployeeDTOs;
using RestuarantDataAccess.UintOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantBusiness.Services
{
    public class DashboardEmployeeService: IDashboardEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardEmployeeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<List<RecentOrderDto>> RecentOrders()
        {
            return _unitOfWork.dashboardRepository.RecentOrders();
        }

        public Task<StatisticsDto> Statistics()
        {
            return _unitOfWork.dashboardEmployeeRepository.Statistics();
        }
    }
}
