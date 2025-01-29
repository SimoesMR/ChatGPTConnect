namespace Prompt.Infrastructure.OpenAi
{
    public class OpenAiConfig
    {
        public OpenAiConfig(string apiKey) => ApiKey = apiKey;

        public string ApiKey { get; set; } = string.Empty;
    }
}
