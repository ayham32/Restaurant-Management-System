using RestaurantShared.DTOs.OrdersDTOs;


namespace RestaurantBusiness.Validation.OrdersValidation
{
    public class UpdateOrdervalidation : OrderValidation<UpdateOrderDto>
    {
        public UpdateOrdervalidation() 
        {
            ApplyOrderValidation
                (
                    o=>o.TableNumber,
                    o=>o.OrderType
                   
                );
        }
    }
}
