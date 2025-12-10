using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.RequestResponse.GoogleAuth.Request
{
    public class CheckUserExistsRequest
    {
        public string Email { get; set; }
        public string RefreshToken { get; set; }
        public int RefreshTokenTime { get; set; }
        public string Name { get; set; }
       // public string State { get; set; }
    }
}
