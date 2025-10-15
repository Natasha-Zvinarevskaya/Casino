using Casino.DataContext;
using Casino.Services.Interfaces;
using Casino.Services.Models.BlackjackGame.Response;
using Casino.Services.Models;
using Casino.Services.Request.GoogleAuth;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static System.Net.WebRequestMethods;
using System.Net.Http.Json;
using System.Text.Json;
using Azure;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Casino.Services.Models.GoogleService;
using Microsoft.EntityFrameworkCore;
using Azure.Core;
using System.Security.Cryptography;


namespace Casino.Services.Service
{
    public class GoogleServices : IGoogleService 
    {
        OptionGoogleSettings _options;
        private DbContextOptions<CasinoDbContext> _optionsDb;
        public GoogleServices(IOptions<OptionGoogleSettings> options, DbContextOptions<CasinoDbContext> optionsDb)
        {
            _options = options.Value;
            _optionsDb = optionsDb;
        }

        /// <summary>
        /// Создается ссылка для авторизации через google
        /// </summary>
        /// <param name="request">ProviderType,RedirectUrl,Action,AuthToken</param>
        /// <returns></returns>
        public string GoogleProvider(GetAuthUrlRequest request)
        {
            string redirectUri = $"https://accounts.google.com/o/oauth2/v2/auth?response_type=code&client_id={_options.ClientId}&redirect_uri={request.RedirectUrl}&scope=openid%20email%20profile&access_type=offline&prompt=consent&state={request.AuthToken},{request.ProviderType},{request.Action}&include_granted_scopes=true";
            // var redirectUri=HttpUtility.UrlEncode(uri);
            return redirectUri;
        }
        /// <summary>
        /// Получить токен от гугла,через Rest запрос
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<string> GetTokenGoogle(CallbackGoogleRequest request)
        {
            HttpClient httpClient = new HttpClient();
            GetTokenPostRequest req = new GetTokenPostRequest { code = request.Code, client_id = _options.ClientId, client_secret = _options.SecretKey, grant_type = "authorization_code", redirect_uri = "http://localhost:5179/GoogleAuth/CallBack" };
            JsonContent content = JsonContent.Create(req);
            using var json = await httpClient.PostAsync("https://oauth2.googleapis.com/token", content);
            var response = await json.Content.ReadFromJsonAsync<ResponseGetToken>();




            //return redirectUri;
            return response.id_token;
        }
        /// <summary>
        /// Получить емэил пользователя через полученный токен от гугла
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse<ResponseGetEmail> GetEmail(string id_token)
        {

            string idToken = id_token;
            // Разбиваем на части
            var parts = idToken.Split('.');
            if (parts.Length < 2)
                return new BaseResponse<ResponseGetEmail>("Некорректный токен.");


            // Берем часть payload
            string payload = parts[1];


            // Декодируем Base64Url (специальная версия Base64)
            string json = DecodeBase64Url(payload);


            // При желании можно десериализовать в объект

            var data = JsonSerializer.Deserialize<ModelGoogleUserData>(json);
            var response = new ResponseGetEmail { Email = data.email, Name = data.name };

            return new BaseResponse<ResponseGetEmail>(response);
        }
        /// <summary>
        /// Декодирование b64
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string DecodeBase64Url(string input)
        {
            input = input.Replace('-', '+').Replace('_', '/');
            switch (input.Length % 4)
            {
                case 2: input += "=="; break;
                case 3: input += "="; break;
            }

            var bytes = Convert.FromBase64String(input);
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// Проверяем существует ли пользователь,когда вход осуществляется через гугл
        /// Если существет, то обновляем для него токен
        /// </summary>
        /// <param name="request">емаил, рефреш токен и время действия токена</param>
        /// <returns></returns>
        public BaseResponse<UserSessionModel> CheckUserExists(CheckUserExistsRequest request)
        {
            var db = new CasinoDbContext(_optionsDb);
            var user = db.Users.FirstOrDefault(x => x.Email == request.Email);
            if (user == null)
            {
                GoogleRegister(new GoogleRegisterRequest { Email = request.Email, Name = request.Name, Token = request.RefreshToken });

            }

            user.Password = CreateSHA256(request.RefreshToken);

            var userProvider = db.UserProviders.FirstOrDefault(x => x.UserId == user.Id);
            if (userProvider == null)
                return new BaseResponse<UserSessionModel>("Пользователя не существует");
            userProvider.Token = request.RefreshToken;

            db.SaveChanges();
           var session= GoogleLogin(new GoogleLoginRequest { Email = request.Email, Token = request.RefreshToken });
            return new BaseResponse<UserSessionModel>(session.Data);
        }

        /// <summary>
        /// Регистрация через гугл
        /// </summary>
        /// <param name="request">Мэил,имя,токен</param>
        /// <returns></returns>
        public BaseResponse GoogleRegister(GoogleRegisterRequest request)
        {
            using var db = new CasinoDbContext(_optionsDb);
            

            db.Users.Add(new Users
            {
                Email = request.Email,
                Name = request.Name,
             
            });
            db.UserProviders.Add(new UserProvider
            {
                ProviderType = DataContext.Enums.EnumProviderType.Google,
                Token = request.Token,

            });
            db.SaveChanges();
            var user = db.Users.FirstOrDefault(x => x.Email == request.Email);
            if (user == null)
                return new BaseResponse("Пользователь не найден.");
            return new BaseResponse();
        }
        /// <summary>
        /// Авторизация через гугл
        /// </summary>
        /// <param name="request">емэил,токен</param>
        /// <returns></returns>
        public BaseResponse<UserSessionModel> GoogleLogin(GoogleLoginRequest request)
        {
            using var db = new CasinoDbContext(_optionsDb);
            var user = db.Users.FirstOrDefault(x => x.Email == request.Email);
            if (user == null)
                return new BaseResponse<UserSessionModel>("Пользователь не найден.");
            var userProvider = db.UserProviders.FirstOrDefault(x => x.UserId == user.Id && x.Token == request.Token);
            if (userProvider == null)
                return new BaseResponse<UserSessionModel>("Некорректные данные.");

            var dataClose = DateTime.UtcNow;
            dataClose.AddHours(3);
            var session = new UserSession
            {
                UserId = user.Id,
                Token = Guid.NewGuid(),
                DateCreate = DateTime.UtcNow,
                DateClose = dataClose
            };
            db.UserSessions.Add(session);
            db.SaveChanges();
            var response = new UserSessionModel
            {
                Id = session.Id,
                Token = session.Token,
                DateClose = session.DateClose,
                DateCreate = session.DateCreate
            };
            return new BaseResponse<UserSessionModel>(response);
        }
        /// <summary>
        /// Хэширование пароля
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string CreateSHA256(string input)
        {
            using SHA256 hash = SHA256.Create();
            return Convert.ToHexString(hash.ComputeHash(Encoding.UTF8.GetBytes(input)));
        }


    }
}
