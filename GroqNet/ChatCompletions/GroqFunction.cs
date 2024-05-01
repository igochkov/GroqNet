namespace GroqNet.ChatCompletions;

public class GroqFunction
{
    public required string Name { get; set; }

    public required string Description { get; set; }

    public BinaryData? Parameters { get; set; }
}
