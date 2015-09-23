using Newtonsoft.Json;
using System;

namespace SoftwareKobo.UniversalToolkit.Converters.Json
{
    public class UnixTimestampConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string) || objectType == typeof(int) || objectType == typeof(long);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            long unixTimestamp = 0;
            if (reader.Value is string)
            {
                if (long.TryParse((string)reader.Value, out unixTimestamp) == false)
                {
                    throw new JsonException(string.Format("could not convert unix timestamp", reader.Value));
                }
            }
            else if (reader.Value is int)
            {
                unixTimestamp = (int)reader.Value;
            }
            else if (reader.Value is long)
            {
                unixTimestamp = (long)reader.Value;
            }

            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTimestamp).ToLocalTime();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}