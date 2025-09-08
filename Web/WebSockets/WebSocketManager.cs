using Casino.DataContext;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Net.WebSockets;

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
        /// Удаление веб сокета и пользователя из коллекции
        /// </summary>
        /// <param name="socket">сокет</param>
        /// <param name="user">ползователь</param>
        public void RemoveSocket (WebSocket socket)
        {
            //Пробует найти сокет по значению, если находит то создает новую переменную юзерИд, которую мы отправляем обратно
            //if (_connections.TryGetValue(socket, out var userId) == null)-получить пользователя
                _connections.TryRemove(socket,out _);
        }
    }
}
