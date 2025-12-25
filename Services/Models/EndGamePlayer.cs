using Casino.DataContext.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.Models
{
    public class EndGamePlayer
    {
        public int UserId { get; set; }
        public EnumStatusPlayerGame StatusGame { get; set; }
    }
}
