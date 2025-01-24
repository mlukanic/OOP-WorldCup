using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WORLDCUP_ClassLibrary.Constants;
using WORLDCUP_ClassLibrary.Interfaces;
using WORLDCUP_ClassLibrary.Models;
using WORLDCUP_ClassLibrary.Services;
using WORLDCUP_ClassLibrary.Factories;
using WORLDCUP_ClassLibrary.Utilities;

namespace WORLDCUP_ClassLibrary.Repositories
{
    public class MenDataRepository : IMenData
    {
        private readonly IDataHandler _dataHandler;
        private readonly HttpService _httpService;
        private readonly FileService _fileService;

        public MenDataRepository(IDataHandler dataHandler)
        {
            _dataHandler = dataHandler;
            _httpService = ServiceFactory.GetHttpService();
            _fileService = ServiceFactory.GetFileService();
        }

        public event Action<string> OnError;

        public async Task<List<CountryStats>?> GetAllCountryStatsAsyncMen()
        {
            try
            {
                string? json;
                if (_dataHandler.GetDataRetrievingMethod() == PropertiesConstants.METHOD_LOCAL)
                {
                    string fileDirectory = Path.Combine(PropertiesConstants.DATA_FOLDER_PATH, MenConstants.MEN_MATCHES_PATH);
                    json = await _fileService.ReadFileAsync(fileDirectory);
                }
                else
                {
                    json = await _httpService.GetJsonAsync(MenConstants.MEN_MATCHES_URI);
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

        public async Task<List<GroupDetails>> GetAllGroupResultsAsyncMen()
        {
            try
            {
                string? json;
                if (_dataHandler.GetDataRetrievingMethod() == PropertiesConstants.METHOD_LOCAL)
                {
                    string fileDirectory = Path.Combine(PropertiesConstants.DATA_FOLDER_PATH, MenConstants.MEN_GROUP_RESULTS_PATH);
                    json = await _fileService.ReadFileAsync(fileDirectory);
                }
                else
                {
                    json = await _httpService.GetJsonAsync(MenConstants.MEN_GROUP_RESULTS_URI);
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

        public async Task<List<Matches>?> GetAllMatchResultsAsyncMen()
        {
            try
            {
                string? json;
                if (_dataHandler.GetDataRetrievingMethod() == PropertiesConstants.METHOD_LOCAL)
                {
                    string fileDirectory = Path.Combine(PropertiesConstants.DATA_FOLDER_PATH, MenConstants.MEN_MATCHES_PATH);
                    json = await _fileService.ReadFileAsync(fileDirectory);
                }
                else
                {
                    json = await _httpService.GetJsonAsync(MenConstants.MEN_MATCHES_URI);
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

        public async Task<List<Team>> GetAllTeamStatsAsyncMen()
        {
            try
            {
                Loading.LoadingScreen?.Show();

                string? json;
                if (_dataHandler.GetDataRetrievingMethod() == PropertiesConstants.METHOD_LOCAL)
                {
                    string fileDirectory = Path.Combine(PropertiesConstants.DATA_FOLDER_PATH, MenConstants.MEN_TEAMS_PATH);
                    json = await _fileService.ReadFileAsync(fileDirectory);
                }
                else
                {
                    json = await _httpService.GetJsonAsync(MenConstants.MEN_TEAMS_URI);
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

        public async Task<List<Player>> GetFavoriteTeamPlayersMen()
        {
            List<Matches> matches = new List<Matches>();
            List<Player> allPlayers = new List<Player>();

            try
            {
                Loading.LoadingScreen?.Show();

                matches = await GetSpecificCountryMatchResultsAsyncMen(UserPreferences.FavouriteCountryCode.ToString());

                if (matches.First().HomeTeam.Code == UserPreferences.FavouriteCountryCode.ToString())
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

        public async Task<List<Player>> GetGoalsAndYellowCardsAsyncMen(string countryCode, int specificMatch = -1)
        {
            var matchResult = await GetSpecificCountryMatchResultsAsyncMen(countryCode);
            List<Player> Players = new List<Player>();

            if (specificMatch == -1)
            {
                try
                {
                    Loading.LoadingScreen?.Show();

                    if (matchResult[0].HomeTeam.Code == countryCode)
                    {
                        foreach (var player in matchResult[0].HomeTeamStatistics.StartingEleven)
                        {
                            Players.Add(player);
                        }
                        foreach (var substitute in matchResult[0].HomeTeamStatistics.Substitutes)
                        {
                            Players.Add(substitute);
                        }
                    }
                    else
                    {
                        foreach (var player in matchResult[0].AwayTeamStatistics.StartingEleven)
                        {
                            Players.Add(player);
                        }
                        foreach (var substitute in matchResult[0].AwayTeamStatistics.Substitutes)
                        {
                            Players.Add(substitute);
                        }
                    }

                    Players.Sort();


                    if (countryCode == "EGY")
                    {
                        Players.Find(p => p.Name == "TREZIGUET").Name = "TREZEGUET";
                    }

                    foreach (var match in matchResult)
                    {
                        if (match.HomeTeam.Code == countryCode)
                        {
                            foreach (var homeTeamEvent in match.HomeTeamEvents)
                            {
                                if (homeTeamEvent.TypeOfEvent.Split(PropertiesConstants.SEPARATOR)[0] == PropertiesConstants.GOAL)
                                {
                                    Players.Find(p => p.Name == homeTeamEvent.Player).GoalsScored++;
                                }
                                else if (homeTeamEvent.TypeOfEvent == PropertiesConstants.YELLOW_CARD)
                                {
                                    Players.Find(p => p.Name == homeTeamEvent.Player).YellowCardsReceived++;
                                }
                            }
                        }
                        else
                        {
                            foreach (var awayTeamEvent in match.AwayTeamEvents)
                            {
                                if (awayTeamEvent.TypeOfEvent.Split(PropertiesConstants.SEPARATOR)[0] == PropertiesConstants.GOAL)
                                {
                                    Players.Find(p => p.Name == awayTeamEvent.Player).GoalsScored++;
                                }
                                else if (awayTeamEvent.TypeOfEvent == PropertiesConstants.YELLOW_CARD)
                                {
                                    if (countryCode == "EGY" && awayTeamEvent.Player == "TREZIGUET")
                                    {
                                        continue;
                                    }
                                    Players.Find(p => p.Name == awayTeamEvent.Player).YellowCardsReceived++;
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
            else if (specificMatch < matchResult.Count && specificMatch >= 0)
            {
                try
                {
                    Loading.LoadingScreen?.Show();

                    var match = matchResult[specificMatch];

                    //HOME TEAM
                    foreach (var player in matchResult[specificMatch].HomeTeamStatistics.StartingEleven)
                    {
                        Players.Add(player);
                    }
                    foreach (var substitute in matchResult[specificMatch].HomeTeamStatistics.Substitutes)
                    {
                        Players.Add(substitute);
                    }
                    //AWAY TEAM
                    foreach (var player in matchResult[specificMatch].AwayTeamStatistics.StartingEleven)
                    {
                        Players.Add(player);
                    }
                    foreach (var substitute in matchResult[specificMatch].AwayTeamStatistics.Substitutes)
                    {
                        Players.Add(substitute);
                    }

                    //HOME TEAM EVENTS
                    foreach (var homeTeamEvent in match.HomeTeamEvents)
                    {
                        if (homeTeamEvent.TypeOfEvent.Split(PropertiesConstants.SEPARATOR)[0] == PropertiesConstants.GOAL)
                        {
                            Players.Find(p => p.Name == homeTeamEvent.Player).GoalsScored++;
                        }
                        else if (homeTeamEvent.TypeOfEvent == PropertiesConstants.YELLOW_CARD)
                        {
                            Players.Find(p => p.Name == homeTeamEvent.Player).YellowCardsReceived++;
                        }
                    }

                    //AWAY TEAM EVENTS
                    foreach (var awayTeamEvent in match.AwayTeamEvents)
                    {
                        if (awayTeamEvent.TypeOfEvent.Split(PropertiesConstants.SEPARATOR)[0] == PropertiesConstants.GOAL)
                        {
                            Players.Find(p => p.Name == awayTeamEvent.Player).GoalsScored++;
                        }
                        else if (awayTeamEvent.TypeOfEvent == PropertiesConstants.YELLOW_CARD)
                        {
                            Players.Find(p => p.Name == awayTeamEvent.Player).YellowCardsReceived++;
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

            return Players;
        }

        public async Task<List<Matches>?> GetSpecificCountryMatchResultsAsyncMen(string countryCode)
        {
            if (_dataHandler.GetDataRetrievingMethod() == PropertiesConstants.METHOD_LOCAL)
            {
                try
                {
                    Loading.LoadingScreen?.Show();

                    string FileDirectory = Path.Combine(PropertiesConstants.DATA_FOLDER_PATH, MenConstants.MEN_MATCHES_PATH);

                    if (File.Exists(FileDirectory))
                    {
                        string json = await _fileService.ReadFileAsync(FileDirectory);
                        List<Matches>? deserializedJson = JsonConvert.DeserializeObject<List<Matches>>(json);

                        var filteredMatches = deserializedJson.Where(
                            match => match.HomeTeam.Code == countryCode.ToUpper() || match.AwayTeam.Code == countryCode.ToUpper()).ToList();

                        return filteredMatches;
                    }
                }
                catch (Exception ex)
                {
                    OnError?.Invoke($"An Error Occured: {ex.Message}");
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
                    string? json = await _httpService.GetJsonAsync(MenConstants.MEN_COUNTRY_URI + countryCode.ToUpper());
                    if (json != null)
                    {
                        List<Matches>? filteredMatches = JsonConvert.DeserializeObject<List<Matches>>(json);
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
            return null;
        }

        public async Task<List<Player>> GetStartingElevensMen()
        {
            List<Matches> matches = new List<Matches>();
            List<Player> homeEleven = new List<Player>();
            List<Player> awayEleven = new List<Player>();
            List<Player> startingEleven = new List<Player>();

            try
            {
                Loading.LoadingScreen?.Show();

                matches = await GetSpecificCountryMatchResultsAsyncMen(UserPreferences.FavouriteCountryCode.ToString());

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
                    startingEleven.Add(player);
                }
                foreach (Player player in awayEleven)
                {
                    startingEleven.Add(player);
                }

                if (UserPreferences.FavouriteCountryCode == "EGY" || UserPreferences.FavouriteCountryOpponentCode == "EGY")
                {
                    startingEleven.Find(p => p.Name == "TREZIGUET").Name = "TREZEGUET";
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

            return startingEleven;
        }
    }
}
