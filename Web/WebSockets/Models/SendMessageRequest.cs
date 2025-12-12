namespace Casino.Web.WebSockets.Models
{
    public class SendMessageRequest
    {
        public int CurrentUserId { get; set; }
        public string? Controller { get; set; }
        public string? Method { get; set; }
        public string Value { get; set; }

        public List<int>? UserIds { get; set; }
    }
}
