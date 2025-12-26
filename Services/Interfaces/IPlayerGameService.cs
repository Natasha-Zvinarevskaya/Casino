using Casino.DataContext;
using Casino.DataContext.Enums;
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
        BaseResponse EndGame(EndGameRequest request);
        void SaveGameHistory(SaveGameHistoryRequest request);
        GetHistoryResponse GetHistory(GetHistoryRequest request);
        BaseResponse<EnumStatusGame> ConnectPlayer(BaseUserIdReq<ConnectPlayerRequest> request);
        BaseResponse DisconnectPlayer(BaseUserIdReq<int> request);
        CreateGameResponce CreateGame(BaseUserIdReq<StartGameRequest> request);
        EnumStatusGame IsGameReady(IsGameReadyRequest req);


    }
}
