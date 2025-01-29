using Microsoft.Extensions.Options;
using OpenAI.Chat;
using Prompt.Domain.Entities;
using Prompt.Domain.Interface;
using Prompt.Infrastructure.OpenAi;
using System.Text.Json;

namespace Prompt.Infrastructure.Services
{
    public class OpenAIService : IOpenAIService
    {
        private readonly IProdutoService _produtoService;
        private readonly OpenAiConfig _openAiConfig;
        public OpenAIService(IProdutoService produtoService, OpenAiConfig openAiConfig)
        {
            _produtoService = produtoService;
            _openAiConfig = openAiConfig;
        }
        public string SendMessage(string message)
        {
            //configura o modelo que vai ser utilizado e a key
            ChatClient client = new(model: "gpt-4o", apiKey: _openAiConfig.ApiKey);

            //Funções que vão ser utilizada caso o ChatGTP precise
            ChatCompletionOptions options = new()
            {
                Tools = { getProdutos, getProdutosPorCategoria }
            };

            List<ChatMessage> messages =
            [
                //SystemChatMessage é a configuração do ChatGTP como ele vai ser portar e etc
                new SystemChatMessage(ChatConstants.SystemMessage),
                //Mensagem do usuário
                new UserChatMessage(message),
            ];
            bool requiresAction;
            ChatCompletion completion;

            //Looping que fica verificando se finalizou a reposta do chat
            do
            {
                requiresAction = false;
                completion = client.CompleteChat(messages, options);

                switch (completion.FinishReason)
                {
                    case ChatFinishReason.Stop:
                        {
                            // Add the assistant message to the conversation history.
                            messages.Add(new AssistantChatMessage(completion));
                            break;
                        }

                    case ChatFinishReason.ToolCalls:
                        {
                            // First, add the assistant message with tool calls to the conversation history.
                            messages.Add(new AssistantChatMessage(completion));

                            // Then, add a new tool message for each tool call that is resolved.
                            foreach (ChatToolCall toolCall in completion.ToolCalls)
                            {
                                switch (toolCall.FunctionName)
                                {
                                    case "GetProdutos":
                                        {
                                            string toolResult = GetProdutos();
                                            messages.Add(new ToolChatMessage(toolCall.Id, toolResult));
                                            break;
                                        }
                                    case "GetProdutosPorCategoria":
                                        {
                                            //Pega os argumentos em forma de dicionario
                                            var parameters = JsonSerializer.Deserialize<Dictionary<string, string>>(toolCall.FunctionArguments.ToString());
                                            string toolResult = GetProdutosPorCategoria(parameters["categoria"].ToString());
                                            messages.Add(new ToolChatMessage(toolCall.Id, toolResult));
                                            break;
                                        }

                                    default:
                                        {
                                            // Handle other unexpected calls.
                                            throw new NotImplementedException();
                                        }
                                }
                            }

                            requiresAction = true;
                            break;
                        }

                    case ChatFinishReason.Length:
                        throw new NotImplementedException("Incomplete model output due to MaxTokens parameter or token limit exceeded.");

                    case ChatFinishReason.ContentFilter:
                        throw new NotImplementedException("Omitted content due to a content filter flag.");

                    case ChatFinishReason.FunctionCall:
                        throw new NotImplementedException("Deprecated in favor of tool calls.");

                    default:
                        throw new NotImplementedException(completion.FinishReason.ToString());
                }
            } while (requiresAction);

            return completion.Content.Count > 0 ? completion.Content[0].Text : "Nada encontrato";
        }

        private readonly ChatTool getProdutos = ChatTool.CreateFunctionTool(
            functionName: nameof(GetProdutos),
            functionDescription: "Obtém todos os produtos disponíveis na loja."
        );

        private ChatTool getProdutosPorCategoria = ChatTool.CreateFunctionTool(
            functionName: nameof(GetProdutosPorCategoria),
            functionDescription: "Obtém todos os produtos que estão dentro de uma Categoria",
            functionParameters: BinaryData.FromBytes("""
            {
                "type": "object",
                "properties": {
                    "categoria": {
                        "type": "string",
                        "description": "Categoria que o produto possui"
                    }
                },
                "required": [ "categoria" ]
            }
            """u8.ToArray())
        );

        private string GetProdutos()
        {
            return _produtoService.GetProdutos();
        }
        private string GetProdutosPorCategoria(string categoria)
        {
            return _produtoService.GetProdutosPorCategoria(categoria);
        }
    }
}
