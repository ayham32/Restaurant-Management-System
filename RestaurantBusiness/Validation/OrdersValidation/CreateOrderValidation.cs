using RestaurantShared.DTOs.OrdersDTOs;


namespace RestaurantBusiness.Validation.OrdersValidation
{
    public class CreateOrderValidation : OrderValidation<CreateOrderDto>
    {
        public CreateOrderValidation()
        {
            ApplyOrderValidation
                (
                    o => o.TableNumber,
                   
                    o => o.OrderType
                );

        }
    }
}
