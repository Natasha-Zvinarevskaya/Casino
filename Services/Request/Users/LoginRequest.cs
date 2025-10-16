using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.Request.Users
{
    public class LoginRequest
    {
        
        public string Email { get; set; }
        //Password
        public string Password { get; set; }
    }
}
