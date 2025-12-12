namespace Casino.Web.WebSockets.Models
{
    using Microsoft.AspNetCore.Mvc;

    public abstract class WsController : Controller
    {
  

        
        /// <summary>
        /// Удобное свойство для доступа к текущему UserId
        /// </summary>
        public WsUser User { get; set; } // или бросать исключение, если null
    }
}
