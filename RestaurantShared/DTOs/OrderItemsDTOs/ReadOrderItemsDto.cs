

namespace RestaurantShared.DTOs.OrderItemsDTOs
{
    public class ReadOrderItemsDto
    {
        public int OrderItemId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }
    }
}
