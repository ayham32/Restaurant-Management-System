
using FluentValidation;
using System.Linq.Expressions;

namespace RestaurantBusiness.Validation.UsersValidation
{
    public abstract class UserValidation<T> : AbstractValidator<T>
    {
        protected void ApplyUserValidation(
        Expression<Func<T, int>> PersonId,
        Expression<Func<T, string>> Username,
        Expression<Func<T, string>> Pass)
        {
            RuleFor(PersonId)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(Username)
                .NotEmpty();

            RuleFor(Pass)
                .NotEmpty()
                .Length(8,20);

        }

        protected void ApplyUserValidation(
       Expression<Func<T, int>> PersonId,
       Expression<Func<T, string>> Username)
        {
            RuleFor(PersonId)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(Username)
                .NotEmpty();

            
            
        }

    }


}
