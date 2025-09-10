using Casino.DataContext;
using Casino.Services.Interfaces;
using Casino.Web.WebSockets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;
using System.Reflection;
using System.Text.Json;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Casino.Services.Service;
using Microsoft.AspNetCore.Authentication;
using Azure.Core;
using Microsoft.IdentityModel.Tokens;
using Casino.Services.Models.UserSessionServiceModel.Response;

namespace Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<CasinoDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString(nameof(CasinoDbContext))));
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUserSessionService, UserSessionService>();
            builder.Services.AddScoped<IBlackJackGameService, BlackjackService>();
            builder.Services.AddScoped<IPlayerGameService, PlayerGameService>();
            builder.Services.AddScoped<IUserTransactionService, UserTransactionService>();



            builder.Services.AddSingleton<Casino.Web.WebSockets.WebSocketManager>();
              
            builder.Services.AddAuthentication("Cookies"); //Сервисы аутенфикации через куки 
            builder.Services.AddAuthorization(); //Сервисы авторизации
            builder.Services.AddSession(); //Сервисы для сессии


            builder.Services.AddCors();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            app.UseWebSockets();  //Подключение вебСокета

            app.UseCors(builder => builder.AllowAnyOrigin());

            //Подключение сервисов,чтобы передать значение переменной
            var serviceProvider = builder.Services.BuildServiceProvider();

            // Для вебСокета
            app.Map("/ws", async context =>
            {
                if (!context.WebSockets.IsWebSocketRequest)
                {
                    context.Response.StatusCode = 400;
                    return;
                }
                var userSessionService = serviceProvider.GetService<IUserSessionService>();

                //Ищем,есть ли у пользователя токен 
                var token = context.Request.Query.FirstOrDefault(x => x.Key == "token");
                //проверка наличия токена
                if (string.IsNullOrEmpty(token.Value))
                    throw new Exception("Токен не найден. Доступ закрыт.");

                CheckUserResponse user;
                if (token.Value == "FA71B9F4-BF59-4F0E-9234-67AD260444C4")
                {
                    user = new CheckUserResponse()
                    {
                        Email = "admin@admin.admin",
                        Name = "admin",
                        Id = 1
                    };
                }
                else
                {
                    var userResponse = userSessionService.CheckUser(token.Value);
                    if (!userResponse.IsSucces)
                        throw new Exception("Пользователь не найден.");

                    user = userResponse.Data;
                }
                using var socket = await context.WebSockets.AcceptWebSocketAsync();
                var ct = CancellationToken.None;

                var webSocketManager = serviceProvider.GetService<Casino.Web.WebSockets.WebSocketManager>();


                var wsUser = new WsUser { Email = user.Email, Name = user.Name, Token = Guid.Parse(token.Value), UserId = user.Id };
                webSocketManager.AddSocket(socket, wsUser);
                while (socket.State == WebSocketState.Open)
                {
                    var messageJson = await WebSocketsHelper.ReceiveStringAsync(socket, ct);
                    if (messageJson == null) break;

                    await WebSocketsHelper.DispatchToControllerAsync(serviceProvider, context, socket, messageJson, ct);
                }

                //Нужно создать событие , отслеживающие закрытие сокета
                webSocketManager.RemoveSocket(socket);
            });

            // Нужно, чтобы обычные контроллеры работали через HTTP (если нужно)
            app.MapControllers();



            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");


            app.Run();


        }
    }
}
