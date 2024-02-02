using Newtonsoft.Json;
using Our.Umbraco.Synthscribe.OpenAi.JsonConverters.ImageMessage;
using System.Collections.Generic;

namespace Our.Umbraco.Synthscribe.OpenAi.Models
{
    [JsonConverter(typeof(ImageMessageJsonConverter))]
    internal sealed class ChatGptCompletionImageMessage : ChatGptCompletionMessage
    {
        [JsonProperty("content")]
        public List<ChatGptCompletionImageMessageItem> Content { get; set; } = new();
    }

    internal sealed class ChatGptCompletionImageMessageItem
    {
        public ImageMessageType Type { get; init; }
        public string Value { get; init; }

        public object GetValue()
        {
            if (Type == ImageMessageType.text)
            {
                return JsonConvert.SerializeObject(new
                {
                    type = Type.ToString(),
                    text = Value
                });
            }
            else if (Type == ImageMessageType.image_url)
            {
                return JsonConvert.SerializeObject(new
                {
                    type = Type.ToString(),
                    image_url = new
                    {
                        url= Value
                    }
                });
            }

            return string.Empty;
        }
    }

    internal enum ImageMessageType
    {
        text,
        image_url
    }

}
