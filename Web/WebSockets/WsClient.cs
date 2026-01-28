using Casino.Web.Middlewares;
using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Casino.Web.WebSockets
{
    public class WsClient
    {
      
        async Task<Socket?> ConnectSocketAsync(string url, int port)
        {
            Socket tempSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            using var mySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                await tempSocket.ConnectAsync(url, port);
                using var stream = new NetworkStream(mySocket); // создаем сетевой поток
                
                return tempSocket;
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
                tempSocket.Close();

            }
            return null;
        }

        //async void SocketSendReceiveAsync(string url, int port)
        //{
        //    using Socket? socket = await ConnectSocketAsync(url, port);
        //    if (socket is null)
        //        throw new Exception("Не удалось установить подключение.");

        //    // отправляем данные
        //    var message = $"GET / HTTP/1.1\r\nHost: {url}\r\nConnection: Close\r\n\r\n";
        //    var messageBytes = Encoding.UTF8.GetBytes(message);
        //    await socket.SendAsync(messageBytes);
        //}

    

        public async void SendMessage(string request)
        {

        //ws://localhost:5084/ws?token=FA71B9F4-BF59-4F0E-9234-67AD260444C4
            using var ws = new ClientWebSocket();
           await ws.ConnectAsync(new Uri("ws://logger.com:8081/ws?token=FA71B9F4-BF59-4F0E-9234-67AD260444C4"), CancellationToken.None);

            // сообщение для отправки
            var message = request;
            // считыванием строку в массив байт
            byte[] requestData = Encoding.UTF8.GetBytes(message);
            

            await ws.SendAsync(requestData, System.Net.WebSockets.WebSocketMessageType.Text, true, CancellationToken.None);
            Console.WriteLine("Сообщение отправлено");
        }



    }
}
