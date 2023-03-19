using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;

namespace Our.Umbraco.Synthscribe.Features.Translation.Content.Service
{
    public interface ITranslateContentService
    {
        Task TranslateContent(int id, string destinationLanguage = null, bool overwrite = false, bool translateDescendants = false);
        Task TranslateAllContent(string destinationLanguage = null, bool overwrite = false);
    }
}
