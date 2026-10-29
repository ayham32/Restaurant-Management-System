using RestaurantShared.DTOs.UsersDTOs;

namespace RestaurantBusiness.Validation.UsersValidation
{
    public class CreateUserValidation : UserValidation<CreateUserDto>
    {
        public CreateUserValidation()
        {
            ApplyUserValidation(
                u => u.PersonId,
                u => u.Username,
                u => u.Pass
                );

        }
    }
}
