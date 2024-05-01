namespace GroqNet.ChatCompletions;

public class GroqChatCompletionsRequest
{
    public required GroqModel Model { get; set; }

    public required IList<GroqMessage> Messages { get; set; }

    public decimal Temperature { get; set; }

    public int MaxTokens { get; set; }

    public decimal TopP { get; set; }

    public bool Stream { get; set; } = false;

    public string? Stop { get; set; }

    public GroqResponseFormat? ResponseFormat { get; set; }

    public IList<GroqTool>? Tools { get; set; }

    public GroqToolChoice? ToolChoice { get; set; }
}
