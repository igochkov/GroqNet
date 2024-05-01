using GroqNet.ChatCompletions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GroqNet.Serialization
{
    public class JsonGroqChatRoleConverter : JsonConverter<GroqChatRole>
    {
        public override GroqChatRole Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var role = reader.GetString();

            switch (role)
            {
                case "user":
                    return GroqChatRole.User;
                case "assistant":
                    return GroqChatRole.Assistant;
                case "system":
                    return GroqChatRole.System;
                default:
                    throw new JsonException($"Unknown chat role: '{role}'");
            }
        }

        public override void Write(Utf8JsonWriter writer, GroqChatRole role, JsonSerializerOptions options)
        {
            writer.WriteStringValue(role.ToString());
        }
    }
}
