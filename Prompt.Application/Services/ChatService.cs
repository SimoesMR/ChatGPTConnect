using Prompt.Domain.Interface;
using Prompt.Infrastructure.Services;

namespace Prompt.Application.Services
{
    public class ChatService : IChatService
    {
        private readonly IOpenAIService _openAIService;

        public ChatService(IOpenAIService openAIService)
        {
            _openAIService = openAIService;
        }

        public string ProcessUserMessage(string userMessage)
        {
            if (string.IsNullOrWhiteSpace(userMessage))
                throw new ArgumentException("A mensagem do usuário não pode ser nula ou vazia.");

            return _openAIService.SendMessage(userMessage);
        }
    }
}
