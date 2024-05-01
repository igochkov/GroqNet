namespace GroqNet.ChatCompletions;

public class GroqChoice
{
    public required int Index { get; set; }

    public required GroqMessage Message { get; set; }

    public required string FinishReason { get; set; }

    public object? Logprobs { get; set; }
}