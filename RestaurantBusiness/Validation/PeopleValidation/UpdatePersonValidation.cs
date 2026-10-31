
using FluentValidation;
using RestaurantShared.DTOs.PeopleDTOs;

namespace RestaurantBusiness.Validation.PeopleValidation
{
    public class UpdatePersonValidation : AbstractValidator<UpdatePersonDto>
    {
        public UpdatePersonValidation()
        {
            ApplyValidation();
        }

        private void ApplyValidation()
        {
            RuleFor(p => p.NationalNo)
                .NotEmpty();

            RuleFor(p => p.FirstName)
                .NotEmpty()
                .NotNull();

            RuleFor(p => p.LastName)
                .NotEmpty()
                .NotNull();

            RuleFor(p => p.Phone)
            .NotEmpty()
            .NotNull();

            RuleFor(p => p.DateOfBirth.Year)
            .NotEmpty()
            .NotNull()
            .GreaterThanOrEqualTo(16);

            RuleFor(p => p.Gender)
            
            .IsInEnum();



        }

    }
}
