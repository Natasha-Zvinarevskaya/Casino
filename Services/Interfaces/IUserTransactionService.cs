using Casino.DataContext;
using Casino.Services.Models;
using Casino.Services.RequestResponse.UserTransactionService.Request;
using Casino.Services.RequestResponse.UserTransactionService.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.Interfaces
{
    public interface IUserTransactionService
    {
        void EndGameTransaction(int gameId,int userId);
        void ReplenishmentBalance(BaseUserIdReq<TopUpBalanceRequest> request);
        BaseResponse<GetHistoryTransactionResponse> GetHistoryTransactions(int userId);



    }
}
