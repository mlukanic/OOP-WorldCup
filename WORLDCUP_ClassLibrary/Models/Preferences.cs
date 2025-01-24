
namespace WORLDCUP_ClassLibrary.Models
{
    public class Preferences
    {
        public string SelectedChampionship { get; set; }
        public string SelectedLanguageCode { get; set; }
        public string FavouriteCountry { get; set; }
        public string FavouriteCountryCode { get; set; }
        public List<Player> FavouritePlayers { get; set; }
        public string WPFResolution { get; set; }
    }
}