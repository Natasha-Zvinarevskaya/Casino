using System.Text.Json;

namespace Casino.Web.Middlewares
{
    public class MessageRequest
    {
        public string? Controller { get; set; }
        public string? Method { get; set; }
        public LogRequest Value { get; set; }
    }
}
