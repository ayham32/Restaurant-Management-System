using FluentValidation;
using RestaurantShared.DTOs.UsersDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantBusiness.Validation.UsersValidation
{
    public class ChangePasswordValidation: AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordValidation()
        {
            ApplyOrderItemValidation();
        }

        private void ApplyOrderItemValidation()
        {
         
            RuleFor(p => p.newPassword)
                  .NotEmpty()
                .Length(8, 20);

            RuleFor(u=>u.oldPassword)
                .NotEmpty()
                .Length(8, 20);

        }
    }
}
