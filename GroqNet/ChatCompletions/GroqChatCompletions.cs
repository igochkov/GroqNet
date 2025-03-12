namespace GroqNet.ChatCompletions;

public class GroqChatCompletions
{
    public required string Id { get; set; }

    public required string Object { get; set; }

    public required long Created { get; set; }

    public required string Model { get; set; }

    public required string SystemFingerprint { get; set; }

    public required IReadOnlyList<GroqChoice> Choices { get; set; }

    public required GroqUsage Usage { get; set; }
}