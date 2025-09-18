using Casino.DataContext;
using Casino.Services.Models;
using Casino.Services.Models.UserTransactionService.Request;
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


    }
}
