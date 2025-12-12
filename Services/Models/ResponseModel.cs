using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.Models
{
    public class ResponseModel
    {
        public int UserId { get; set; }
        public string? Controller { get; set; }
        public string? Method { get; set; }
        public string Value { get; set; }
    }
}
