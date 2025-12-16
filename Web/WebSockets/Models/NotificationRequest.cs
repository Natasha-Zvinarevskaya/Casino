namespace Casino.Web.WebSockets.Models
{
    public class NotificationRequest <T>
    {
        public string? Controller { get; set; }
        public string? Method { get; set; }
        public T Value  { get; set; }
    }
}
