using Microsoft.AspNetCore.Identity;

namespace Domain.Models
{
    public class AdminUser: IdentityUser
    {
        public Boolean IsAdmin { get; set; }
    }
}
