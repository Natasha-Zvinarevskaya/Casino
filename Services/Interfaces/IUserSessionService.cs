using Casino.Services.Models.UserSessionServiceModel.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casino.Services.Models.BlackjackGame.Response;

namespace Casino.Services.Interfaces
{
    public interface IUserSessionService
    {
        BaseResponse<CheckUserResponse> CheckUser(string tokenString);

    }
}
