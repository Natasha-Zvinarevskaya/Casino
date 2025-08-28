using Casino.DataContext;
using Casino.Services;
using Casino.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

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

            app.UseCors(builder => builder.AllowAnyOrigin());

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
