using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Utilities
{
    public static class SerializerJson
    {

        public static string SerializeObjectToJsonString(object input)
        {
            var serializeOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters =
                {
                    new DateTimeConverter()
                },
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                IgnoreNullValues = true

            };
            var jsonString = JsonSerializer.Serialize(input, serializeOptions);

            return jsonString;
        }

        public static T DeserialiseJsonStringToObject<T>(string jsonString)
        {
            var res = JsonSerializer.Deserialize<T>(jsonString);

            return res;
        }


        /*
        public class DateTimeOffsetConverter : JsonConverter<DateTimeOffset>
        {
            public override DateTimeOffset Read(
                ref Utf8JsonReader reader,
                Type typeToConvert,
                JsonSerializerOptions options) =>
                DateTimeOffset.ParseExact(reader.GetString(),
                    "MM/dd/yyyy", CultureInfo.InvariantCulture);

            public override void Write(
                Utf8JsonWriter writer,
                DateTimeOffset dateTimeValue,
                JsonSerializerOptions options) =>
                writer.WriteStringValue(dateTimeValue.ToString(
                    "MM/dd/yyyy", CultureInfo.InvariantCulture));
        }*/

        public class DateTimeConverter : JsonConverter<DateTime>
        {
            public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return DateTime.Parse(reader.GetString());
            }

            public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
            {
                //writer.WriteStringValue(value.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssZ"));
                writer.WriteStringValue(value.ToUniversalTime().ToString("yyyy'-'MM'-'dd"));
            }
        }




    }

}

