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
                case "grok-2-latest":
                    return GroqModel.Grok2_latest;
                case "grok-2-vision-latest":
                    return GroqModel.Grok2_vision_latest;
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
