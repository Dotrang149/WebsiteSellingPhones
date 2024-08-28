using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSellingPhone.Bussiness.ViewModel
{
    public class LoginResponseViewModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public required string Token { get; set; }
        public required string RefershToken { get; set; }
        public DateTime Expires { get; set; }
    }
}
