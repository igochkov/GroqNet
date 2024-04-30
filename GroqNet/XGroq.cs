namespace GroqNet
{
    /// <summary>
    /// Represents an x-Groq identifier object which is typically used to track or correlate requests.
    /// </summary>
    public class XGroq
    {
        /// <summary>
        /// Gets or sets the unique identifier for a specific request or response.
        /// This ID can be used for logging, debugging, or correlating with server-side information.
        /// </summary>
        public string Id { get; set; }
    }
}