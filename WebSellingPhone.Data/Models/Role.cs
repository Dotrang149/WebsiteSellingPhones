using Microsoft.AspNetCore.Identity;

namespace WebSellingPhone.Data.Models
{
    public class Role : IdentityRole<Guid>
    {
        public string Description { get; set; }
    }
}
