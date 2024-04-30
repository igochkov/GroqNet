# Groq .NET Core Client Service

[![NuGet version (GroqNet)](https://img.shields.io/nuget/v/GroqNet.svg?style=flat-square)](https://www.nuget.org/packages/GroqNet/)
[![NuGet downloads (GroqNet)](https://img.shields.io/nuget/dt/GroqNet.svg?style=flat-square)](https://www.nuget.org/packages/GroqNet/)
[![GitHub license](https://img.shields.io/github/license/igochkov/groq.net)](https://github.com/igochkov/groq.net)

The Groq .NET Core is a library for interacting with the Groq API, the quickest LLM inference method available yet. 

## About

The library provides a simple and easy-to-use interface for accessing the Groq API, allowing developers to integrate the platform's capabilities into their applications.

## Features

* Sends HTTP requests to the Groq API
* Handles rate limiting and retries when necessary
* Supports JSON serialization and deserialization using System.Text.Json
* Can be used with ILogger for logging

## Installation

To install the Groq .NET Core Client Library, simply run the following command in your .NET Core project:

```
dotnet add package GroqNet
```

## Usage

Here's an example of how to use the client library:

```csharp
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
```

## License

The Groq .NET Core Client Library is licensed under the MIT License.

## Contribution

Contributions are welcome! If you find a bug or have an idea for a new feature, please open an issue and let us know.
