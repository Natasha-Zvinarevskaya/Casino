using Casino.DataContext;
using Casino.Services.Models;
using Casino.Services.Models.BlackjackGame.Response;
using Casino.Services.Models.PlayerGameService.Request;
using Casino.Services.Models.PlayerGameService.Response;
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
        GetHistoryResponse GetHistory(int gameId, int userId);

    }
}
