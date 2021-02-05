using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace YogoServer.StartupManager
{
    public static class JsonConfiguration
    {
        private static readonly JsonSerializerOptions jsonSerializerOptions;
        private static readonly TextEncoderSettings textEncoderSettings;

        static JsonConfiguration()
        {
            textEncoderSettings = new TextEncoderSettings();
            textEncoderSettings.AllowRanges(UnicodeRanges.BasicLatin, UnicodeRanges.Latin1Supplement);

            jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
                IgnoreNullValues = false,
                WriteIndented = false,
                Encoder = JavaScriptEncoder.Create(textEncoderSettings)

            };
        }

        public static JsonSerializerOptions Get()
        {
            return jsonSerializerOptions;
        }
    }
}
