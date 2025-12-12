using System.Text.Json;

namespace Casino.Web.WebSockets.Models
{

    public class SocketMessage
    {

        public string? Controller { get; set; }
        public string? Method { get; set; }
        public JsonElement Value { get; set; }
    }


[AttributeUsage(AttributeTargets.Method)]
    public sealed class SocketActionAttribute : Attribute
    {
    }
}
