using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.Models
{
    /// <summary>
    /// Модель сессии пользователя
    /// </summary>
    public class UserSessionModel
    {
        /// <summary>
        /// Ид сессии
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Токен авторизации
        /// </summary>
        public Guid Token { get; set; }
        /// <summary>
        /// Время создания 
        /// </summary>
        public DateTime DateCreate { get; set; }
        /// <summary>
        /// Время завершения
        /// </summary>
        public DateTime DateClose { get; set; }
    }
}
