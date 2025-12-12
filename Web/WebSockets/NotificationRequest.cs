namespace Casino.Web.WebSockets
{
    public class NotificationRequest
    {
        public string? Controller { get; set; }
        public string? Method { get; set; }
        public string Value { get; set; }
    }
}
