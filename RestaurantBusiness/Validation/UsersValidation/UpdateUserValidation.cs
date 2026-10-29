using RestaurantShared.DTOs.UsersDTOs;

namespace RestaurantBusiness.Validation.UsersValidation
{
    public class UpdateUserValidation : UserValidation<UpdateUserDto>
    {
        public UpdateUserValidation()
        {
            ApplyUserValidation(
                u => u.PersonId,
                u => u.Username
                );

        }
    }
}
