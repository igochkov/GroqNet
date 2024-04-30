namespace GroqNet
{
    public class GroqCompletionsResult
    {
        public string Id { get; set; }

        public string Object { get; set; }

        public long Created { get; set; }

        public string Model { get; set; }

        public string SystemFingerprint { get; set; }

        public IReadOnlyList<GroqChoice> Choices { get; set; }

        public GroqUsage Usage { get; set; }

        public XGroq XGroq { get; set; }
    }
}