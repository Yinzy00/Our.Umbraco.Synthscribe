using Newtonsoft.Json;
using Our.Umbraco.Synthscribe.OpenAi.Models;
using Our.Umbraco.Synthscribe.OpenAi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Our.Umbraco.Synthscribe.OpenAi.Services
{
    internal class ChatGptService : IChatGptService
    {
        private string _apiKey { get; }

        public ChatGptService(string apiKey)
        {
            //TMP HARDCODED KEY
            _apiKey =  apiKey;
        }

        public async Task<string> CreateCompletion(ChatGptCompletion completion)
        {
            using (HttpClient client = new())
            {
                client.DefaultRequestHeaders.Authorization = new("Bearer", _apiKey);

                var stringContent = JsonConvert.SerializeObject(completion, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore, });
                var content = new StringContent(stringContent, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(Constants.Gpt.CompletionUrl, content);

                var responseData = await response.Content.ReadAsStringAsync();

                var responseObj = JsonConvert.DeserializeObject<ChatGptCompletionResponse>(responseData);

                return responseObj?.choices?.FirstOrDefault()?.message?.content;

            }
        }
    }
}
