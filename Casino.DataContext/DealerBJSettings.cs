using Casino.DataContext.Enums.BlackjackGame;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.DataContext
{
    public class DealerBJSettings
    {
       // [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public EnumBlackJackGameSettings Id { get; set; }

        public string Settings { get; set; }
    }
}
