using Newtonsoft.Json;
using Our.Umbraco.Synthscribe.OpenAi.Models;
using System;
using System.Linq;

namespace Our.Umbraco.Synthscribe.OpenAi.JsonConverters.ImageMessage;
internal class ImageMessageJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(ChatGptCompletionImageMessage);

    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        return null;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        ChatGptCompletionImageMessage msg = (ChatGptCompletionImageMessage)value;

        var msgObj = new
        {
            role = msg.Role,
            content = msg.Content.Select(x => x.GetValue())
        };
        var a = JsonConvert.SerializeObject(msgObj);
        writer.WriteRaw($",{a}");
    }
}
