using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.Enums.BlackjackGame
{
    /// <summary>
    /// Значение карты о 2 до тузов
    /// </summary>
    public enum CardValue
    {
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        /// <summary>
        /// Валет
        /// </summary>
        Jack = 10,
        /// <summary>
        /// Дама
        /// </summary>
        Queen = 10,
        /// <summary>
        /// Король
        /// </summary>
        King = 10,
        /// <summary>
        /// Туз
        /// </summary>
        Ace = 11

    }
}
