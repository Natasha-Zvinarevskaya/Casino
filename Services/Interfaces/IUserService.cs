using Casino.Services.Models;
using Casino.Services.RequestResponse.Users.Request;
using Casino.Services.RequestResponse.UserService.Request;
using Casino.Services.RequestResponse.UserService.Response;
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
        BaseResponse<List<int>> GetListUsersId(BaseGameIdReq req);




    }
}
