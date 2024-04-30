using System.Text.Json;

namespace GroqNet
{
    public class GroqModelNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            switch (name)
            {
                case "LLaMA3_8b":
                    return "llama3-8b-8192";
                case "LLaMA3_70b":
                    return "llama3-70b-8192";
                case "LLaMA2_70b":
                    return "llama2-70b-4096";
                case "Mixtral_8x7b":
                    return "mixtral-8x7b-32768";
                case "Gemma_7b":
                    return "gemma-7b-it";
                default:
                    return name;
            }
        }
    }
}
