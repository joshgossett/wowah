namespace AHWeb.Schema
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using AHWeb.Db;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class AhFiles
    {
        [JsonProperty("files")]
        public List<File> Files { get; set; }
    }

    public partial class File
    {
        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("lastModified")]
        [JsonConverter(typeof(MillisecondEpochConverter))]
        public DateTime LastModified { get; set; }
    }

    public partial class AhFiles
    {
        public static AhFiles FromJson(string json) => JsonConvert.DeserializeObject<AhFiles>(json, Converter.Settings);
    }

    public static class SerializeAhFiles
    {
        public static string ToJson(this AhFiles self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    static class DeserializeAHSnap
    {
        public static AuctionSnap FromJson(string json) => JsonConvert.DeserializeObject<AuctionSnap>(json, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.DateTime,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AdjustToUniversal }
            },
        };
    }

    public class MillisecondEpochConverter : DateTimeConverterBase
    {
        private static readonly DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteRawValue(((DateTime)value - _epoch).TotalMilliseconds + "000");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null) { return null; }
            return _epoch.AddMilliseconds((long)reader.Value);
        }
    }
}
