using Casino.DataContext;
using Casino.Services.Interfaces;
using Casino.Services.Models;
using Casino.Services.Models.BlackjackGame.Response;
using Casino.Services.Request.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace Casino.Services.Service
{
    public class UserService : IUserService
    {
        private DbContextOptions<CasinoDbContext> _options;
        public UserService(DbContextOptions<CasinoDbContext> options)
        {
            _options = options;
        }
        /// <summary>
        /// Ркгистрация пользователя
        /// </summary>
        /// <param name="request">Емаил и пароль пользователя</param>
        /// <returns>ид пользователя</returns>
      
        public BaseResponse<int> Registration(RegisterRequest request)
        {
            using var db = new CasinoDbContext(_options);
            var passwordHash = CreateSHA256(request.Password);
            db.Users.Add(new Users
            {
                Password = passwordHash,
                Email = request.Email

            });
            db.SaveChanges();
            var user = db.Users.FirstOrDefault(x => x.Email == request.Email && x.Password == passwordHash);
            if (user == null)
                return new BaseResponse<int>("Пользователь не найден.");
            return new BaseResponse<int>(user.Id);

        }
        /// <summary>
        /// Создания хэшированного пароля
        /// </summary>
        /// <param name="input">Строка вводимая пользователем</param>
        /// <returns>Хэшированная строка</returns>
        public static string CreateSHA256(string input)
        {
            using SHA256 hash = SHA256.Create();
            return Convert.ToHexString(hash.ComputeHash(Encoding.UTF8.GetBytes(input)));
        }
        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        /// <param name="request">Емаил, пароль</param>
        /// <returns>Модель сессии</returns>
        public BaseResponse<UserSessionModel> Login (LoginRequest request)
        {
            var passwordHash = CreateSHA256(request.Password);
            using var db = new CasinoDbContext(_options);
            var user = db.Users.FirstOrDefault(x => x.Email == request.Email && x.Password == passwordHash);
            if (user == null)
                return new BaseResponse<UserSessionModel>("Пользователь не найден");
            var dataClose = DateTime.UtcNow;
            dataClose.AddHours(3);
            var token = Guid.NewGuid();
            var session = new UserSession
            {
                UserId=user.Id,
                Token=token,
                DateCreate = DateTime.UtcNow,
                DateClose=dataClose
            };
            db.UserSessions.Add(session);
            db.SaveChanges();
            var response = new UserSessionModel
            {
                Id = session.Id,
                Token=session.Token,
                DateClose=session.DateClose,
                DateCreate=session.DateCreate
            };
            return new BaseResponse<UserSessionModel>(response);


        }
    }
}
