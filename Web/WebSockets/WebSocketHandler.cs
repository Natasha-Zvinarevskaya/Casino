using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Casino.Web.WebSockets
{
    public class WebSocketHandler
    {
        private readonly WebSocket _socket;
        public WebSocketHandler(WebSocket socket)
        {
            _socket = socket;
        }
        /// <summary>
        /// Отправка сообщения 
        /// </summary>
        /// <param name="message">сообщение</param>
        /// <returns></returns>
        public async Task SendMessageAsync(object message)
        {
            string json = JsonSerializer.Serialize(message);
            var buffer = Encoding.UTF8.GetBytes(json);
            var segment = new ArraySegment<byte>(buffer);

            await _socket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);

        }
        /// <summary>
        /// Чтение сообщения
        /// </summary>
        /// <returns></returns>
        public async Task<string?> ReceiveMessageAsync()
        {
            var buffer = new byte[1024 * 4];
            var result = await _socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            
            if (result.MessageType==WebSocketMessageType.Close)
            {
                await _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by server", CancellationToken.None);
                return null;
            }
            return Encoding.UTF8.GetString(buffer, 0, result.Count);
        }
        /// <summary>
        /// Закрытие соединения
        /// </summary>
        /// <returns></returns>
        public async Task CloseAsync ()
        {
            await _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed", CancellationToken.None);
        }

    }
}
