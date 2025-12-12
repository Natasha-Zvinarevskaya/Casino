using Casino.DataContext;
using Casino.Services.Models;
using Casino.Services.RequestResponse.PlayerGameService.Request;
using Casino.Services.RequestResponse.PlayerGameService.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.Interfaces
{
    public interface IPlayerGameService
    {
        int StartGame(StartGameRequest request);
        void EndGame(EndGameRequest request);
        void SaveGameHistory(SaveGameHistoryRequest request);
        GetHistoryResponse GetHistory(GetHistoryRequest request);
        BaseResponse ConnectPlayer(BaseUserIdReq<int> request);
        BaseResponse DisconnectPlayer(BaseUserIdReq<int> request);
        CreateGameResponce CreateGame(BaseUserIdReq<StartGameRequest> request);
        bool IsGameReady(BaseGameIdReq req);


    }
}
