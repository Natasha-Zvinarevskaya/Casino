using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casino.Services.Models;
using Casino.Services.RequestResponse.UserSessionService.Response;

namespace Casino.Services.Interfaces
{
    public interface IUserSessionService
    {
        BaseResponse<CheckUserResponse> CheckUser(string tokenString);

    }
}
