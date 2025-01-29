namespace Prompt.Domain.Interface
{
    public interface IOpenAIService
    {
        string SendMessage(string message);
    }
}
