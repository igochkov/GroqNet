namespace GroqNet.ChatCompletions;

public class StreamingDelta
{
    public GroqChatRole? Role { get; set; }

    public string? Content { get; set; }
}
