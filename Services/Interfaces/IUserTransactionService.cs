using Casino.DataContext;
using Casino.Services.Models;
using Casino.Services.Models.BlackjackGame.Response;
using Casino.Services.Models.UserTransactionService.Request;
using Casino.Services.Models.UserTransactionService.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.Interfaces
{
    public interface IUserTransactionService
    {
        void EndGameTransaction(int gameId);
        void ReplenishmentBalance(TopUpBalanceRequest request);
        BaseResponse<GetHistoryTransactionResponse> GetHistoryTransactions(int userId);



    }
}
