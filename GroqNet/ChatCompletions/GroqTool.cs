namespace GroqNet.ChatCompletions;

public class GroqTool
{
    public required string Type { get; set; }

    public required GroqFunction Function { get; set; }
}