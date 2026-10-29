using FluentValidation;
using RestaurantShared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace RestaurantBusiness.Validation.OrdersValidation
{
    public abstract class OrderValidation<T>:AbstractValidator<T>
    {

        protected void ApplyOrderValidation(
        Expression<Func<T, int?>> TableNumber,
       
        Expression<Func<T, enOrderType>> OrderType)
        {
            
           

            RuleFor(OrderType)
               
                .IsInEnum();

           
        }


    }
}
