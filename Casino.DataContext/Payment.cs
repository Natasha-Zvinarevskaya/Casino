using Casino.DataContext.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.DataContext
{
    public class Payment
    {
        public int Id { get; set; }
        public decimal Amount { get; set; } 
        public int UserId { get; set; }
        public EnumStatus Status { get; set; }
    
        public DateTime Date {  get; set; }

        public Users User { get; set; } 
             
    }
}
