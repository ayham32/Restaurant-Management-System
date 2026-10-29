using FluentValidation;
using RestaurantShared.DTOs.OrderItemsDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantBusiness.Validation.OrderItemsValidation
{
    public class CreateOrderItemValidation:AbstractValidator<CreateOrderItemDto>
    {
        public CreateOrderItemValidation()
        {
            ApplyOrderItemValidation();
        }

        private void ApplyOrderItemValidation()
        {
           

            RuleFor(oi => oi.ProductId)
                .NotEmpty()
                .GreaterThan(0);

         
            RuleFor(oi => oi.Quantity)
                .NotEmpty()
                .GreaterThan(0);


        }

    }
}
