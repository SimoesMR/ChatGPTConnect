using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prompt.Domain.Interface;
using Prompt.Infrastructure.OpenAi;
using Prompt.Infrastructure.Services;

namespace Prompt.Infrastructure
{
    public static class InfrastructureDependencyInjection
    {
        public static void AddInfrastrucutre(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IOpenAIService, OpenAIService>();

            var openAiKey = configuration.GetSection("OpenAI:ApiKey").Value;
            services.AddSingleton(option => new OpenAiConfig(openAiKey!));
        }
    }
}
