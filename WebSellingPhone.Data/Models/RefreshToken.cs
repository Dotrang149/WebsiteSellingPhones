using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSellingPhone.Data.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }
        public string Token { get; set; }
        public string JwtId { get; set; }

        public bool IsRevoked { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateExpire { get; set; }

        [ForeignKey("UserId")]
        public Users User { get; set; }
    }
}
