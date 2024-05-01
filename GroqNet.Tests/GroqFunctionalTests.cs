using GroqNet.ChatCompletions;
using Microsoft.Extensions.Logging;
using System.Net;
using Xunit;

namespace GroqNet.Tests
{
    public class GroqFunctionalTests
    {
        //[Fact]
        public async Task GetChatCompletionsAsync_NoApiKey()
        {
            // Arrange
            var apiKey = "123";

            using var httpClient = new HttpClient();
            var logger = new Logger<GroqClient>(new LoggerFactory());
            var cancellationToken = new CancellationToken();
            var options = new GroqChatCompletionOptions(1.0m, 100, 1.0m);
            var model = GroqModel.LLaMA3_8b;
            var conversation = new GroqChatHistory
            {
                new("What is the capital of France?")
            };

            try
            {
                // Act
                var client = new GroqClient(apiKey, model, httpClient, logger);
                await client.GetChatCompletionsAsync(conversation, options, cancellationToken);
            }
            catch (InvalidOperationException ex)
            {
                // Assert
                Assert.IsType<HttpRequestException>(ex.InnerException);
                Assert.Equal(HttpStatusCode.Unauthorized, ((HttpRequestException)ex.InnerException).StatusCode);
            }
        }

        //[Fact]
        public async Task GetChatCompletionsAsync_NonStreaming()
        {
            // Arrange
            var apiKey = Environment.GetEnvironmentVariable("API_Key_Groq", EnvironmentVariableTarget.User);
            var model = GroqModel.LLaMA3_8b;
            using var httpClient = new HttpClient();
            var logger = new Logger<GroqClient>(new LoggerFactory());
            var cancellationToken = new CancellationToken();
            var options = new GroqChatCompletionOptions(1.0m, 1024, 1.0m);
            var conversation = new GroqChatHistory
            {
                new("What is the capital of France?")
            };

            // Act
            var client = new GroqClient(apiKey, model, httpClient, logger);
            var response = await client.GetChatCompletionsAsync(conversation, options, cancellationToken);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.Choices.Count > 0);
            Assert.Equal(GroqChatRole.Assistant, response.Choices[0].Message.Role);
        }
    }
}
