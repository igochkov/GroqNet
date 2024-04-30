namespace GroqNet
{
    /// <summary>
    /// Encapsulates information about the API rate limit.
    /// </summary>
    public class RateLimitInfo
    {
        /// <summary>
        /// Maximum number of allowed requests in the current time window.
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// Number of requests remaining before hitting the rate limit in the current time window.
        /// </summary>
        public int Remaining { get; set; }

        /// <summary>
        /// Time remaining until the rate limit resets.
        /// </summary>
        public TimeSpan Reset { get; set; }
    }
}
