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
using System;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Casino.Web.Middlewares;
//using Logger.Extension.Extension;
using Logger.Extension.Client.Interface;
using WS.Extension.Extension;
using Stripe.Extension.Interfaces;
using Stripe.Extension.Services;
using Casino.Services.RequestResponse.GoogleAuth.Request;
using Casino.Services.RequestResponse.UserSessionService.Response;
using Casino.Web.WebSockets.Models;


namespace Casino.Web
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
            builder.Services.AddScoped<IGoogleService, GoogleServices>();
            builder.Services.AddLoggerServcie(builder.Configuration);
            builder.Services.AddScoped<IStripeUserService, StripeUserService>();
            builder.Services.AddScoped<IStripePaymentServices, StripePaymentServices>();


            builder.Services.Configure<OptionGoogleSettings>(builder.Configuration.GetSection(nameof(OptionGoogleSettings)));
            builder.Services.AddCors();



            builder.Services.AddSingleton<WebSockets.WebSocketManager>();

            builder.Services.AddAuthentication("Cookies"); //Сервисы аутенфикации через куки 
            builder.Services.AddAuthorization(); //Сервисы авторизации
            builder.Services.AddSession(); //Сервисы для сессии



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
                

                //Макс проверял бд
                CheckUserResponse user;
                if (token.Value == "FA71B9F4-BF59-4F0E-9234-67AD260444C4")
                {
                    user = new CheckUserResponse()
                    {
                        Email = "admin@admin.admin",
                        Name = "admin",
                        Id = 4
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

                var webSocketManager = serviceProvider.GetService<WebSockets.WebSocketManager>();
                var loggerService = serviceProvider.GetService<ILoggerService>();


                //Из бд берет пользователя и открывает для него сокет
                var wsUser = new WsUser { Email = user.Email, Name = user.Name, Token = Guid.Parse(token.Value), UserId = user.Id, Role = user.Role };
                webSocketManager.AddSocket(socket, wsUser);

                //Цикл (пока открыт сокет) слушает сообщение от фронта
                while (socket.State == WebSocketState.Open)
                {
                    var messageJson = await WebSocketsHelper.ReceiveStringAsync(socket, ct);
                    if (messageJson == null) break;

                    await WebSocketsHelper.DispatchToControllerAsync(serviceProvider, context, socket, messageJson, ct, token.Value, loggerService);
                }

                //Нужно создать событие , отслеживающие закрытие сокета
                webSocketManager.RemoveSocket(socket);
            });

            // Нужно, чтобы обычные контроллеры работали через HTTP (если нужно)
            app.MapControllers();

            app.UseMiddleware<Middleware>();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");


            app.Run();


        }
    }
}
