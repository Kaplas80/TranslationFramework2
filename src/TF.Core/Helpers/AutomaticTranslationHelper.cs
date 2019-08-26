using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TF.Core.Helpers
{
    public class AutomaticTranslationHelper
    {
        private class TranslationResult
        {
            public DetectedLanguage DetectedLanguage { get; set; }
            public TextResult SourceText { get; set; }
            public Translation[] Translations { get; set; }
        }

        private class DetectedLanguage
        {
            public string Language { get; set; }
            public float Score { get; set; }
        }

        private class TextResult
        {
            public string Text { get; set; }
            public string Script { get; set; }
        }

        private class Translation
        {
            public string Text { get; set; }
            public TextResult Transliteration { get; set; }
            public string To { get; set; }
            public Alignment Alignment { get; set; }
            public SentenceLength SentLen { get; set; }
        }

        private class Alignment
        {
            public string Proj { get; set; }
        }

        private class SentenceLength
        {
            public int[] SrcSentLen { get; set; }
            public int[] TransSentLen { get; set; }
        }

        private static async Task<TranslationResult[]> TranslateTextRequest(string subscriptionKey, string endpoint, string route, string inputText)
        {
            var body = new object[] { new { Text = inputText } };
            var requestBody = JsonConvert.SerializeObject(body);

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                // Build the request.
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(endpoint + route);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

                // Send the request and get response.
                var response = await client.SendAsync(request).ConfigureAwait(false);
                // Read response as a string.
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TranslationResult[]>(result);
            }
        }

        public static string Translate(string text)
        {
            if (!File.Exists("MicrosoftTranslator.txt"))
            {
                throw new FileNotFoundException("No se ha encontrado el fichero de configuración para la traducción automática.");
            }

            var lines = File.ReadAllLines("MicrosoftTranslator.txt");

            const string route = "/translate?api-version=3.0&to=es";

            var results = TranslateTextRequest(lines[0], lines[1], route, text);
            results.Wait();

            return results.Result[0].Translations[0].Text;
        }

    }
}
