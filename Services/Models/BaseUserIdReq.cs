using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.Models
{
   public  class BaseUserIdReq <T>
    {
        public BaseUserIdReq(int userId, T request)
        {
            UserId = userId;
            Request = request;
        }

        public int UserId { get; set; }
        public T Request { get; set; }
    }
}
