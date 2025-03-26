using GroqNet;
using GroqNet.ChatCompletions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var apiKey = Environment.GetEnvironmentVariable("API_Key_Groq", EnvironmentVariableTarget.User);

var host = new HostBuilder()
    .ConfigureServices(services =>
    {
        services.AddHttpClient();
        services.AddGroqClient(apiKey, GroqModel.Grok2_latest);
    }).Build();

var groqClient = host.Services.GetRequiredService<GroqClient>();

var history = new GroqChatHistory
{
    new("What is the capital of France?")
};

// -- Example 1: Get chat completions without streaming
var result = await groqClient.GetChatCompletionsAsync(history);

Console.WriteLine(result.Choices.First().Message.Content);
Console.WriteLine($"Total tokens used: {result.Usage.TotalTokens}");

// -- Example 2: Get chat completions with streaming
await foreach (var msg in groqClient.GetChatCompletionsStreamingAsync(history))
{
    Console.WriteLine(msg.Choices[0].Delta.Content);

    if (msg?.XGroq?.Usage != null)
    {
        Console.WriteLine($"Total tokens used: {msg?.XGroq?.Usage.TotalTokens}; Time to response: {msg?.XGroq?.Usage.TotalTime} sec.");
    }
}