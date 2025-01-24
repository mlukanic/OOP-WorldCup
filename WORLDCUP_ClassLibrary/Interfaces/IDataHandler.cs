using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WORLDCUP_ClassLibrary.Interfaces
{
    public interface IDataHandler
    {
        void UpdateRetrievingMethod(string newMethod);
        string? GetDataRetrievingMethod();
        Stream GetResourceStream(string resourceName);
        Icon GetAppIcon(Stream imageStream);
        void SetLanguage(string languageCode);
        string GetLanguageString(string key);
        string ToTitleCase(string text);
    }
}
