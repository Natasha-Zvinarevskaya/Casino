using Casino.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.Models.PlayerGameService.Request
{
    public class StartGameRequest
    {
        public int UserId { get; set; }
        public int Game { get; set; }
    }
}
