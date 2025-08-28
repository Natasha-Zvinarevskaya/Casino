using Casino.Web.WebSockets;
using Microsoft.AspNetCore.Mvc;

namespace Casino.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController :ControllerBase
    {
        // Разрешаем вызов из WebSocket: пометим атрибутом
        [SocketAction]
        public IActionResult SendMessage(MessageDto dto)
        {
            // Здесь может быть любая логика — запись в БД, публикация в хаб и т.д.
            return Ok(new { Received = dto.Text, From = "TestController.SendMessage" });
        }

        // Пример метода с несколькими параметрами
        [SocketAction]
        public object MultiParam(string text, int level)
        {
            return new { text, level, handledBy = "MultiParam" };
        }
    }

    public class MessageDto
    {
        public string Text { get; set; } = string.Empty;
    }
}

