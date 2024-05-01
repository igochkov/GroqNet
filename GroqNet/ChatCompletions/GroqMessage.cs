namespace GroqNet.ChatCompletions;

public class GroqMessage
{
    /// <summary>
    /// The role of the chat participant.
    /// </summary>
    public GroqChatRole Role { get; set; }

    /// <summary>
    ///  The text of a message.
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// An optional name to disambiguate messages from different users with the same role.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Seed used for sampling.Groq attempts to return the same response to the same request with an identical seed.
    /// </summary>
    public string? Seed { get; set; }

    public GroqMessage() { }

    public GroqMessage(string content)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(content, nameof(content));

        Role = GroqChatRole.User;
        Content = content;        
    }

    public GroqMessage(GroqChatRole role, string content, string? name = null, string? seed = null)
    {
        ArgumentNullException.ThrowIfNull(role, nameof(role));
        ArgumentException.ThrowIfNullOrWhiteSpace(content, nameof(content));

        Role = role;
        Content = content;
        Name = name;
        Seed = seed;
    }
}
