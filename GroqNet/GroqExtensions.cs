using GroqNet.ChatCompletions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GroqNet
{
    public static class GroqExtensions
    {
        public static void AddGroqClient(
            this IServiceCollection services,
            string apiKey,
            GroqModel model,
            string? serviceId = null)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentException.ThrowIfNullOrWhiteSpace(apiKey);
            ArgumentNullException.ThrowIfNull(model);

            Func<IServiceProvider, object?, GroqClient> factory = (serviceProvider, _) =>
                new(apiKey,
                    model,
                    serviceProvider?.GetService<HttpClient>(),
                    serviceProvider?.GetService<ILogger<GroqClient>>());

            services.AddKeyedSingleton(serviceId, factory);
        }
    }
}
