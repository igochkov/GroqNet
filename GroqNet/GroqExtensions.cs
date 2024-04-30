using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GroqNet
{
    public static class GroqExtensions
    {
        public static void AddGroqService(
            this IServiceCollection services,
            string apiKey,
            string model,
            string? serviceId = null)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentException.ThrowIfNullOrWhiteSpace(apiKey);
            ArgumentException.ThrowIfNullOrWhiteSpace(model);

            Func<IServiceProvider, object?, GroqService> factory = (serviceProvider, _) =>
                new(apiKey,
                    model,
                    serviceProvider?.GetService<HttpClient>(),
                    serviceProvider?.GetService<ILogger<GroqService>>());

            services.AddKeyedSingleton(serviceId, factory);
        }
    }
}
