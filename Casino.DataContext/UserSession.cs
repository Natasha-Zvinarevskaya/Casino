using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.DataContext
{
    public class UserSession
    {
        public int Id { get; set; }
        public Guid Token { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime DateClose{ get; set; }
        public int UserId { get; set; }
        public Users User { get; set; }
    }
}
