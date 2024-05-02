# Groq .NET Core Client Service

[![NuGet version (GroqNet)](https://img.shields.io/nuget/v/GroqNet.svg?style=flat-square)](https://www.nuget.org/packages/GroqNet/)
[![NuGet downloads (GroqNet)](https://img.shields.io/nuget/dt/GroqNet.svg?style=flat-square)](https://www.nuget.org/packages/GroqNet/)
[![GitHub license](https://img.shields.io/github/license/igochkov/GroqNet)](https://github.com/igochkov/GroqNet)

The Groq .NET Core is a library for interacting with the Groq API, the quickest LLM inference method available yet. 

## About

The library provides a simple and easy-to-use interface for accessing the Groq API, allowing developers to integrate the platform's capabilities into their applications.

## Features

* Sends HTTP requests to the Groq API
* Streaming chat completions
* Handles rate limiting and retries when necessary
* Supports JSON serialization and deserialization using System.Text.Json
* Can be used with ILogger for logging

## Usage

Here's an example of how to use the client library:

```csharp
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

// -- Example 1: Get chat completions without streaming
var result = await groqClient.GetChatCompletionsAsync(history);

Console.WriteLine(result.Choices.First().Message.Content);
Console.WriteLine($"Total tokens used: {result.Usage.TotalTokens}; Time to response: {result.Usage.TotalTime} sec.");

// -- Example 2: Get chat completions with streaming
await foreach (var msg in groqClient.GetChatCompletionsStreamingAsync(history))
{
    Console.WriteLine(msg.Choices[0].Delta.Content);

    if (msg?.XGroq?.Usage != null)
    {
        Console.WriteLine($"Total tokens used: {msg?.XGroq?.Usage.TotalTokens}; Time to response: {msg?.XGroq?.Usage.TotalTime} sec.");
    }
}
```

## License

The Groq .NET Core Client Library is licensed under the MIT License.

## Contribution

Contributions are welcome! If you find a bug or have an idea for a new feature, please open an issue and let us know.
