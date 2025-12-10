using Casino.DataContext;
using Casino.Services.Interfaces;
using Casino.Services.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.Service
{
    public class UsersGameService
    {
        private DbContextOptions<CasinoDbContext> _options;
        public UsersGameService(IUserTransactionService userService, DbContextOptions<CasinoDbContext> options)
        {
            _options = options;
        }
        //AddUsersRequest request)
        //public BaseResponse GetListUsers(List<int> userIds)
        //{
        //    var db = new CasinoDbContext(_options);
        //    vars
        //}
    }
}
