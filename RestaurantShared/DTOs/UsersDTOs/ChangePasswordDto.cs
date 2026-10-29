using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantShared.DTOs.UsersDTOs
{
    public class ChangePasswordDto
    {
        public string oldPassword{get;set;}=null!;
        public string newPassword { get; set; } = null!;
    }
}
