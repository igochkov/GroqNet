namespace GroqNet.ChatCompletions;

public class StreamingChoice
{
    public required int Index { get; set; }

    public required StreamingDelta Delta { get; set; }

    public object? Logprobs { get; set; }
    
    public string? FinishReason { get; set; }
}