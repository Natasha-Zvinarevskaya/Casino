using Casino.DataContext;
using Casino.Services.Models;
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
        void ReplenishmentBalance(int userId);


    }
}
