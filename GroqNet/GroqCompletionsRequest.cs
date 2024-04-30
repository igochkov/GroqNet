namespace GroqNet
{
    public class GroqCompletionsRequest
    {
        public string Model { get; set; }

        public IList<GroqMessage> Messages { get; set; }
        
        public decimal Temperature { get; set; }
        
        public int MaxTokens { get; set; }
        
        public decimal TopP { get; set; }

        public bool Stream { get; set; } = false;

        public string? Stop { get; set; }

        public GroqResponseFormat? ResponseFormat { get; set; }

        public IList<GroqTool>? Tools { get; set; }
    }
}
