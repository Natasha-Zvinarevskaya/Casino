using Casino.DataContext;
using Casino.Services.Interfaces;
using Casino.Services.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino
{
    public class ConfigurationService
    {
        public IServiceProvider Init (IConfiguration config)
        {

            var services = new ServiceCollection();
            services.AddDbContext<CasinoDbContext>(options => options.UseSqlServer(config.GetConnectionString(nameof(CasinoDbContext))));
            services.AddScoped<IPlayerGameService, PlayerGameService>();
            services.AddScoped<IUserTransactionService, UserTransactionService>();
            services.AddScoped<IUserService, UserService>();
            return services.BuildServiceProvider();


        }
    }
}
