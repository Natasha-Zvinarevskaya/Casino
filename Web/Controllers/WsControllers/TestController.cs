using Casino.Web.WebSockets.Models;
using Microsoft.AspNetCore.Mvc;

namespace Casino.Web.Controllers.WsControllers
{
    public class TestController :WsController
    {
        //Проверка приходят ли сообщения
        public IActionResult SendMessage(MessageDto dto)
        {
            var test = User;
            // Здесь может быть любая логика — запись в БД, публикация в хаб и т.д.
            return Ok(new { Received = dto.Text, From = "TestController.SendMessage" });
        }

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

