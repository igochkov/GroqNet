using GroqNet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var apiKey = Environment.GetEnvironmentVariable("API_Key_Groq", EnvironmentVariableTarget.User);

var host = new HostBuilder()
    .ConfigureServices(services =>
    {
        services.AddHttpClient();
        services.AddGroqService(apiKey, GroqModel.LLaMA3_8b);
    }).Build();

var groqService = host.Services.GetRequiredService<GroqService>();

var conversation = new List<GroqMessage> 
{
    new GroqMessage(ChatRole.User, "What is the capital of France?")
};

var result = await groqService.GetChatCompletionAsync(conversation);

Console.WriteLine(result.Choices.First().Message.Content);
Console.WriteLine($"Total tokens used: {result.Usage.TotalTokens}; Time to response: {result.Usage.TotalTime} sec.");