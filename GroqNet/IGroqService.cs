namespace GroqNet
{
    public interface IGroqService
    {
        Task<GroqCompletionsResult> GetChatCompletionAsync(IList<GroqMessage> messages, GroqChatCompletionOptions? options = default, CancellationToken cancellationToken = default);
    }
}
