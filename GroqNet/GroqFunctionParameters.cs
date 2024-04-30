namespace GroqNet
{
    public class GroqFunctionParameters
    {
        public string Type { get; set; }

        public IDictionary<string, GroqFunctionParameterDetail> Properties { get; set; }

        public IList<string> Required { get; set; }
    }
}
