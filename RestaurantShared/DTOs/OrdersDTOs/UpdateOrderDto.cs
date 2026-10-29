

using RestaurantShared.Enums;

namespace RestaurantShared.DTOs.OrdersDTOs
{
    public class UpdateOrderDto
    {

        public int? CustomerId { get; set; }

        public enOrderType OrderType { get; set; }

        public int? TableNumber { get; set; }

    }
}
