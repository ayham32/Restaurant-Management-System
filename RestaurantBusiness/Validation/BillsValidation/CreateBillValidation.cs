using FluentValidation;
using RestaurantShared.DTOs.BillsDTOs;


namespace RestaurantBusiness.Validation.BillsValidation
{
    public class CreateBillValidation:AbstractValidator<CreateBillDto>
    {

        public CreateBillValidation() 
        {
            ApplyBillValidation();
        }

        private void ApplyBillValidation()
        {
            RuleFor(b => b.OrderId)
                .NotNull()
                .GreaterThan(0);

            
            RuleFor(b => b.TaxAmount)
                .NotNull()
                .GreaterThanOrEqualTo(0);

            RuleFor(b => b.Discount)
              .NotNull()
              .GreaterThanOrEqualTo(0);

         
            RuleFor(b => b.PaymentMethod)
              .NotNull()
              .IsInEnum();


            RuleFor(b => b.PaymentStatus)
              .NotNull()
              .IsInEnum();
        }
    }
}
