using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using WORLDCUP_ClassLibrary.Constants;
using WORLDCUP_ClassLibrary.Interfaces;
using WORLDCUP_ClassLibrary.Language;
using WORLDCUP_ClassLibrary.Utilities;

namespace WORLDCUP_ClassLibrary
{
    public class DataHandler : IDataHandler
    {
        private static DataHandler? instance;
        public static DataHandler dataHandler
        {
            get
            {
                if (instance == null)
                {
                    instance = new DataHandler();
                }
                return instance;
            }
        }
        private DataHandler() { }

        public event Action<string> OnError;

        private readonly LanguageManager languageManager = new LanguageManager();

        public void UpdateRetrievingMethod(string newMethod)
        {
            if (GetDataRetrievingMethod() != newMethod)
            {
                try
                {
                    if (!File.Exists(PropertiesConstants.JSON_FILE_PATH))
                    {
                        File.Create(PropertiesConstants.JSON_FILE_PATH).Dispose();
                    }

                    var json = File.ReadAllText(PropertiesConstants.JSON_FILE_PATH);
                    var jsonObj = JObject.Parse(json);

                    jsonObj[PropertiesConstants.METHOD] = newMethod;
                    File.WriteAllText(PropertiesConstants.JSON_FILE_PATH, jsonObj.ToString());
                }
                catch (Exception e)
                {
                    OnError?.Invoke(e.Message);
                }
            }
        }

        public string? GetDataRetrievingMethod()
        {
            try
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(PropertiesConstants.DATA_FOLDER_PATH)
                    .AddJsonFile(PropertiesConstants.PREFERENCES_PATH, optional: false, reloadOnChange: true)
                    .Build();

                var method = configuration[PropertiesConstants.METHOD];
                return method;
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex.Message);
                return null;
            }
        }

        public Stream GetResourceStream(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            return assembly.GetManifestResourceStream(resourceName) ?? Stream.Null;
        }

        public Icon GetAppIcon(Stream imageStream)
        {
            if (imageStream == null)
                throw new ArgumentNullException(nameof(imageStream));

            using (var image = Image.FromStream(imageStream))
            {
                using (Bitmap bitmap = new Bitmap(image))
                {
                    IntPtr hIcon = bitmap.GetHicon();
                    Icon icon = Icon.FromHandle(hIcon);

                    bitmap.Dispose();

                    return icon;
                }
            }
        }

        public void SetLanguage(string languageCode)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(languageCode))
                {
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(languageCode);
                }
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex.Message);
            }
        }

        public string GetLanguageString(string key)
        {
            try
            {
                return languageManager.GetLanguageString(key);
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Error retrieving string for key '{key}': {ex.Message}");
                return $"[Missing: {key}]";
            }
        }

        public string ToTitleCase(string text)
        {
            return TextUtilities.ToTitleCase(text);
        }
    }
}
