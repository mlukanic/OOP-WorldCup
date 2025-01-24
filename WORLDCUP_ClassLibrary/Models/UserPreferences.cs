using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WORLDCUP_ClassLibrary.Language;

namespace WORLDCUP_ClassLibrary.Models
{
    public static class UserPreferences
    {
        private const string SEPARATOR = "-";
        public static string SelectedChampionship { get; set; }
        public static string SelectedLanguageCode { get; set; }
        public static string FavouriteCountry { get; set; }
        public static string FavouriteCountryCode { get; set; }
        public static List<Player> FavouritePlayers { get; set; } = new List<Player>();
        public static string WPFResolution { get; set; }
        public static string FavouriteCountryOpponentCode { get; set; }

        private static readonly string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\DAL\Data\Preferences");
        private static readonly string filePath = Path.Combine(folderPath, "Preferences.json");

        private static readonly LanguageManager languageManager = new LanguageManager();

        public static event Action<string> OnError;

        /// <summary>
        /// Saves the selected preferences into a json file.
        /// </summary>
        public static void SavePreferences()
        {
            try
            {
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                if (SelectedChampionship != null && SelectedLanguageCode != null)
                {
                    var preferences = new Preferences
                    {
                        SelectedChampionship = SelectedChampionship,
                        SelectedLanguageCode = SelectedLanguageCode,
                        FavouriteCountry = FavouriteCountry,
                        FavouriteCountryCode = FavouriteCountryCode,
                        FavouritePlayers = FavouritePlayers,
                        WPFResolution = WPFResolution
                    };

                    string json = JsonConvert.SerializeObject(preferences, Formatting.Indented);

                    File.WriteAllText(filePath, json);
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                OnError?.Invoke("Error: You do not have permission to write to this location.");
            }
            catch (IOException ex)
            {
                OnError?.Invoke("An error occurred while trying to save preferences.");
            }
            catch (Exception ex)
            {
                OnError?.Invoke("An unexpected error occurred.");
            }
        }

        /// <summary>
        /// Loads the preferences from the saved json file, if there are any.
        /// </summary>
        /// <returns></returns>
        public static bool LoadPreferences()
        {
            if (File.Exists(filePath))
            {
                try
                {
                    string json = File.ReadAllText(filePath);
                    var preferences = JsonConvert.DeserializeObject<Preferences>(json);

                    if (preferences != null)
                    {
                        SelectedChampionship = preferences.SelectedChampionship;
                        SelectedLanguageCode = preferences.SelectedLanguageCode;
                        FavouriteCountry = preferences.FavouriteCountry;
                        FavouriteCountryCode = preferences.FavouriteCountryCode;
                        FavouritePlayers = preferences.FavouritePlayers;
                        WPFResolution = preferences.WPFResolution;
                        languageManager.SetLanguage(SelectedLanguageCode);
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    OnError?.Invoke($"The following error occurred: {ex.Message}");
                }
            }
            return false;
        }

        /// <summary>
        /// Returns the first word from the saved championship name.
        /// </summary>
        /// <returns></returns>
        public static string GetSelectedChampionshipGender()
        {
            return SelectedChampionship.Split(SEPARATOR)[0].Trim();
        }
    }
}
