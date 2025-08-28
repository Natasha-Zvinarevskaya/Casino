using Casino.DataContext;
using Casino.Services;
using Casino.Services.Interfaces;
using Casino.Web.WebSockets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;
using System.Reflection;
using System.Text.Json;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

            // Для вебСокета
            app.Map("/ws", async context =>
            {
                if (!context.WebSockets.IsWebSocketRequest)
                {
                    context.Response.StatusCode = 400;
                    return;
                }

                using var socket = await context.WebSockets.AcceptWebSocketAsync();
                var ct = CancellationToken.None;

                while (socket.State == WebSocketState.Open)
                {
                    var messageJson = await WebSocketsHelper.ReceiveStringAsync(socket, ct);
                    if (messageJson == null) break;

                    await WebSocketsHelper.DispatchToControllerAsync(context, socket, messageJson, ct);
                }
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
