using Casino.DataContext.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.DataContext
{
   public class UserProvider
    {
        public int Id { get; set; }
        public EnumProviderType ProviderType { get; set; }
        public string Token { get; set; }

        public Users User { get; set; }
        public int UserId { get; set; }
    }
}
