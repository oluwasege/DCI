using System;
using System.Collections.Generic;
using System.Text;

namespace DCI.Entities.ViewModels.LoginVMs
{
    public class LoginResponseVM
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public IList<string> Roles { get; set; }
    }
}
