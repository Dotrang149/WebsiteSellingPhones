﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSellingPhone.Bussiness.ViewModel
{
    public class RegisterViewModel
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public string FirstName { get; set; }
        public bool IsActive { get; set; }
    }
}
