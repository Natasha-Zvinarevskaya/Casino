using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.Request.GoogleAuth
{
    public class GoogleLoginRequest
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
