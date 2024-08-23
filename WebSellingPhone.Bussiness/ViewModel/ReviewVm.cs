using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSellingPhone.Bussiness.ViewModel
{
    public class ReviewVm
    {
        public string Comment { get; set; }
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
    }
}
