namespace GroqNet.ChatCompletions;

public class GroqChatCompletionOptions
{
    public readonly decimal Temperature;

    public readonly int MaxTokens;

    public readonly decimal TopP;

    public readonly string? Stop;

    public readonly GroqResponseFormat? ResponseFormat;

    public readonly IList<GroqTool>? Tools;

    public GroqChatCompletionOptions()
    {
        Temperature = 1.0m;
        MaxTokens = 1024;
        TopP = 1.0m;
    }

    public GroqChatCompletionOptions(decimal temperature, int maxTokens, decimal topP, string? stop = null, GroqResponseFormat? format = null, IList<GroqTool>? tools = null)
    {
        if (0 > temperature || temperature > 2)
        {
            throw new ArgumentOutOfRangeException(nameof(temperature), "Temperature must be between 0 and 2.");
        }

        //if (0 > maxTokens || maxTokens > GroqModel.MaxTokens(model))
        //{
        //    throw new ArgumentOutOfRangeException(nameof(maxTokens), $"Max tokens must be between 0 and {GroqModel.MaxTokens(model)}.");
        //}

        if (0 > topP || topP > 2)
        {
            throw new ArgumentOutOfRangeException(nameof(topP), "Top-P must be between 0 and 2.");
        }

        Temperature = temperature;
        MaxTokens = maxTokens;
        TopP = topP;
        Stop = stop;
        ResponseFormat = format;
        Tools = tools;
    }
}