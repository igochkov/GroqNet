namespace GroqNet.ChatCompletions;

/// <summary>
/// Represents an x-Groq identifier object which is typically used to track or correlate requests.
/// </summary>
public class StreamingXGroq
{
    /// <summary>
    /// Gets or sets the unique identifier for a specific request or response.
    /// This ID can be used for logging, debugging, or correlating with server-side information.
    /// </summary>
    public required string Id { get; set; }

    public StreamingUsage? Usage { get; set; }
}