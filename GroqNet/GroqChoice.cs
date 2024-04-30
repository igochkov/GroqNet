namespace GroqNet
{
    public class GroqChoice
    {
        public int Index { get; set; }

        public GroqMessage Message { get; set; }

        public string FinishReason { get; set; }

        /// <summary>
        /// Null in the provided documentation, datatype can be adjusted if future use cases require
        /// </summary>
        public object? Logprobs { get; set; } 
    }
}