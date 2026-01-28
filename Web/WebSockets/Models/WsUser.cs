using Casino.DataContext.Enums;

namespace Casino.Web.WebSockets.Models
{
    public class WsUser
    { 
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Name { get;set; }
        public Guid Token{ get; set; }
        public EnumRoles Role { get; set; } 
    }
}
