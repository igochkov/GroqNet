using System.Text.Json;
using Xunit;

namespace GroqNet.Tests
{
    public class GroqSerializationTests
    {
        [Fact]
        public async Task SerializeAsync_GroqRequest_ReturnsJsonString()
        {
            // Arrange
            var request = new GroqCompletionsRequest
            {
                Model = GroqModel.LLaMA3_8b,
                Messages = new GroqMessage[]
                {
                    new(ChatRole.User,"Hello, how are you?", "Alice", "1")
                },
                Temperature = 0.5M,
                TopP = 0.9M,
                MaxTokens = 100,
                Stream = false,
                ResponseFormat = new GroqResponseFormat() { Type = "json_object" }
            };

            // Act
            using var jsonStream = new MemoryStream();
            await JsonSerializer.SerializeAsync(jsonStream, request, GroqService.SerializerOptions);

            // Assert
            jsonStream.Position = 0;
            var result = new StreamReader(jsonStream).ReadToEnd();

            Assert.Equal(@"
{""model"":""llama3-8b-8192"",
""messages"":[{
""role"":""user"",
""content"":""Hello, how are you?"",
""name"":""Alice"",
""seed"":""1""
}],
""temperature"":0.5,
""max_tokens"":100,
""top_p"":0.9,
""stream"":false,
""response_format"":{
""type"":""json_object""
}}".Replace("\r\n", ""), result);
        }

        [Fact]
        public async Task DeserializeAsync_JsonString_Returns_GroqRequest()
        {
            // Arrange
            var responseContent = File.ReadAllText("data/response.json");
            var expected = new GroqCompletionsResult
            {
                Id = "34a9110d-c39d-423b-9ab9-9c748747b204",
                Object = "chat.completion",
                Created = 1708045122,
                Model = "mixtral-8x7b-32768",
                SystemFingerprint = "fp_dbffcd8265",
                Choices = new List<GroqChoice>
                {
                    new ()
                    {
                        Index = 0,
                        Message = new GroqMessage(ChatRole.Assistant, "Hello World"),
                        FinishReason = "stop"
                    }
                },
                Usage = new GroqUsage
                {
                    PromptTokens = 24,
                    CompletionTokens = 377,
                    TotalTokens = 401,
                    PromptTime = 0.009M,
                    CompletionTime = 0.774M,
                    TotalTime = 0.783M
                },
                XGroq = new XGroq
                {
                    Id = "req_01htzpsmfmew5b4rbmbjy2kv74"
                }
            };

            // Act
            using var jsonStream = new MemoryStream();
            using var stream = new StreamWriter(jsonStream);
            stream.Write(responseContent);
            stream.Flush();
            jsonStream.Position = 0;
            var actual = await JsonSerializer.DeserializeAsync<GroqCompletionsResult>(jsonStream, GroqService.SerializerOptions);

            // Assert

            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Object, actual.Object);
            Assert.Equal(expected.Created, actual.Created);
            Assert.Equal(expected.Model, actual.Model);
            Assert.Equal(expected.SystemFingerprint, actual.SystemFingerprint);
            Assert.Equal(expected.Choices.Count, actual.Choices.Count);
            Assert.Equal(expected.Choices[0].Index, actual.Choices[0].Index);
            Assert.Equal(expected.Choices[0].FinishReason, actual.Choices[0].FinishReason);
            Assert.Equal(expected.Choices[0].Logprobs, actual.Choices[0].Logprobs);
            Assert.Equal(expected.Choices[0].Message.Role, actual.Choices[0].Message.Role);
            Assert.Equal(expected.Choices[0].Message.Content, actual.Choices[0].Message.Content);
            Assert.Equal(expected.Usage.PromptTokens, actual.Usage.PromptTokens);
            Assert.Equal(expected.Usage.CompletionTokens, actual.Usage.CompletionTokens);
            Assert.Equal(expected.Usage.TotalTokens, actual.Usage.TotalTokens);
            Assert.Equal(expected.Usage.PromptTime, actual.Usage.PromptTime);
            Assert.Equal(expected.Usage.TotalTime, actual.Usage.TotalTime);
            Assert.Equal(expected.XGroq.Id, actual.XGroq.Id);
        }

        [Fact]
        public async Task DeserializeAsyncEnumerable_JsonStream_Returns_GroqRequest()
        {
            // Arrange
            var responseContent = File.ReadAllText("data/response.json");
            var expected = new GroqCompletionsResult
            {
                Id = "34a9110d-c39d-423b-9ab9-9c748747b204",
                Object = "chat.completion",
                Created = 1708045122,
                Model = "mixtral-8x7b-32768",
                SystemFingerprint = "fp_dbffcd8265",
                Choices = new List<GroqChoice>
                {
                    new ()
                    {
                        Index = 0,
                        Message = new GroqMessage(ChatRole.Assistant,"Hello World"),
                        FinishReason = "stop",
                        Logprobs = null
                    }
                },
                Usage = new GroqUsage
                {
                    PromptTokens = 24,
                    CompletionTokens = 377,
                    TotalTokens = 401,
                    PromptTime = 0.009M,
                    CompletionTime = 0.774M,
                    TotalTime = 0.783M
                },
                XGroq = new XGroq
                {
                    Id = "req_01htzpsmfmew5b4rbmbjy2kv74"
                }
            };

            // Act
            using var jsonStream = new MemoryStream();
            using var stream = new StreamWriter(jsonStream);
            stream.Write($"[{responseContent}]");
            stream.Flush();
            jsonStream.Position = 0;

            await foreach (var actual in JsonSerializer.DeserializeAsyncEnumerable<GroqCompletionsResult>(jsonStream, GroqService.SerializerOptions))
            {
                // Assert
                Assert.Equal(expected.Id, actual.Id);
                Assert.Equal(expected.Object, actual.Object);
                Assert.Equal(expected.Created, actual.Created);
                Assert.Equal(expected.Model, actual.Model);
                Assert.Equal(expected.SystemFingerprint, actual.SystemFingerprint);
                Assert.Equal(expected.Choices.Count, actual.Choices.Count);
                Assert.Equal(expected.Choices[0].Index, actual.Choices[0].Index);
                Assert.Equal(expected.Choices[0].FinishReason, actual.Choices[0].FinishReason);
                Assert.Equal(expected.Choices[0].Logprobs, actual.Choices[0].Logprobs);
                Assert.Equal(expected.Choices[0].Message.Role, actual.Choices[0].Message.Role);
                Assert.Equal(expected.Choices[0].Message.Content, actual.Choices[0].Message.Content);
                Assert.Equal(expected.Usage.PromptTokens, actual.Usage.PromptTokens);
                Assert.Equal(expected.Usage.CompletionTokens, actual.Usage.CompletionTokens);
                Assert.Equal(expected.Usage.TotalTokens, actual.Usage.TotalTokens);
                Assert.Equal(expected.Usage.PromptTime, actual.Usage.PromptTime);
                Assert.Equal(expected.Usage.TotalTime, actual.Usage.TotalTime);
                Assert.Equal(expected.XGroq.Id, actual.XGroq.Id);
            }
        }
    }
}
