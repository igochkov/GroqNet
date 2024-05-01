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

    private const string LLaMA3_8bValue = "llama3-8b-8192";
    private const string LLaMA3_70bValue = "llama3-70b-8192";
    private const string Mixtral_8x7bValue = "mixtral-8x7b-32768";
    private const string Gemma_7bValue = "gemma-7b-it";

    /// <summary>
    /// Model ID: llama3-8b-8192
    /// Developer: Meta
    /// Context Window: 8,192 tokens
    /// Model Card: https://huggingface.co/meta-llama/Meta-Llama-3-8B
    /// </summary>
    public static GroqModel LLaMA3_8b { get; } = new GroqModel(LLaMA3_8bValue);
    /// <summary>
    /// Model ID: llama3-70b-8192
    /// Developer: Meta
    /// Context Window: 8,192 tokens
    /// Model Card: https://huggingface.co/meta-llama/Meta-Llama-3-70B
    /// </summary>
    public static GroqModel LLaMA3_70b { get; } = new GroqModel(LLaMA3_70bValue);
    /// <summary>
    /// Model ID: mixtral-8x7b-32768
    /// Developer: Mistral
    /// Context Window: 32,768 tokens
    /// Model Card: https://huggingface.co/mistralai/Mixtral-8x7B-Instruct-v0.1
    /// </summary>
    public static GroqModel Mixtral_8x7b { get; } = new GroqModel(Mixtral_8x7bValue);
    /// <summary>
    /// Model ID: gemma-7b-it
    /// Developer: Google
    /// Context Window: 8,192 tokens
    /// Model Card: https://huggingface.co/google/gemma-7b-it
    /// </summary>
    public static GroqModel Gemma_7b { get; } = new GroqModel(Gemma_7bValue);

    public static int MaxTokens(GroqModel model)
    {
        return model._value switch
        {
            LLaMA3_8bValue => 8192,
            LLaMA3_70bValue => 8192,
            Mixtral_8x7bValue => 32768,
            Gemma_7bValue => 8192,
            _ => 8192 // Unknown model -> defaults to 8192
        };
    }

    public bool Equals(GroqModel other) => string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);

    public override string ToString() => _value;

    public string Value { get { return _value; } }
}
