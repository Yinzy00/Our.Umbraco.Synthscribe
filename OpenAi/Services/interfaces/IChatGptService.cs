using Our.Umbraco.Synthscribe.OpenAi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Our.Umbraco.Synthscribe.OpenAi.Services.Interfaces
{
    internal interface IChatGptService
    {

        Task<string> CreateCompletion(ChatGptCompletion completion);

    }
}
