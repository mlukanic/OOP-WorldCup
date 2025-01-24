using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WORLDCUP_ClassLibrary.Constants
{
    public class PropertiesConstants
    {
        public const string RESOURCE_STREAM = "WORLDCUP_ClassLibrary.Resources.Language.Strings";
        public const string METHOD = "method";
        public const string PREFERENCES_PATH = "Preferences/appsettings.json";
        public const string METHOD_LOCAL = "local";
        public const string SEPARATOR = "-";
        public const string GOAL = "goal";
        public const string YELLOW_CARD = "yellow-card";
        public static readonly string DATA_FOLDER_PATH = Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\WORLDCUP_ClassLibrary", @"Data");
        public static readonly string JSON_FILE_PATH = Path.Combine(
            Path.GetFullPath(
                Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\WORLDCUP_ClassLibrary\Data")),
                "Preferences",
                "appsettings.json"
        );
        public static readonly string DATA_FOLDER = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\DAL\Data\"));

    }
}
