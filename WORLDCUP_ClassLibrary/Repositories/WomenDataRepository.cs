using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WORLDCUP_ClassLibrary.Constants;
using WORLDCUP_ClassLibrary.Factories;
using WORLDCUP_ClassLibrary.Interfaces;
using WORLDCUP_ClassLibrary.Models;
using WORLDCUP_ClassLibrary.Services;
using WORLDCUP_ClassLibrary.Utilities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WORLDCUP_ClassLibrary.Repositories
{
    public class WomenDataRepository : IWomenData
    {
        private readonly IDataHandler _dataHandler;
        private readonly HttpService _httpService;
        private readonly FileService _fileService;

        public WomenDataRepository(IDataHandler dataHandler)
        {
            _dataHandler = dataHandler;
            _httpService = ServiceFactory.GetHttpService();
            _fileService = ServiceFactory.GetFileService();
        }

        public event Action<string> OnError;
        public async Task<List<CountryStats>?> GetAllCountryStatsAsyncWomen()
        {
            try
            {
                string? json;
                if (_dataHandler.GetDataRetrievingMethod() == PropertiesConstants.METHOD_LOCAL)
                {
                    string fileDirectory = Path.Combine(PropertiesConstants.DATA_FOLDER_PATH, WomenConstants.WOMEN_MATCHES_PATH);
                    json = await _fileService.ReadFileAsync(fileDirectory);
                }
                else
                {
                    json = await _httpService.GetJsonAsync(WomenConstants.WOMEN_MATCHES_URI);
                }

                if (json != null)
                {
                    return JsonConvert.DeserializeObject<List<CountryStats>>(json);
                }
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex.Message);
            }
            return null;
        }

        public async Task<List<GroupDetails>> GetAllGroupResultsAsyncWomen()
        {
            try
            {
                string? json;
                if (_dataHandler.GetDataRetrievingMethod() == PropertiesConstants.METHOD_LOCAL)
                {
                    string fileDirectory = Path.Combine(PropertiesConstants.DATA_FOLDER_PATH, WomenConstants.WOMEN_GROUP_RESULTS_PATH);
                    json = await _fileService.ReadFileAsync(fileDirectory);
                }
                else
                {
                    json = await _httpService.GetJsonAsync(WomenConstants.WOMEN_GROUP_RESULTS_URI);
                }

                if (json != null)
                {
                    return JsonConvert.DeserializeObject<List<GroupDetails>>(json);
                }
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex.Message);
            }
            return null;
        }

        public async Task<List<Matches>?> GetAllMatchResultsAsyncWomen()
        {
            try
            {
                string? json;
                if (_dataHandler.GetDataRetrievingMethod() == PropertiesConstants.METHOD_LOCAL)
                {
                    string fileDirectory = Path.Combine(PropertiesConstants.DATA_FOLDER_PATH, WomenConstants.WOMEN_MATCHES_PATH);
                    json = await _fileService.ReadFileAsync(fileDirectory);
                }
                else
                {
                    json = await _httpService.GetJsonAsync(WomenConstants.WOMEN_MATCHES_URI);
                }

                if (json != null)
                {
                    return JsonConvert.DeserializeObject<List<Matches>>(json);
                }
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex.Message);
            }
            return null;
        }

        public async Task<List<Team>> GetAllTeamStatsAsyncWomen()
        {
            try
            {
                Loading.LoadingScreen?.Show();

                string? json;
                if (_dataHandler.GetDataRetrievingMethod() == PropertiesConstants.METHOD_LOCAL)
                {
                    string fileDirectory = Path.Combine(PropertiesConstants.DATA_FOLDER_PATH, WomenConstants.WOMEN_TEAMS_PATH);
                    json = await _fileService.ReadFileAsync(fileDirectory);
                }
                else
                {
                    json = await _httpService.GetJsonAsync(WomenConstants.WOMEN_TEAMS_URI);
                }

                if (json != null)
                {
                    return JsonConvert.DeserializeObject<List<Team>>(json);
                }
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex.Message);
            }
            finally
            {
                Loading.LoadingScreen?.Hide();
            }
            return null;
        }

        public async Task<List<Player>> GetFavoriteTeamPlayersWomen()
        {
            List<Matches> matches = new List<Matches>();
            List<Player> allPlayers = new List<Player>();

            try
            {
                Loading.LoadingScreen?.Show();

                matches = await GetSpecificCountryMatchResultsAsyncWomen(UserPreferences.FavouriteCountryCode.ToString());

                if (matches[0].HomeTeam.Code == UserPreferences.FavouriteCountryCode.ToString())
                {
                    var startingEleven = matches.First().HomeTeamStatistics.StartingEleven.ToList();
                    var substitutes = matches.First().HomeTeamStatistics.Substitutes.ToList();
                    allPlayers = startingEleven.Concat(substitutes).ToList();
                    allPlayers.Sort();
                }
                else
                {
                    var startingEleven = matches.First().AwayTeamStatistics.StartingEleven.ToList();
                    var substitutes = matches.First().AwayTeamStatistics.Substitutes.ToList();
                    allPlayers = startingEleven.Concat(substitutes).ToList();
                    allPlayers.Sort();
                }

                if (UserPreferences.FavouriteCountryCode == "EGY")
                {
                    allPlayers.Find(p => p.Name == "TREZIGUET").Name = "TREZEGUET";
                }
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex.Message);
            }
            finally
            {
                Loading.LoadingScreen?.Hide();
            }

            return allPlayers;
        }

        public async Task<List<Player>> GetGoalsAndYellowCardsAsyncWomen(string countryCode, int specificMatch = -1)
        {
            var matchResults = await GetSpecificCountryMatchResultsAsyncWomen(countryCode);
            List<Player> players = new List<Player>();

            if (specificMatch == -1)
            {
                try
                {
                    Loading.LoadingScreen?.Show();

                    if (matchResults[0].HomeTeam.Code == countryCode)
                    {
                        foreach (var player in matchResults[0].HomeTeamStatistics.StartingEleven)
                        {
                            players.Add(player);
                        }
                        foreach (var substitute in matchResults[0].HomeTeamStatistics.Substitutes)
                        {
                            players.Add(substitute);
                        }
                    }
                    else
                    {
                        foreach (var player in matchResults[0].AwayTeamStatistics.StartingEleven)
                        {
                            players.Add(player);
                        }
                        foreach (var substitute in matchResults[0].AwayTeamStatistics.Substitutes)
                        {
                            players.Add(substitute);
                        }
                    }

                    players.Sort();

                    foreach (var match in matchResults)
                    {
                        if (match.HomeTeam.Code == countryCode)
                        {
                            foreach (var homeTeamEvent in match.HomeTeamEvents)
                            {
                                if (homeTeamEvent.TypeOfEvent.Split(PropertiesConstants.SEPARATOR)[0] == PropertiesConstants.GOAL)
                                {
                                    players.Find(p => p.Name == homeTeamEvent.Player).GoalsScored++;
                                }
                                else if (homeTeamEvent.TypeOfEvent == PropertiesConstants.YELLOW_CARD)
                                {
                                    players.Find(p => p.Name == homeTeamEvent.Player).YellowCardsReceived++;
                                }
                            }
                        }
                        else
                        {
                            foreach (var awayTeamEvent in match.AwayTeamEvents)
                            {
                                if (awayTeamEvent.TypeOfEvent.Split(PropertiesConstants.SEPARATOR)[0] == PropertiesConstants.GOAL)
                                {
                                    players.Find(p => p.Name == awayTeamEvent.Player).GoalsScored++;
                                }
                                else if (awayTeamEvent.TypeOfEvent == PropertiesConstants.YELLOW_CARD)
                                {
                                    players.Find(p => p.Name == awayTeamEvent.Player).YellowCardsReceived++;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    OnError?.Invoke(ex.Message);
                }
                finally
                {
                    Loading.LoadingScreen?.Hide();
                }
            }
            else if (specificMatch < matchResults.Count && specificMatch >= 0)
            {
                try
                {
                    Loading.LoadingScreen?.Show();

                    var match = matchResults[specificMatch];

                    //HOME TEAM
                    foreach (var player in matchResults[specificMatch].HomeTeamStatistics.StartingEleven)
                    {
                        players.Add(player);
                    }
                    foreach (var substitute in matchResults[specificMatch].HomeTeamStatistics.Substitutes)
                    {
                        players.Add(substitute);
                    }
                    //HOME TEAM EVENTS
                    foreach (var homeTeamEvent in match.HomeTeamEvents)
                    {
                        if (homeTeamEvent.TypeOfEvent.Split(PropertiesConstants.SEPARATOR)[0] == PropertiesConstants.GOAL)
                        {
                            players.Find(p => p.Name == homeTeamEvent.Player).GoalsScored++;
                        }
                        else if (homeTeamEvent.TypeOfEvent == PropertiesConstants.YELLOW_CARD)
                        {
                            players.Find(p => p.Name == homeTeamEvent.Player).YellowCardsReceived++;
                        }
                    }

                    //AWAY TEAM
                    foreach (var player in matchResults[specificMatch].AwayTeamStatistics.StartingEleven)
                    {
                        players.Add(player);
                    }
                    foreach (var substitute in matchResults[specificMatch].AwayTeamStatistics.Substitutes)
                    {
                        players.Add(substitute);
                    }
                    //AWAY TEAM EVENTS
                    foreach (var awayTeamEvent in match.AwayTeamEvents)
                    {
                        if (awayTeamEvent.TypeOfEvent.Split(PropertiesConstants.SEPARATOR)[0] == PropertiesConstants.GOAL)
                        {
                            players.Find(p => p.Name == awayTeamEvent.Player).GoalsScored++;
                        }
                        else if (awayTeamEvent.TypeOfEvent == PropertiesConstants.YELLOW_CARD)
                        {
                            players.Find(p => p.Name == awayTeamEvent.Player).YellowCardsReceived++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    OnError?.Invoke(ex.Message);
                }
                finally
                {
                    Loading.LoadingScreen?.Hide();
                }
            }

            return players;
        }

        public async Task<List<Matches>?> GetSpecificCountryMatchResultsAsyncWomen(string countryCode)
        {
            if (_dataHandler.GetDataRetrievingMethod() == PropertiesConstants.METHOD_LOCAL)
            {
                try
                {
                    Loading.LoadingScreen?.Show();

                    string FileDirectory = Path.Combine(PropertiesConstants.DATA_FOLDER_PATH, WomenConstants.WOMEN_MATCHES_PATH);

                    if (File.Exists(FileDirectory))
                    {
                        string json = File.ReadAllText(FileDirectory);
                        List<Matches>? deserializedJson = JsonConvert.DeserializeObject<List<Matches>>(json);

                        var filteredMatches = deserializedJson.Where(
                            match => match.HomeTeam.Code == countryCode.ToUpper() || match.AwayTeam.Code == countryCode.ToUpper()).ToList();

                        return filteredMatches;
                    }
                }
                catch (Exception ex)
                {
                    OnError?.Invoke(ex.Message);
                }
                finally
                {
                    Loading.LoadingScreen?.Hide();
                }
            }
            else
            {
                try
                {
                    Loading.LoadingScreen?.Show();

                    using (var client = new HttpClient())
                    {
                        var endpoint = new Uri(WomenConstants.WOMEN_COUNTRY_URI + countryCode.ToUpper());
                        var result = await client.GetAsync(endpoint);
                        result.EnsureSuccessStatusCode();

                        string? json = await result.Content.ReadAsStringAsync();

                        List<Matches>? filteredMatches = JsonConvert.DeserializeObject<List<Matches>>(json);
                        return filteredMatches;
                    }
                }
                catch (HttpRequestException httpEx)
                {
                    OnError?.Invoke($"HTTP Request Error Occured: {httpEx.Message}");
                    return null;
                }
                catch (JsonException jsonEx)
                {
                    OnError?.Invoke($"JSON Deserialization Error Occured: {jsonEx.Message}");
                    return null;
                }
                catch (Exception ex)
                {
                    OnError?.Invoke($"An Error Occured: {ex.Message}");
                    return null;
                }
                finally
                {
                    Loading.LoadingScreen?.Hide();
                }
            }
            return null;
        }

        public async Task<List<Player>> GetStartingElevensWomen()
        {
            List<Matches> matches = new List<Matches>();
            List<Player> homeEleven = new List<Player>();
            List<Player> awayEleven = new List<Player>();
            List<Player> startingElevens = new List<Player>();

            try
            {
                Loading.LoadingScreen?.Show();

                matches = await GetSpecificCountryMatchResultsAsyncWomen(UserPreferences.FavouriteCountryCode.ToString());

                foreach (var match in matches)
                {
                    if (match.HomeTeam.Code == UserPreferences.FavouriteCountryCode.ToString() && match.AwayTeam.Code == UserPreferences.FavouriteCountryOpponentCode.ToString())
                    {
                        homeEleven = match.HomeTeamStatistics.StartingEleven.ToList();
                        awayEleven = match.AwayTeamStatistics.StartingEleven.ToList();
                    }
                    else if (match.AwayTeam.Code == UserPreferences.FavouriteCountryCode.ToString() && match.HomeTeam.Code == UserPreferences.FavouriteCountryOpponentCode.ToString())
                    {
                        homeEleven = match.AwayTeamStatistics.StartingEleven.ToList();
                        awayEleven = match.HomeTeamStatistics.StartingEleven.ToList();
                    }
                }

                foreach (Player player in homeEleven)
                {
                    player.DeclarePositionEnum();
                }

                foreach (Player player in awayEleven)
                {
                    player.DeclarePositionEnum();
                }

                homeEleven.Sort((x, y) => x.PlayerPositions.CompareTo(y.PlayerPositions));
                awayEleven.Sort((x, y) => y.PlayerPositions.CompareTo(x.PlayerPositions));

                foreach (Player player in homeEleven)
                {
                    startingElevens.Add(player);
                }
                foreach (Player player in awayEleven)
                {
                    startingElevens.Add(player);
                }

                if (UserPreferences.FavouriteCountryCode == "EGY" || UserPreferences.FavouriteCountryOpponentCode == "EGY")
                {
                    startingElevens.Find(p => p.Name == "TREZIGUET").Name = "TREZEGUET";
                }
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex.Message);
            }
            finally
            {
                Loading.LoadingScreen?.Hide();
            }

            return startingElevens;
        }
    }
}
