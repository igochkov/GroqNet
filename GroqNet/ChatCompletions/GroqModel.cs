namespace GroqNet.ChatCompletions;

/// <summary>
/// Represents pre-trained models provided by Groq with identifiers and additional metadata.
/// </summary>
public readonly struct GroqModel : IEquatable<GroqModel>
{
    private readonly string _value;

    public GroqModel(string value)
    {
        _value = value ?? throw new ArgumentNullException(nameof(value));
    }

    private const string Grok_2_latest = "grok-2-latest";
    private const string Grok_2_vision_latest = "grok-2-vision-latest";

    /// <summary>
    /// Model ID: grok-2-latest
    /// </summary>
    public static GroqModel Grok2_latest { get; } = new GroqModel(Grok_2_latest);

    /// <summary>
    /// Model ID: grok-2-vision-latest
    /// </summary>
    public static GroqModel Grok2_vision_latest { get; } = new GroqModel(Grok_2_vision_latest);

    public static int MaxTokens(GroqModel model)
    {
        return model._value switch
        {
            Grok_2_latest => 131072,
            Grok_2_vision_latest => 32768,
            _ => 8192 // Unknown model -> defaults to 8192
        };
    }

    public bool Equals(GroqModel other) => string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);

    public override string ToString() => _value;

    public string Value { get { return _value; } }
}
