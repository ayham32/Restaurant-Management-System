

using RestaurantShared.Enums;

namespace RestaurantShared.DTOs.BillsDTOs
{
    public class ReadBillsDto
    {
        public int BillId { get; set; }

        public int OrderId { get; set; }

        public decimal? SubTotal { get; set; }

        public decimal? TaxAmount { get; set; }

        public decimal? Discount { get; set; }

        public decimal? FinalAmount { get; set; }

        public enPaymentMethod PaymentMethod { get; set; }

        public enPaymentStatus PaymentStatus { get; set; }

        public DateTime? IssuedAt { get; set; }

        public string CreatedBy { get; set; } = null!;

    }
}
