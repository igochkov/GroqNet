namespace GroqNet
{
    public class GroqMessage
    {
        /// <summary>
        /// The role of the chat participant.
        /// </summary>
        public ChatRole Role { get; private set; }

        /// <summary>
        ///  The text of a message.
        /// </summary>
        public string Content { get; private set; }

        /// <summary>
        /// An optional name to disambiguate messages from different users with the same role.
        /// </summary>
        public string? Name { get; private set; }

        /// <summary>
        /// Seed used for sampling.Groq attempts to return the same response to the same request with an identical seed.
        /// </summary>
        public string? Seed { get; private set; }

        public GroqMessage(ChatRole role, string content, string? name = null, string? seed = null)
        {
            Role = role;
            Content = content;
            Name = name;
            Seed = seed;
        }
    }
}
