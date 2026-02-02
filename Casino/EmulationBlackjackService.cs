using Casino.DataContext;
using Casino.DataContext.Enums;
using Casino.DataContext.Enums.BlackjackGame;
using Casino.DataContext.Models.BlackjackGame;
using Casino.Interfaces;
using Casino.Models;
using Casino.Models.RequestResponse.EmulationBlackjackService.Request;
using Casino.Models.RequestResponse.EmulationBlackjackService.Response;
using Casino.Services.Interfaces;
using Casino.Services.Models;
using Casino.Services.RequestResponse.BlackjackGame.Requests;
using Casino.Services.RequestResponse.BlackjackGame.Response;
using Casino.Services.RequestResponse.PlayerGameService.Request;
using Casino.Services.RequestResponse.UserService.Request;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino
{
    public class EmulationBlackjackService : IEmulationBlackjackService
    {
        private IBlackJackGameService _blackJackGameService;
        private IPlayerGameService _playerGameService;
        private OptionEmulation _options;
        public EmulationBlackjackService(IBlackJackGameService blackJackGameService, IPlayerGameService playerGameService, IOptions<OptionEmulation> options)
        {
            _blackJackGameService = blackJackGameService;
            _playerGameService = playerGameService;
            _options = options.Value;
        }
        public void StartEmulation()
        {
            var blackjackGameModel = new BlackJackGameModel { };
            int index;
            int indexDealer;
            decimal bet = 5;
            int userId = 17;

            int dealerCheating = 0;
            int dealerRisk = 0;
            int dealerWinCheating = 0;
            int dealerCrook = 0;
            int dealerWinCrook = 0;


            for (int i = 0; i < _options.CountGames; i++)
            {

                var gameId = _playerGameService.CreateGame(new BaseUserIdReq<StartGameRequest>(userId, new StartGameRequest { Game = EnumGames.BlackJack, Bet = bet, MaxCountPlayers = 1 })).GameId;
                _playerGameService.ConnectPlayer(new BaseUserIdReq<ConnectPlayerRequest>(userId, new ConnectPlayerRequest { GameId = gameId, Bet = bet }));

                Console.WriteLine($"Создана игра. gameId: {gameId}");
                Console.WriteLine();



                blackjackGameModel = _blackJackGameService.Play(new BlackjackPlayRequest { GameId = gameId }).Data;

                index = blackjackGameModel.PLayerCards.FindIndex(x => x.UserId != null);
                indexDealer = blackjackGameModel.PLayerCards.FindIndex((x) => x.UserId == null);

                Console.WriteLine($"Очки дилера: {blackjackGameModel.PLayerCards[indexDealer].Score}.");
                Console.WriteLine($"Очки пользователя: {blackjackGameModel.PLayerCards[index].Score}.");
                Console.WriteLine();

                while (blackjackGameModel.PLayerCards[index].Score < _options.BotMaxTake)
                {
                    blackjackGameModel = _blackJackGameService.Turn(new TurnPlayerRequest { GameId = gameId, UserId = userId }).Data;

                    Console.WriteLine("Пользователь добрал карту.");
                    Console.WriteLine($"Очки дилера: {blackjackGameModel.PLayerCards[indexDealer].Score}.");
                    Console.WriteLine($"Очки пользователя: {blackjackGameModel.PLayerCards[index].Score}.");
                    Console.WriteLine();

                }
                
                if (blackjackGameModel.PLayerCards[index].Score >= _options.BotMaxTake && blackjackGameModel.Status == EnumStatusGame.None)
                {
                    Console.WriteLine("Пользователь пропустил ход.");

                    blackjackGameModel = _blackJackGameService.SkipPlayer(new SkipPlayerRequest { GameId = gameId, UserId = userId }).Data;

                    Console.WriteLine($"Игра окончена. {blackjackGameModel.Status.ToString()}");
                    Console.WriteLine($"Очки пользователя: {blackjackGameModel.PLayerCards[index].Score}.");
                    Console.WriteLine($"Очки дилера: {blackjackGameModel.PLayerCards[indexDealer].Score}.");
                    Console.WriteLine("______________________");
                    Console.WriteLine();

                }
                else
                {
                    Console.WriteLine($"Игра окончена. {blackjackGameModel.Status.ToString()}");
                    Console.WriteLine($"Очки пользователя: {blackjackGameModel.PLayerCards[index].Score}.");
                    Console.WriteLine($"Очки дилера: {blackjackGameModel.PLayerCards[indexDealer].Score}.");
                    Console.WriteLine("______________________");
                    Console.WriteLine();
                }
                var response = DealerStatistics(new DealerStatisticsRequest { GameId = gameId });
                if (response.Risk)
                    dealerRisk++;
                if (response.Cheating)
                    dealerCheating++;
                if (response.WinCheating)
                    dealerWinCheating++;
                if (response.IsCrook)
                {
                    dealerCrook++;
                    if (blackjackGameModel.Status == EnumStatusGame.DealerWin)
                        dealerWinCrook++;
                }

            }
            Console.WriteLine();

            Console.WriteLine($"Дилеру выпало рискнуть {dealerRisk} раз.");
            Console.WriteLine($"Дилеру выпало сжульничать {dealerCheating} раз.");
            Console.WriteLine($"Дилеру выпало взять победную карту {dealerWinCheating} раз.");
            Console.WriteLine($"Дилер воспользовался шансом сжульничать {dealerCrook} раз.");
            Console.WriteLine($"Дилер воспользовался шансом сжульничать и выиграл {dealerWinCrook} раз.");

            Console.WriteLine();
            Console.ReadLine();



        }
        private DealerStatisticsResponse DealerStatistics(DealerStatisticsRequest request)
        {
            var history = _playerGameService.GetHistory(new GetHistoryRequest { GameId = request.GameId });
            var response = new DealerStatisticsResponse
            {
                Cheating = history.Cheating,
                Risk = history.Risk,
                WinCheating = history.WinCheating,
                IsCrook = history.IsCrook
            };
            return response;


        }
    }
}
