using Casino.Services.Models;
using Casino.Services.Request.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.Interfaces
{
    public interface IUserService
    {
        BaseResponse<int> Registration(RegisterRequest request);
        BaseResponse<UserSessionModel> Login(LoginRequest request);



    }
}
