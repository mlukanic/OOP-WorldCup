using Newtonsoft.Json;
using WORLDCUP_ClassLibrary.Constants;
using WORLDCUP_ClassLibrary.Factories;
using WORLDCUP_ClassLibrary.Interfaces;
using WORLDCUP_ClassLibrary.Models;
using WORLDCUP_ClassLibrary.Services;
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

        private async Task<string?> GetDataAsync(string localPath, string remoteUri)
        {
            try
            {
                if (_dataHandler.GetDataRetrievingMethod() == PropertiesConstants.METHOD_LOCAL)
                {
                    string fileDirectory = Path.Combine(PropertiesConstants.DATA_FOLDER_PATH, localPath);
                    return await _fileService.ReadFileAsync(fileDirectory);
                }
                else
                {
                    return await _httpService.GetJsonAsync(remoteUri);
                }
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex.Message);
                return null;
            }
        }

        public async Task<List<CountryStats>?> GetAllCountryStatsAsyncMen()
        {
            string? json = await GetDataAsync(MenConstants.MEN_MATCHES_PATH, MenConstants.MEN_MATCHES_URI);
            return json != null ? JsonConvert.DeserializeObject<List<CountryStats>>(json) : null;
        }

        public async Task<List<GroupDetails>> GetAllGroupResultsAsyncMen()
        {
            string? json = await GetDataAsync(MenConstants.MEN_GROUP_RESULTS_PATH, MenConstants.MEN_GROUP_RESULTS_URI);
            return json != null ? JsonConvert.DeserializeObject<List<GroupDetails>>(json) : null;
        }

        public async Task<List<Matches>?> GetAllMatchResultsAsyncMen()
        {
            string? json = await GetDataAsync(MenConstants.MEN_MATCHES_PATH, MenConstants.MEN_MATCHES_URI);
            return json != null ? JsonConvert.DeserializeObject<List<Matches>>(json) : null;
        }

        public async Task<List<Team>> GetAllTeamStatsAsyncMen()
        {
            try
            {
                Loading.LoadingScreen?.Show();
                string? json = await GetDataAsync(MenConstants.MEN_TEAMS_PATH, MenConstants.MEN_TEAMS_URI);
                return json != null ? JsonConvert.DeserializeObject<List<Team>>(json) : null;
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex.Message);
                return null;
            }
            finally
            {
                Loading.LoadingScreen?.Hide();
            }
        }

        public async Task<List<Player>> GetFavoriteTeamPlayersMen()
        {
            List<Player> allPlayers = new List<Player>();

            try
            {
                Loading.LoadingScreen?.Show();
                var matches = await GetSpecificCountryMatchResultsAsyncMen(UserPreferences.FavouriteCountryCode.ToString());

                if (matches != null && matches.Any())
                {
                    var match = matches.First();
                    var teamStats = match.HomeTeam.Code == UserPreferences.FavouriteCountryCode.ToString() ? match.HomeTeamStatistics : match.AwayTeamStatistics;
                    allPlayers = teamStats.StartingEleven.Concat(teamStats.Substitutes).ToList();
                    allPlayers.Sort();

                    if (UserPreferences.FavouriteCountryCode == "EGY")
                    {
                        allPlayers.Find(p => p.Name == "TREZIGUET").Name = "TREZEGUET";
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

            return allPlayers;
        }

        public async Task<List<Player>> GetGoalsAndYellowCardsAsyncMen(string countryCode, int specificMatch = -1)
        {
            var matchResults = await GetSpecificCountryMatchResultsAsyncMen(countryCode);
            List<Player> players = new List<Player>();

            if (matchResults == null || !matchResults.Any())
                return players;

            try
            {
                Loading.LoadingScreen?.Show();

                if (specificMatch == -1)
                {
                    players = GetPlayersFromMatch(matchResults.First(), countryCode);
                    foreach (var match in matchResults)
                    {
                        UpdatePlayerStats(players, match, countryCode);
                    }
                }
                else if (specificMatch < matchResults.Count && specificMatch >= 0)
                {
                    var match = matchResults[specificMatch];
                    players = GetPlayersFromMatch(match, countryCode);
                    UpdatePlayerStats(players, match, countryCode);
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

            return players;
        }

        private List<Player> GetPlayersFromMatch(Matches match, string countryCode)
        {
            var players = new List<Player>();
            var teamStats = match.HomeTeam.Code == countryCode ? match.HomeTeamStatistics : match.AwayTeamStatistics;
            players.AddRange(teamStats.StartingEleven);
            players.AddRange(teamStats.Substitutes);
            players.Sort();
            return players;
        }

        private void UpdatePlayerStats(List<Player> players, Matches match, string countryCode)
        {
            var teamEvents = match.HomeTeam.Code == countryCode ? match.HomeTeamEvents : match.AwayTeamEvents;
            foreach (var teamEvent in teamEvents)
            {
                var player = players.Find(p => p.Name == teamEvent.Player);
                if (player != null)
                {
                    if (teamEvent.TypeOfEvent.Split(PropertiesConstants.SEPARATOR)[0] == PropertiesConstants.GOAL)
                    {
                        player.GoalsScored++;
                    }
                    else if (teamEvent.TypeOfEvent == PropertiesConstants.YELLOW_CARD)
                    {
                        player.YellowCardsReceived++;
                    }
                }
            }
        }

        public async Task<List<Matches>?> GetSpecificCountryMatchResultsAsyncMen(string countryCode)
        {
            string? json = await GetDataAsync(MenConstants.MEN_MATCHES_PATH, MenConstants.MEN_COUNTRY_URI + countryCode.ToUpper());
            if (json != null)
            {
                var matches = JsonConvert.DeserializeObject<List<Matches>>(json);
                return matches?.Where(match => match.HomeTeam.Code == countryCode.ToUpper() || match.AwayTeam.Code == countryCode.ToUpper()).ToList();
            }
            return null;
        }

        public async Task<List<Player>> GetStartingElevensMen()
        {
            List<Player> startingElevens = new List<Player>();

            try
            {
                Loading.LoadingScreen?.Show();
                var matches = await GetSpecificCountryMatchResultsAsyncMen(UserPreferences.FavouriteCountryCode.ToString());

                if (matches != null)
                {
                    foreach (var match in matches)
                    {
                        if (match.HomeTeam.Code == UserPreferences.FavouriteCountryCode.ToString() && match.AwayTeam.Code == UserPreferences.FavouriteCountryOpponentCode.ToString())
                        {
                            startingElevens.AddRange(match.HomeTeamStatistics.StartingEleven);
                            startingElevens.AddRange(match.AwayTeamStatistics.StartingEleven);
                        }
                        else if (match.AwayTeam.Code == UserPreferences.FavouriteCountryCode.ToString() && match.HomeTeam.Code == UserPreferences.FavouriteCountryOpponentCode.ToString())
                        {
                            startingElevens.AddRange(match.AwayTeamStatistics.StartingEleven);
                            startingElevens.AddRange(match.HomeTeamStatistics.StartingEleven);
                        }
                    }

                    foreach (var player in startingElevens)
                    {
                        player.DeclarePositionEnum();
                    }

                    startingElevens.Sort((x, y) => x.PlayerPositions.CompareTo(y.PlayerPositions));

                    if (UserPreferences.FavouriteCountryCode == "EGY" || UserPreferences.FavouriteCountryOpponentCode == "EGY")
                    {
                        startingElevens.Find(p => p.Name == "TREZIGUET").Name = "TREZEGUET";
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

            return startingElevens;
        }
    }
}
