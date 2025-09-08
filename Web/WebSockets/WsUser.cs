namespace Casino.Web.WebSockets
{
    public class WsUser
    { 
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Name { get;set; }
        public Guid Token{ get; set; }
    }
}
