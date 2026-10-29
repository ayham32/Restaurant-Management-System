using RestaurantBusiness.InterfaceServices;
using RestaurantShared.DTOs.DashboardDTOs;
using RestuarantDataAccess.UintOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantBusiness.Services
{
    public class DashboardService: IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<List<RecentOrderDto>> RecentOrders()
        {
            return _unitOfWork.dashboardRepository.RecentOrders();
        }

        public Task<DashboardStatisticsDto> Statistics()
        {
            return _unitOfWork.dashboardRepository.Statistics();
        }
    }
}
