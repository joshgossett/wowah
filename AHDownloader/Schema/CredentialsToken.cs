using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AHDownloader.Schema
{


    public partial class CredentialsToken
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }
    }

    public partial class CredentialsToken
    {
        public static CredentialsToken FromJson(string json) => JsonConvert.DeserializeObject<CredentialsToken>(json, Converter.Settings);
    }

    public static class SerializeCredentialsToken
    {
        public static string ToJson(this CredentialsToken self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }
}
