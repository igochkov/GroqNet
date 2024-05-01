using GroqNet;
using GroqNet.ChatCompletions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var apiKey = Environment.GetEnvironmentVariable("API_Key_Groq", EnvironmentVariableTarget.User);

var host = new HostBuilder()
    .ConfigureServices(services =>
    {
        services.AddHttpClient();
        services.AddGroqClient(apiKey, GroqModel.LLaMA3_8b);
    }).Build();

var groqClient = host.Services.GetRequiredService<GroqClient>();

var history = new GroqChatHistory
{
    new("What is the capital of France?")
};

var result = await groqClient.GetChatCompletionsAsync(history);

Console.WriteLine(result.Choices.First().Message.Content);
Console.WriteLine($"Total tokens used: {result.Usage.TotalTokens}; Time to response: {result.Usage.TotalTime} sec.");