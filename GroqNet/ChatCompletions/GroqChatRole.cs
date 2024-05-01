namespace GroqNet.ChatCompletions;

public readonly struct GroqChatRole : IEquatable<GroqChatRole>
{
    private readonly string _value;

    public GroqChatRole(string value)
    {
        _value = value ?? throw new ArgumentNullException(nameof(value));
    }

    private const string SystemValue = "system";
    private const string AssistantValue = "assistant";
    private const string UserValue = "user";

    public static GroqChatRole System { get; } = new GroqChatRole(SystemValue);

    public static GroqChatRole Assistant { get; } = new GroqChatRole(AssistantValue);

    public static GroqChatRole User { get; } = new GroqChatRole(UserValue);


    public static bool operator ==(GroqChatRole left, GroqChatRole right) => left.Equals(right);
    public static bool operator !=(GroqChatRole left, GroqChatRole right) => !left.Equals(right);
    public static implicit operator GroqChatRole(string value) => new GroqChatRole(value);
    public override bool Equals(object obj) => obj is GroqChatRole other && Equals(other);
    public bool Equals(GroqChatRole other) => string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);
    public override int GetHashCode() => _value?.GetHashCode() ?? 0;
    public override string ToString() => _value;
}