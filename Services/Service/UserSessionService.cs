using Casino.DataContext;
using Casino.Services.Interfaces;
using Casino.Services.Models.BlackjackGame.Response;
using Casino.Services.Models.UserSessionServiceModel.Response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.Service
{
   public  class UserSessionService : IUserSessionService
    {
        private DbContextOptions<CasinoDbContext> _options;
        public UserSessionService(DbContextOptions<CasinoDbContext> options)
        {
            _options = options;
        }
        //через сессии посмотреть есть ли такой юзер  и метод вернет модель юзера (ид,маил и тд)
        /// <summary>
        /// Проверяет есть ли у пользователь доступ и отправляет данные о пользователе
        /// </summary>
        /// <param name="response">токен</param>
        /// <returns>инфо о пользователе</returns>
        /// <exception cref="Exception"></exception>
        public BaseResponse<CheckUserResponse> CheckUser(string tokenString)
        {
            var db = new CasinoDbContext(_options);
            var token = Guid.Parse(tokenString);
            var userSession = db.UserSessions.FirstOrDefault(x => x.Token == token);
            if (userSession == null)
                return new BaseResponse<CheckUserResponse>("Сессия не найдена. Неверный токен.");
            var user = db.Users.FirstOrDefault(x => x.Id == userSession.UserId);
            if (user == null)
                return new BaseResponse<CheckUserResponse>("Пользователь не найден.");
            var userModel = new CheckUserResponse
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name

            };

            return new BaseResponse<CheckUserResponse>(userModel);

        }
        public void UserExit (int userSessionId)
        {
            var db = new CasinoDbContext(_options);
            var userSession = db.UserSessions.FirstOrDefault(x => x.Id == userSessionId);
            if (userSession == null)
                throw new Exception("Сессия не найдена.");
            db.UserSessions.Remove(userSession);
            db.SaveChanges();
        }
    }

}
