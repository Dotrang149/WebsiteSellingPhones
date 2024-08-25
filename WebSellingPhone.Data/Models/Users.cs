using Microsoft.AspNetCore.Identity;

namespace WebSellingPhone.Data.Models
{
    public class Users : IdentityUser<Guid>
    {
       
        //1-many
        public ICollection<Order> Orders { get; set; }
        public ICollection<Review> Reviews { get; set; }

        public ICollection<RefreshToken> RefreshTokens { get; set; }

    }
}
