using Microsoft.Extensions.DependencyInjection;
using Prompt.Application.Services;
using Prompt.Domain.Interface;

namespace Prompt.Application
{
    public static class ApplicationDependecyInjection
    {
        public static void AddApplication(this IServiceCollection service)
        {
            service.AddScoped<IProdutoService, ProdutoService>();
            service.AddScoped<IChatService, ChatService>();
        }
    }
}
