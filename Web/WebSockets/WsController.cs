namespace Casino.Web.WebSockets
{
    using Microsoft.AspNetCore.Mvc;

    public abstract class WsController : ControllerBase
    {
  

        
        /// <summary>
        /// Удобное свойство для доступа к текущему UserId
        /// </summary>
        public WsUser User { get; set; } // или бросать исключение, если null
    }
}
