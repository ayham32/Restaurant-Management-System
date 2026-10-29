
using RestaurantShared.Enums;

namespace RestaurantDataAccess.Entities;

public partial class Order
{
    public int OrderId { get; set; }

    public int? CustomerId { get; set; }

    public enOrderType OrderType { get; set; }

    public enOrderStatus Status { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? OrderDate { get; set; }

    public decimal? TotalAmount { get; set; }

    public int? TableNumber { get; set; }

    public virtual Bill? Bill { get; set; }

    public virtual Employee? CreatedByNavigation { get; set; }

    public virtual Customer? Customer { get; set; }

   
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
