using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Reflection;
using System.Text.Json;
using System.Text;
using Casino.DataContext;
using Casino.Services.Models.BlackjackGame;
using System;
using Azure.Core;
using Casino.Web.Middleware;
using Microsoft.AspNetCore.Http.Extensions;
using static System.Net.Mime.MediaTypeNames;
using Google.Apis.Drive.v3.Data;
using Microsoft.VisualBasic;
using Logger.Extension.Client.Interface;
using Logger.Extension.Client.Models.Enums;
using Logger.Extension.Client;

namespace Casino.Web.WebSockets
{
    public class WebSocketsHelper
    {
        // ----------------- Вспомогательные методы -----------------

        public static async Task<string?> ReceiveStringAsync(WebSocket socket, CancellationToken ct)
        {

            var buffer = new byte[4 * 1024];
            using var ms = new MemoryStream();
            WebSocketReceiveResult? result;

            do
            {
                result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), ct);
                if (result.MessageType == WebSocketMessageType.Close)
                    return null;

                ms.Write(buffer, 0, result.Count);
            } while (!result.EndOfMessage);

            return Encoding.UTF8.GetString(ms.ToArray());
        }

        public static async Task DispatchToControllerAsync(IServiceProvider scopedServices,
            HttpContext context,
            WebSocket socket,
            string messageJson,
            CancellationToken ct,
            string token,
            ILoggerService loggerService)
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            Exception exception = null;
            var dateStart = DateTime.UtcNow;
            string argsBodys = "";
            try
            {
                var doc = JsonDocument.Parse(messageJson);
                var root = doc.RootElement;

                if (!root.TryGetProperty("Controller", out var ctrlEl) ||
                    !root.TryGetProperty("Method", out var methodEl) ||
                    !root.TryGetProperty("Value", out var valueEl))
                {
                    await SendSocketResponse(socket, new { error = "Invalid message format. Required: Controller, Method, Value." }, ct);
                    return;
                }

                var controllerName = ctrlEl.GetString() ?? string.Empty;
                var methodName = methodEl.GetString() ?? string.Empty;

                // Находим тип контроллера: имя + "Controller"
                var targetTypeName = controllerName.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)
                    ? controllerName
                    : controllerName + "WsController";

                var ctrlType = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => SafeGetTypes(a))
                    .FirstOrDefault(t =>
                        string.Equals(t.Name, targetTypeName, StringComparison.OrdinalIgnoreCase) &&
                        typeof(WsController).IsAssignableFrom(t) &&
                        t.IsPublic);

                if (ctrlType == null)
                {
                    await SendSocketResponse(socket, new { error = $"Controller '{targetTypeName}' not found." }, ct);



                    return;
                }

                // Находим метод
                var method = ctrlType.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                    .FirstOrDefault(m => string.Equals(m.Name, methodName, StringComparison.OrdinalIgnoreCase));

                if (method == null)
                {
                    var methods = ctrlType.GetMethods(BindingFlags.Instance | BindingFlags.Public).ToList();
                    await SendSocketResponse(socket, new { error = $"Method '{methodName}' not found in controller '{targetTypeName}'." }, ct);
                    return;
                }




                var connManager = scopedServices.GetRequiredService<WebSocketManager>();
                //Находим пользователя который делает запрос
                var user = connManager.GetUser(socket);
                if (user == null)
                {
                    await SendSocketResponse(socket, new { error = "Unknown session" }, CancellationToken.None);
                    return;
                }



                var controllerInstance = ActivatorUtilities.CreateInstance(scopedServices, ctrlType) as WsController;
                if (controllerInstance == null)
                {
                    await SendSocketResponse(socket, new { error = "Unable to create controller instance." }, ct);
                    return;
                }
                controllerInstance.ControllerContext = new ControllerContext { HttpContext = context };
                //Сохраняем в контролере информацию о пользователе
                controllerInstance.User = user;

                // Подготовка аргументов метода
                var parameters = method.GetParameters();
                var args = new object?[parameters.Length];

                if (parameters.Length == 1)
                {
                    // Попытка десериализовать Value целиком в тип параметра
                    args[0] = JsonSerializer.Deserialize(valueEl.GetRawText(), parameters[0].ParameterType, options);
                }
                else
                {
                    // Для каждого параметра попробуем найти соответствующее свойство в Value по имени,
                    // иначе попробуем десериализовать весь Value в этот тип (fallback).
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        var p = parameters[i];
                        if (valueEl.ValueKind == JsonValueKind.Object && valueEl.TryGetProperty(p.Name, out var prop))
                        {
                            args[i] = JsonSerializer.Deserialize(prop.GetRawText(), p.ParameterType, options);
                        }
                        else
                        {
                            args[i] = JsonSerializer.Deserialize(valueEl.GetRawText(), p.ParameterType, options);
                        }
                    }
                }

                // Вызов метода
                var invokeResult = method.Invoke(controllerInstance, args);
                if (args != null)
                    argsBodys = JsonSerializer.Serialize(args);
                else throw new Exception("args = null");



                // Если метод возвращает Task / Task<T>
                if (invokeResult is Task task)
                {
                    await task; // дождаться выполнения

                    var taskType = task.GetType();
                    if (taskType.IsGenericType)
                    {
                        // Task<T> — получим результат
                        var resultProperty = taskType.GetProperty("Result");
                        var resultValue = resultProperty?.GetValue(task);
                        await SendSocketResponse(socket, resultValue, ct);
                    }
                    else
                    {
                        await SendSocketResponse(socket, new { ok = true }, ct);
                    }
                }
                else
                {
                    // Синхронный результат
                    await SendSocketResponse(socket, invokeResult, ct);
                }
            }
            catch (Exception ex)
            {
                await SendSocketResponse(socket, new { error = ex.Message, stack = ex.StackTrace }, ct);
                exception = ex;

            }
            finally
            {
                try
                {
                    var dateEnd = DateTime.UtcNow;
                    var request = context.Request;
                    var connManager = scopedServices.GetRequiredService<WebSocketManager>();
                    var user = connManager.GetUser(socket);

                    var logLevel = EnumLevelId.Trace;
                    if (exception != null)
                    {
                        logLevel = EnumLevelId.Error;
                        
                    }
                    var loggerRequst = new Logger.Extension.Client.Models.LogRequest(EnumType.WebSocket, logLevel, "WebSocket")
                        .SetRequestBody(argsBodys)
                        .SetUserId(user.UserId.ToString())
                        .SetSessionToken(new Guid(token))
                        .SetIp(context.Connection.RemoteIpAddress?.ToString())
                        .SetUserAgent(context.Request.Headers["User-Agent"].ToString());

                    if (exception != null)
                        loggerRequst.SetError(exception);

                    loggerService.SaveLog(loggerRequst);

                    //var response = new LogRequest
                    //{
                    //    RequestId = Guid.NewGuid(), //guidx   
                    //    DateStart = dateStart,
                    //    LoggerName = "TestLoggerName",
                    //    Url = context.Request.GetDisplayUrl(),
                    //    Type = 1, //int
                    //    RequestLenght = 1, //int
                    //    ResponseLenght = 1, //int
                    //    DateEnd = dateEnd,
                    //    LevelId = 1, //int
                    //    SessionToken = new Guid(token), //guid
                    //    UserId = user.UserId.ToString(),
                    //    Ip = context.Connection.RemoteIpAddress?.ToString(),
                    //    ShortMessage = "WebSocket",
                    //    UserAgent = context.Request.Headers["User-Agent"].ToString(),
                    //    RequestBody = argsBodys,
                    //    Message = "TestMessage"
                    //};

                    //if (exception != null)
                    //{
                    //    var errMsg = $"Message: {exception.GetBaseException().Message} \nStackTrace: {exception.StackTrace}";
                    //    response.Error = errMsg;
                    //}

                    //if (string.IsNullOrEmpty(argsBodys))
                    //{
                    //    var reqBody = "";
                    //    response.RequestBody = reqBody;
                    //}


                    //var messageRequest = new MessageRequest { Controller = "Log", Method = "AddLog", Value = response };
                    //string jsonMessage = JsonSerializer.Serialize(messageRequest);
                    //var wsClient = new WsClient();
                    //wsClient.SendMessage(jsonMessage);
                     
                    
                }
                catch (Exception ex)
                {

                }

            }

            static async Task SendSocketResponse(WebSocket socket, object payload, CancellationToken ct)
            {
                var json = JsonSerializer.Serialize(payload);
                var bytes = Encoding.UTF8.GetBytes(json);
                await socket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, ct);
            }

            // Помощник: безопасный GetTypes (чтобы не падать на недоступных сборках)
            static IEnumerable<Type> SafeGetTypes(Assembly assembly)
            {
                try
                {
                    return assembly.GetTypes();
                }
                catch
                {
                    return Array.Empty<Type>();
                }
            }

        }
    }
}

