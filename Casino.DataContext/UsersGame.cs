using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.DataContext
{
    public class UsersGame
    {
        /// <summary>
        /// Ид пользователя
        /// </summary>
        public int UserId { get; set; }
        public Users User { get; set; }
       
        /// <summary>
        /// Ид игры
        /// </summary>
        public int GameId { get; set; }
        public Game Game { get; set; }
    }
}
