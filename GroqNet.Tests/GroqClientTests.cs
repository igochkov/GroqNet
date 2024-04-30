using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
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
            var logger = new Mock<ILogger<GroqService>>();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new GroqService(apiKey, model, httpClient, logger.Object));
        }

        [Fact]
        public void Constructor_NoModel_Throws_ArgumentNullException()
        {
            // Arrange
            var apiKey = "NOKEY";
            string? model = null;
            var messageHandlerStub = new HttpMessageHandlerStub();
            var httpClient = new HttpClient(messageHandlerStub, false);
            var logger = new Mock<ILogger<GroqService>>();


            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new GroqService(apiKey, model, httpClient, logger.Object));
        }

        [Fact]
        public void Constructor_NoHttpClient_Success()
        {
            // Arrange
            var apiKey = "NOKEY";
            var model = GroqModel.LLaMA3_8b;
            HttpClient? httpClient = null;
            var logger = new Mock<ILogger<GroqService>>();

            // Act
            var client = new GroqService(apiKey, model, httpClient, logger.Object);

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
            ILogger<GroqService>? logger = null;

            // Act
            var client = new GroqService(apiKey, model, httpClient, logger);

            // Assert
            Assert.NotNull(client);
        }

        [Fact]
        public async Task GetChatCompletionAsync_NoConversation_Throws_ArgumentNullException()
        {
            // Arrange
            var apiKey = "NOKEY";
            var model = GroqModel.LLaMA3_8b;
            var messageHandlerStub = new HttpMessageHandlerStub();
            var httpClient = new HttpClient(messageHandlerStub, false);
            var options = new Mock<GroqChatCompletionOptions>();
            var logger = new Mock<ILogger<GroqService>>();

            IList<GroqMessage>? conversation = null;

            // Act
            var client = new GroqService(apiKey, model, httpClient, logger.Object);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.GetChatCompletionAsync(conversation, options.Object));
        }

        [Fact]
        public async Task GetChatCompletionAsync_ValidRequest_ReturnsGroqResponse()
        {
            // Arrange
            var apiKey = "NOKEY";
            var model = GroqModel.LLaMA3_8b;
            var conversation = new List<GroqMessage>
            {
                new GroqMessage(ChatRole.User, "Hello")
            };

            var fileContent = File.ReadAllText("data/response.json");
            var messageHandlerStub = new HttpMessageHandlerStub();
            messageHandlerStub.ResponseToReturn = new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent(fileContent) };
            var httpClient = new HttpClient(messageHandlerStub, false);
            var options = new Mock<GroqChatCompletionOptions>();
            var logger = new Mock<ILogger<GroqService>>();

            // Act
            var client = new GroqService(apiKey, model, httpClient, logger.Object);
            var response = await client.GetChatCompletionAsync(conversation, options.Object);

            // Assert
            Assert.NotNull(response);
            Assert.Equal("Hello World", response.Choices[0].Message.Content);
        }

        [Fact]
        public async Task GetChatCompletionAsync_TooManyRequests_RetriesAfterDelay()
        {
            // Arrange
            var apiKey = "NOKEY";
            var model = GroqModel.LLaMA3_8b;
            var conversation = new List<GroqMessage>
            {
                new GroqMessage(ChatRole.User, "Hello")
            };

            var fileContent = File.ReadAllText("data/response.json");
            var httpResponse1 = new HttpResponseMessage { StatusCode = HttpStatusCode.TooManyRequests, Content = new StringContent(fileContent) };
            httpResponse1.Headers.Add("retry-after", "1");
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
            var logger = new Mock<ILogger<GroqService>>();
            var sw = new Stopwatch();

            // Act
            var client = new GroqService(apiKey, model, httpClient, logger.Object);

            sw.Start();
            var response = await client.GetChatCompletionAsync(conversation, options.Object);
            sw.Stop();

            // Assert
            Assert.NotNull(response);
            Assert.True(sw.ElapsedMilliseconds >= 1000);
            Assert.Equal("Hello World", response.Choices[0].Message.Content);
        }

        [Fact]
        public async Task GetChatCompletionAsync_LogsRateLimits()
        {
            // Arrange
            var apiKey = "NOKEY";
            var model = GroqModel.LLaMA3_8b;
            var conversation = new List<GroqMessage>
            {
                new GroqMessage(ChatRole.User, "Hello")
            };

            var fileContent = File.ReadAllText("data/response.json");
            var messageHandlerStub = new HttpMessageHandlerStub();
            messageHandlerStub.ResponseToReturn = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(fileContent),
                Headers =
                {
                    { "x-ratelimit-remaining-requests", "10" },
                    { "x-ratelimit-remaining-tokens", "20" },
                    { "x-ratelimit-reset-requests", "12:00:00" },
                    { "x-ratelimit-reset-tokens", "1:00:00" }
                }
            };

            var httpClient = new HttpClient(messageHandlerStub, false);
            var options = new Mock<GroqChatCompletionOptions>();
            var logger = new Mock<ILogger<GroqService>>();

            // Act
            var client = new GroqService(apiKey, model, httpClient, logger.Object);
            var response = await client.GetChatCompletionAsync(conversation, options.Object);

            // Assert
            Assert.NotNull(response);
            logger.Verify(l => l.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Exactly(4));

        }
    }
}