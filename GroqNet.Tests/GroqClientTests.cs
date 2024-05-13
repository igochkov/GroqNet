using GroqNet.ChatCompletions;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using Xunit;

namespace GroqNet.Tests
{
    public class GroqClientTests
    {
        [Fact]
        public void Constructor_NoKey_Throws_ArgumentNullException()
        {
            // Arrange
            string? apiKey = null;
            var model = GroqModel.LLaMA3_8b;
            var messageHandlerStub = new HttpMessageHandlerStub();
            var httpClient = new HttpClient(messageHandlerStub, false);
            var logger = new Mock<ILogger<GroqClient>>();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new GroqClient(apiKey, model, httpClient, logger.Object));
        }

        [Fact]
        public void Constructor_NoModel_Throws_ArgumentNullException()
        {
            // Arrange
            var apiKey = "NOKEY";
            GroqModel model;
            var messageHandlerStub = new HttpMessageHandlerStub();
            var httpClient = new HttpClient(messageHandlerStub, false);
            var logger = new Mock<ILogger<GroqClient>>();


            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new GroqClient(apiKey, model, httpClient, logger.Object));
        }

        [Fact]
        public void Constructor_NoHttpClient_Success()
        {
            // Arrange
            var apiKey = "NOKEY";
            var model = GroqModel.LLaMA3_8b;
            HttpClient? httpClient = null;
            var logger = new Mock<ILogger<GroqClient>>();

            // Act
            var client = new GroqClient(apiKey, model, httpClient, logger.Object);

            // Assert
            Assert.NotNull(client);
        }

        [Fact]
        public void Constructor_NoLogger_Success()
        {
            // Arrange
            var apiKey = "NOKEY";
            var model = GroqModel.LLaMA3_8b;
            var messageHandlerStub = new HttpMessageHandlerStub();
            var httpClient = new HttpClient(messageHandlerStub, false);
            ILogger<GroqClient>? logger = null;

            // Act
            var client = new GroqClient(apiKey, model, httpClient, logger);

            // Assert
            Assert.NotNull(client);
        }

        [Fact]
        public async Task GetChatCompletionsAsync_NoConversation_Throws_ArgumentNullException()
        {
            // Arrange
            var apiKey = "NOKEY";
            var model = GroqModel.LLaMA3_8b;
            var messageHandlerStub = new HttpMessageHandlerStub();
            var httpClient = new HttpClient(messageHandlerStub, false);
            var options = new Mock<GroqChatCompletionOptions>();
            var logger = new Mock<ILogger<GroqClient>>();

            IList<GroqMessage>? conversation = null;

            // Act
            var client = new GroqClient(apiKey, model, httpClient, logger.Object);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.GetChatCompletionsAsync(conversation, options.Object));
        }

        [Fact]
        public async Task GetChatCompletionsAsync_ValidRequest_ReturnsGroqResponse()
        {
            // Arrange
            var apiKey = "NOKEY";
            var model = GroqModel.LLaMA3_8b;
            var conversation = new GroqChatHistory { new("Hello") };

            var fileContent = File.ReadAllText("data/response.json");
            var messageHandlerStub = new HttpMessageHandlerStub();
            messageHandlerStub.ResponseToReturn = new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent(fileContent) };
            var httpClient = new HttpClient(messageHandlerStub, false);
            var options = new Mock<GroqChatCompletionOptions>();
            var logger = new Mock<ILogger<GroqClient>>();

            // Act
            var client = new GroqClient(apiKey, model, httpClient, logger.Object);
            var response = await client.GetChatCompletionsAsync(conversation, options.Object);

            // Assert
            Assert.NotNull(response);
            Assert.Equal("Hello World", response.Choices[0].Message.Content);
        }

        [Fact]
        public async Task GetChatCompletionsAsync_TooManyRequests_RetriesAfterDelay()
        {
            // Arrange
            var apiKey = "NOKEY";
            var model = GroqModel.LLaMA3_8b;
            var conversation = new GroqChatHistory { new("Hello") };

            var fileContent = File.ReadAllText("data/response.json");
            var httpResponse1 = new HttpResponseMessage { StatusCode = HttpStatusCode.TooManyRequests, Content = new StringContent(fileContent) };
            httpResponse1.Headers.Add("retry-after", "2");
            var httpResponse2 = new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent(fileContent) };
            var messageHandlerMock = new Mock<HttpMessageHandler>();
            messageHandlerMock.Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponse1)
                .ReturnsAsync(httpResponse2);
            var httpClient = new HttpClient(messageHandlerMock.Object, false);
            var options = new Mock<GroqChatCompletionOptions>();
            var logger = new Mock<ILogger<GroqClient>>();
            var sw = new Stopwatch();

            // Act
            var client = new GroqClient(apiKey, model, httpClient, logger.Object);

            sw.Start();
            var response = await client.GetChatCompletionsAsync(conversation, options.Object);
            sw.Stop();

            // Assert
            Assert.NotNull(response);
            Assert.True(sw.ElapsedMilliseconds >= 2000);
            Assert.Equal("Hello World", response.Choices[0].Message.Content);
        }

        [Fact]
        public async Task GetChatCompletionsAsync_RespectRateLimits()
        {
            // Arrange
            var apiKey = "NOKEY";
            var model = GroqModel.LLaMA3_8b;
            var conversation = new GroqChatHistory { new("Hello") };

            var fileContent = File.ReadAllText("data/response.json");
            var messageHandlerStub = new HttpMessageHandlerStub();
            messageHandlerStub.ResponseToReturn = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(fileContent),
                Headers =
                {
                    { "x-ratelimit-limit-requests", "14400" },
                    { "x-ratelimit-limit-tokens", "18000" },
                    { "x-ratelimit-remaining-requests", "14370" },
                    { "x-ratelimit-remaining-tokens", "17997" },
                    { "x-ratelimit-reset-requests", "1h2m59.56s" },
                    { "x-ratelimit-reset-tokens", "7.66s" }
                }
            };

            var httpClient = new HttpClient(messageHandlerStub, false);
            var options = new Mock<GroqChatCompletionOptions>();
            var logger = new Mock<ILogger<GroqClient>>();

            // Act
            var client = new GroqClient(apiKey, model, httpClient, logger.Object);
            var response = await client.GetChatCompletionsAsync(conversation, options.Object);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(14400, client.CurrentRateLimits.LimitRequests);
            Assert.Equal(18000, client.CurrentRateLimits.LimitTokens);
            Assert.Equal(14370, client.CurrentRateLimits.RemainingRequests);
            Assert.Equal(17997, client.CurrentRateLimits.RemainingTokens);
            Assert.Equal(new TimeSpan(0, 1, 2, 59, 560), client.CurrentRateLimits.ResetRequests);
            Assert.Equal(new TimeSpan(0, 0, 0, 7, 660), client.CurrentRateLimits.ResetTokens);
        }
    }
}