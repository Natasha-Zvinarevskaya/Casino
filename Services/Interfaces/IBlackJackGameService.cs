using Casino.Services.Models;
using Casino.Services.Models.BlackjackGame;
using Casino.Services.RequestResponse.BlackjackGame.Requests;
using Casino.Services.RequestResponse.BlackjackGame.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.Interfaces
{
    public interface IBlackJackGameService
    {
        BaseResponse<BlackJackGameModel> Play(BlackjackPlayRequest request);
        BaseResponse<BlackJackGameModel> Turn(int gameId, int? userId);
        BaseResponse<BlackJackGameModel> SkipPlayer(int gameId);




    }
}
