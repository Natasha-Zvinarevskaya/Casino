using Casino.DataContext.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.RequestResponse.UserSessionService.Response
{
   public class CheckUserResponse
    {
        /// <summary>
        /// Пользователя
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Почта
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Роль поьзователя
        /// </summary>
        public EnumRoles Role {  get; set; }
    }
}
