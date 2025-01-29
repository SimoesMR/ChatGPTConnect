using Microsoft.AspNetCore.Mvc;
using Prompt.Domain.Interface;

namespace Prompt.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatController : ControllerBase
    {
        [HttpPost]
        public IActionResult SendMessage([FromServices] IChatService chatService, [FromBody] string userMessage)
        {
            if (string.IsNullOrWhiteSpace(userMessage))
            {
                return BadRequest("A mensagem não pode estar vazia.");
            }

            var response = chatService.ProcessUserMessage(userMessage);
            return Ok(response);
        }
    }
}
