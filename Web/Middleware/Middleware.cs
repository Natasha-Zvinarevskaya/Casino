using Azure.Core;
using Casino.Web.WebSockets;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace Casino.Web.Middleware
{
    public class Middleware
    {
        private readonly RequestDelegate next;
        public Middleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var dateStart = DateTime.UtcNow;
            try
            {
            

                //var request = context.Request;
                //if (request.Method == HttpMethods.Post && request.ContentLength > 0)
                //{
                //    HttpRequestRewindExtensions.EnableBuffering(request);
                //    var buffer = new byte[Convert.ToInt32(request.ContentLength)];
                //    await request.Body.ReadAsync(buffer, 0, buffer.Length);
                //    //get body string here...
                //    var requestContent = Encoding.UTF8.GetString(buffer);
                //    request.Body.Position = 0;
                //}

                //var email = context.Request.Query["email"];
                //Console.WriteLine(email);

       

                



            }
            catch (Exception ex)
            {
                Console.WriteLine("");
            }
            finally
            {
                var dateEnd = DateTime.UtcNow;
                var request = context.Request;
                await next.Invoke(context);
                if (request.Method == HttpMethods.Post && request.ContentLength > 0)
                {
                    var response = new LogRequest
                    {
                        RequestId = Guid.NewGuid(), //guid
                        DateStart = dateStart,
                        LoggerName = "TestLoggerName",
                        Url = context.Request.GetDisplayUrl(),
                        Type = 1, //int
                        RequestLenght = 1, //int
                        ResponseLenght = 1, //int
                        DateEnd = dateEnd,
                        LevelId = 1, //int
                        SessionToken = new Guid("FA71B9F4-BF59-4F0E-9234-67AD260444C1"), //guid
                        UserId = "TestUserId",
                        Ip = "TestIp",
                        ShortMessage = "Middleware",
                        UserAgent = "TestUserAgent",
                        Error = "TestError",
                        RequestBody = "TestRequestBody",
                        Message = "TestMessage"
                    };
                    // string jsonLogRequest = JsonSerializer.Serialize(response);

                    var messageRequest = new MessageRequest { Controller = "Log", Method = "AddLog", Value = response };
                    string jsonMessage = JsonSerializer.Serialize(messageRequest);
                    var wsClient = new WsClient();
                    wsClient.SendMessage(jsonMessage);
                }
            }
            //var request = context.Request;
            //if (request.Method == HttpMethods.Post && request.ContentLength > 0)
            //{
            //    HttpRequestRewindExtensions.EnableBuffering(request);
            //    var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            //    await request.Body.ReadAsync(buffer, 0, buffer.Length);
            //    //get body string here...
            //    var requestContent = Encoding.UTF8.GetString(buffer);
            //    request.Body.Position = 0;
            //}

            //var email = context.Request.Query["email"];
            //Console.WriteLine(email);

            //await next.Invoke(context);
            //if (request.Method == HttpMethods.Post && request.ContentLength > 0)
            //{

            //}

        }


    }
}
