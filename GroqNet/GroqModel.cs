namespace GroqNet
{
    /// <summary>
    /// Represents pre-trained models provided by Groq with identifiers and additional metadata.
    /// </summary>
    public static class GroqModel
    {
        /// <summary>
        /// Model ID: llama3-8b-8192
        /// Developer: Meta
        /// Context Window: 8,192 tokens
        /// Model Card: https://huggingface.co/meta-llama/Meta-Llama-3-8B
        /// </summary>
        public const string LLaMA3_8b = "llama3-8b-8192";
        /// <summary>
        /// Model ID: llama3-70b-8192
        /// Developer: Meta
        /// Context Window: 8,192 tokens
        /// Model Card: https://huggingface.co/meta-llama/Meta-Llama-3-70B
        /// </summary>
        public const string LLaMA3_70b = "llama3-70b-8192";
        /// <summary>
        /// Model ID: mixtral-8x7b-32768
        /// Developer: Mistral
        /// Context Window: 32,768 tokens
        /// Model Card: https://huggingface.co/mistralai/Mixtral-8x7B-Instruct-v0.1
        /// </summary>
        public const string Mixtral_8x7b = "mixtral-8x7b-32768";
        /// <summary>
        /// Model ID: gemma-7b-it
        /// Developer: Google
        /// Context Window: 8,192 tokens
        /// Model Card: https://huggingface.co/google/gemma-7b-it
        /// </summary>
        public const string Gemma_7b = "gemma-7b-it";

        public static int MaxTokens(string model)
        {
            return model switch
            {
                LLaMA3_8b => 8192,
                LLaMA3_70b => 8192,
                Mixtral_8x7b => 32768,
                Gemma_7b => 8192,
                _ => 8192 // Unknown model -> defaults to 8192
            };
        }
    }
}
