using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.RequestResponse.GoogleAuth.Request
{
    public class GetAuthUrlRequest
    {
        public int ProviderType { get; set; } //EnumProviderType
        public string RedirectUrl { get; set; } //Ссылка на которую будет направлен пользователь после авторизации
        public int Action { get; set; } //EnumActionType (Перечисление действий пользователя.Авторизация/Регистрация/Привязка аккаунта
        public int AuthToken { get; set; } //Токен авторизации 
    }
}
