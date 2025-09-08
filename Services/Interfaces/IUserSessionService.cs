using Casino.Services.Models.UserSessionServiceModel.Response;
using Casino.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.Interfaces
{
    public interface IUserSessionService
    {
        BaseResponse<CheckUserResponse> CheckUser(string tokenString);

    }
}
