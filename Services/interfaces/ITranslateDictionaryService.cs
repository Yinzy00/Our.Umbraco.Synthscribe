using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Our.Umbraco.Synthscribe.Services.interfaces
{
    public interface ITranslateDictionaryService
    {
        Task TranslateAllDictionaries(string destinationLanguage = null, bool overwrite = false);
        Task TranslateDictionary(int id, string destinationLanguage = null, bool overwrite = false, bool translateDescendants = false);
    }
}
