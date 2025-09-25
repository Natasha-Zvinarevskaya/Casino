using Casino.Services.Models;
using Casino.Services.Models.BlackjackGame.Response;
using Casino.Services.Models.UserService.Request;
using Casino.Services.Models.UserService.Response;
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
         BaseResponse<ShowUserDataResponse> GetUserData(int userId);
        BaseResponse ChangeUserName(BaseUserIdReq<ChangeUserNameRequest> request);
        BaseResponse SaveUserImage(BaseUserIdReq<SaveUserImageRequest> request);




    }
}
