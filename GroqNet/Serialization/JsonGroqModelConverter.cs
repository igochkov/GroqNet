using GroqNet.ChatCompletions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GroqNet.Serialization
{
    public class JsonGroqModelConverter : JsonConverter<GroqModel>
    {
        public override GroqModel Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var model = reader.GetString();

            switch (model)
            {
                case "llama3-8b-8192":
                    return GroqModel.LLaMA3_8b;
                case "llama3-70b-8192":
                    return GroqModel.LLaMA3_70b;
                case "mixtral-8x7b-32768":
                    return GroqModel.Mixtral_8x7b;
                case "gemma-7b-it":
                    return GroqModel.Gemma_7b;
                default:
                    throw new JsonException($"Unknown model: '{model}'");
            }
        }

        public override void Write(Utf8JsonWriter writer, GroqModel model, JsonSerializerOptions options)
        {
            writer.WriteStringValue(model.ToString());
        }
    }
}
