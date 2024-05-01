using GroqNet.ChatCompletions;
using GroqNet.Serialization;
using GroqNet.Sse;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GroqNet;

public sealed class GroqClient
{
    private readonly HttpClient client;
    private readonly ILogger logger;
    private readonly GroqModel model;

    private const string GroqApiVersion = "1";
    private const string GroqEndpoint = $"https://api.groq.com/openai/v{GroqApiVersion}/";

    public static JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        Converters = {
            new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower),
            new JsonGroqChatRoleConverter(),
            new JsonGroqModelConverter()
        },
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public GroqClient(string apiKey, GroqModel model, HttpClient? httpClient = null, ILogger<GroqClient>? logger = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(apiKey, nameof(apiKey));
        ArgumentException.ThrowIfNullOrEmpty(model.Value, nameof(model));

        this.model = model;

        client = httpClient ?? new HttpClient();
        client.BaseAddress = new Uri(GroqEndpoint);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        client.DefaultRequestHeaders.Add("Accept", "application/json");

        this.logger = logger ?? NullLogger<GroqClient>.Instance;
    }

    public async Task<GroqChatCompletions> GetChatCompletionsAsync(
        IList<GroqMessage> messages,
        GroqChatCompletionOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(messages, nameof(messages));

        options ??= new GroqChatCompletionOptions();

        var request = new GroqChatCompletionsRequest
        {
            Model = model,
            Messages = messages,
            MaxTokens = options.MaxTokens,
            Temperature = options.Temperature,
            TopP = options.TopP,
            Stop = options.Stop,
            Stream = false,
            ResponseFormat = options.ResponseFormat,
            Tools = options.Tools
        };

        var response = await GetResponseAsync(request, cancellationToken);
        var content = await response.Content.ReadAsStreamAsync(cancellationToken);
        var result = await JsonSerializer.DeserializeAsync<GroqChatCompletions>(content, SerializerOptions, cancellationToken);

        if (result == null)
        {
            throw new InvalidOperationException("Failed to deserialize Groq response.");
        }

        return result;
    }

    public async IAsyncEnumerable<StreamingUpdate> GetChatCompletionsStreamingAsync(
        IList<GroqMessage> messages,
        GroqChatCompletionOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(messages, nameof(messages));

        options ??= new GroqChatCompletionOptions();

        var request = new GroqChatCompletionsRequest
        {
            Model = model,
            Messages = messages,
            MaxTokens = options.MaxTokens,
            Temperature = options.Temperature,
            TopP = options.TopP,
            Stop = options.Stop,
            Stream = false,
            ResponseFormat = options.ResponseFormat,
            Tools = options.Tools
        };

        var response = await GetResponseAsync(request, cancellationToken);
        var content = await response.Content.ReadAsStreamAsync(cancellationToken);
        var stream = SseAsyncEnumerator<StreamingUpdate>.EnumerateFromSseStream(
            content, 
            e => JsonSerializer.Deserialize<StreamingUpdate>(e, SerializerOptions), 
            cancellationToken);

        await foreach (var item in stream)
        {
            yield return item;
        }
    }

    private async Task<HttpResponseMessage> GetResponseAsync(GroqChatCompletionsRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        int retryAfterSeconds = 0;

        try
        {
            var jsonContent = JsonSerializer.Serialize(request, SerializerOptions);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("chat/completions", httpContent, cancellationToken)
                                       .ConfigureAwait(false);

            if (response == null)
            {
                throw new InvalidOperationException("The Groq response is null.");
            }

            retryAfterSeconds = GetRetryAfterSeconds(response);
            response.EnsureSuccessStatusCode();

            LogRateLimits(response);

            return response;
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.TooManyRequests)
        {
            logger?.LogError($"Rate limit reached, sleeping for {retryAfterSeconds} seconds.");

            // Sleep for the number of seconds specified by the server
            await Task.Delay(retryAfterSeconds * 1000, cancellationToken);

            // Retry the request
            return await GetResponseAsync(request, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to retrieve Groq completions.", ex);
        }
    }

    private int GetRetryAfterSeconds(HttpResponseMessage response)
    {
        if (response.StatusCode == HttpStatusCode.TooManyRequests)
        {
            if (response.Headers.TryGetValues("retry-after", out var retryAfterValues))
            {
                return int.Parse(retryAfterValues?.FirstOrDefault() ?? "0");
            }
        }

        return 0;
    }

    private void LogRateLimits(HttpResponseMessage response)
    {
        if (response.Headers.TryGetValues("x-ratelimit-remaining-requests", out var remainingRequestsValues))
        {
            var remainingRequests = int.Parse(remainingRequestsValues?.FirstOrDefault() ?? "0");
            logger?.LogInformation($"Rate limit for requests: {remainingRequests}/day remaining.");
        }

        if (response.Headers.TryGetValues("x-ratelimit-remaining-tokens", out var remainingTokensValues))
        {
            var remainingTokens = int.Parse(remainingTokensValues?.FirstOrDefault() ?? "0");
            logger?.LogInformation($"Rate limit for tokens: {remainingTokens}/minute remaining.");
        }

        if (response.Headers.TryGetValues("x-ratelimit-reset-requests", out var resetRequestsValues))
        {
            var resetRequests = resetRequestsValues?.FirstOrDefault() ?? "0";
            logger?.LogInformation($"Rate limit for requests resets in: {resetRequests}.");
        }

        if (response.Headers.TryGetValues("x-ratelimit-reset-tokens", out var resetTokensValues))
        {
            var resetTokens = resetTokensValues?.FirstOrDefault() ?? "0";
            logger?.LogInformation($"Rate limit for tokens resets in: {resetTokens}.");
        }
    }
}
