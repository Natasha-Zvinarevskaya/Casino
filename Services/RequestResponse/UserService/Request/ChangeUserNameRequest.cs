using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.RequestResponse.UserService.Request
{
    public class ChangeUserNameRequest
    {
        public int UserId { get; set; }
        public string Name { get; set; }
    }
}
