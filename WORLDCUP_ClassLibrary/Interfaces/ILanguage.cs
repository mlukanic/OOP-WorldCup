using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WORLDCUP_ClassLibrary.Interfaces
{
    public interface ILanguage
    {
        void SetLanguage(string languageCode);
        string GetLanguageString(string key);
    }
}
