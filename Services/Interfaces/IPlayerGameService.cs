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
        BaseResponse ConnectPlayer(int userId, int gameId);
        BaseResponse DisconnectPlayer(int userId, int gameId);
        void CreateGame(BaseUserIdReq<StartGameRequest> request);
        bool IsGameReady(int gameId);


    }
}
