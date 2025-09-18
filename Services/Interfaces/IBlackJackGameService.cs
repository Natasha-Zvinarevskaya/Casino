using Casino.Services.Models.BlackjackGame.Requests;
using Casino.Services.Models.BlackjackGame.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.Interfaces
{
   public  interface IBlackJackGameService
    {
        BaseResponse<BlackJackGameModel> Play(BlackjackPlayRequest request,int userId);
        BaseResponse<BlackJackGameModel> PlayerTurn(int gameId);
         BaseResponse<BlackJackGameModel> SkipPlayer(int gameId);



    }
}
