using System.Text.Json;
using System.Text.Json.Serialization;

namespace TaskManager_backend.Converters;

public class UtcDateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Obtener el string de la fecha
        string dateString = reader.GetString()!;
        
        // Parsear a DateTime; si no tiene zona horaria, se asume UTC
        DateTime date = DateTime.Parse(dateString, null, System.Globalization.DateTimeStyles.RoundtripKind);
        
        // Si el tipo es Unspecified, lo convertimos a UTC
        if (date.Kind == DateTimeKind.Unspecified)
        {
            date = DateTime.SpecifyKind(date, DateTimeKind.Utc);
        }
        
        return date;
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        // Al serializar, aseguramos que se escriba en UTC con formato ISO 8601 + Z
        writer.WriteStringValue(value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ"));
    }
}