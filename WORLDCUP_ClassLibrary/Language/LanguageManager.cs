using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WORLDCUP_ClassLibrary.Constants;
using WORLDCUP_ClassLibrary.Interfaces;

namespace WORLDCUP_ClassLibrary.Language
{
    public class LanguageManager : ILanguage
    {
        private static ResourceManager languageManager =
            new ResourceManager(PropertiesConstants.RESOURCE_STREAM, typeof(LanguageManager).Assembly);

        public void SetLanguage(string languageCode)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(languageCode))
                {
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(languageCode);
                }
                else
                {
                    throw new ArgumentException("Language code cannot be null or whitespace.", nameof(languageCode));
                }
            }
            catch (CultureNotFoundException ex)
            {
                Console.Error.WriteLine($"Invalid language code '{languageCode}': {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Console.Error.WriteLine($"Error setting language: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error setting language: {ex.Message}");
            }
        }

        public string GetLanguageString(string key)
        {
            try
            {
                string? value = languageManager.GetString(key);
                if (value == null)
                {
                    throw new MissingManifestResourceException($"The key '{key}' was not found in the resource files.");
                }
                return value;
            }
            catch (MissingManifestResourceException ex)
            {
                Console.Error.WriteLine(ex.Message);
                return $"[Missing: {key}]";
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error retrieving language string for key '{key}': {ex.Message}");
                return $"[Error: {key}]";
            }
        }
    }
}
