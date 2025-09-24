using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Casino.DataContext
{
    /// <summary>
    /// Сохраненные данные карт в игре , для передачи этих данных фронту
    /// </summary>
    public class GameHistory
    {
        public int Id { get; set; }
        /// <summary>
        /// json формат карт дилера и карт игрока
        /// </summary>
        public string CardsHistory { get; set; }

        public int PlayerGameId { get; set; }
        public PlayerGame PlayerGame { get; set; }
    }
}
