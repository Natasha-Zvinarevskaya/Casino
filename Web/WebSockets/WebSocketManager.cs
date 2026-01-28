using Casino.DataContext;
using Casino.Web.Middlewares;
using Casino.Web.WebSockets.Models;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace Casino.Web.WebSockets
{
    public class WebSocketManager
    {
        //WebSocketManager(List<WsUser>),Add,Remove,GetUser return-,WebSocketUser(SessionId,userId,Name,Token)

        //Коллекция - хранит всю инфу об открытом веб сокете и пользователе, подключенному к этому веб сокету
        //ридонли позволяет изменять данные внутри коллецкии, но не позволяет полностью перезаписывать эту переменную на другие данные
        private readonly ConcurrentDictionary<WebSocket, WsUser> _connections = new();

        /// <summary>
        /// Добавить веб сокет и пользователя в коллекцию
        /// </summary>
        public void AddSocket(WebSocket socket, WsUser user)
        {
            _connections.TryAdd(socket, user);
        }
        /// <summary>
        /// Получить пользователя по сокету
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public WsUser GetUser(WebSocket socket)
        {
            return _connections.TryGetValue(socket, out var user) ? user : (WsUser)null;
        }
        /// <summary>
        /// Удаление веб сокета и пользователя из коллекции
        /// </summary>
        /// <param name="socket">сокет</param>
        /// <param name="user">ползователь</param>
        public void RemoveSocket(WebSocket socket)
        {
            //Пробует найти сокет по значению, если находит то создает новую переменную юзерИд, которую мы отправляем обратно
            //if (_connections.TryGetValue(socket, out var userId) == null)-получить пользователя
            _connections.TryRemove(socket, out _);
        }

        /// <summary>
        /// Отправляет одному подключенному пользователю
        /// </summary>
        /// <param name="request"></param>
        public void SendMessageToUser<T>(SendMessageRequest<T> request)
        {
            // var valueJson = JsonSerializer.Serialize(request.Value);
            if (request.UserIds.Count > 1)
                throw new Exception("Невозможно отправить сообщение нескольким пользователям;");
            foreach (var connection in _connections)
            {
                var user = connection.Value;
                foreach (var userId in request.UserIds)
                {
                    if (user.UserId == userId)
                    {
                        var pushMessage = new NotificationRequest<object>
                        {
                            Controller = request.Controller,
                            Method = request.Method,
                            Value = request.Value
                        };
                        string json = JsonSerializer.Serialize(pushMessage);
                        var buffer = Encoding.UTF8.GetBytes(json);
                        var segment = new ArraySegment<byte>(buffer);
                        connection.Key.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                }
            }
        }
        /// <summary>
        /// Отправляет всем подключенным пользователям
        /// </summary>
        /// <param name="request"></param>
        public void SendMessageSelectedUsers<T>(SendMessageRequest<T> request)
        {
            var connectionsSend = _connections.Where(x => request.UserIds.Contains(x.Value.UserId)).ToList();

            foreach (var connection in connectionsSend)
            {
                if (connection.Value.UserId != request.CurrentUserId)
                {
                    var pushMessage = new NotificationRequest<object>
                    {
                        Controller = request.Controller,
                        Method = request.Method,
                        Value = request.Value
                    };
                    string json = JsonSerializer.Serialize(pushMessage);
                    var buffer = Encoding.UTF8.GetBytes(json);
                    var segment = new ArraySegment<byte>(buffer);
                    connection.Key.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
                }

            }
        }
            /// <summary>
            /// Отправляет всем подключенным пользователям
            /// </summary>
            /// <param name="request"></param>
        public void SendMessageAllUsers<T>(SendMessageRequest<T> request)
        {
            foreach (var connection in _connections)
            {
                if (connection.Value.UserId != request.CurrentUserId)
                {
                    var pushMessage = new NotificationRequest<object>
                    {
                        Controller = request.Controller,
                        Method = request.Method,
                        Value = request.Value
                    };
                    string json = JsonSerializer.Serialize(pushMessage);
                    var buffer = Encoding.UTF8.GetBytes(json);
                    var segment = new ArraySegment<byte>(buffer);
                    connection.Key.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
                }

            }
        }
    }
}
