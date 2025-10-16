using Casino.Services.Models;
using Casino.Services.Models.BlackjackGame.Response;
using Casino.Services.Request.GoogleAuth;
using Casino.Services.Request.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.Interfaces
{
    public interface IGoogleService
    {
        string GoogleProvider(GetAuthUrlRequest request);
        Task<string> GetTokenGoogle(CallbackGoogleRequest request);
        BaseResponse<ResponseGetEmail> GetEmail(string id_token);
        BaseResponse<UserSessionModel> CheckUserExists(CheckUserExistsRequest request);
        BaseResponse GoogleRegister(GoogleRegisterRequest request);
        BaseResponse<UserSessionModel> GoogleLogin(GoogleLoginRequest request);


    }
}
