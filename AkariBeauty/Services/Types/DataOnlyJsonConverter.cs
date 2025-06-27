using System.Text.Json;
using System.Text.Json.Serialization;

namespace AkariBeauty.Services.Types;

public class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    private const string DateFormat = "yyyy-MM-dd";

    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var stringValue = reader.GetString();
            if (DateOnly.TryParseExact(stringValue, DateFormat, out var result))
            {
                return result;
            }
        }
        
        throw new JsonException($"Não foi possível converter para DateOnly: {reader.GetString()}");
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(DateFormat));
    }
}
