using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WORLDCUP_ClassLibrary.Models;

namespace WORLDCUP_ClassLibrary.Interfaces
{
    public interface IMenData
    {
        Task<List<Matches>?> GetAllMatchResultsAsyncMen();
        Task<List<Matches>?> GetSpecificCountryMatchResultsAsyncMen(string CountryCode);
        Task<List<CountryStats>?> GetAllCountryStatsAsyncMen();
        Task<List<Team>> GetAllTeamStatsAsyncMen();
        Task<List<GroupDetails>> GetAllGroupResultsAsyncMen();
        Task<List<Player>> GetGoalsAndYellowCardsAsyncMen(string CountryCode, int SpecificMatch = -1);
        Task<List<Player>> GetFavoriteTeamPlayersMen();
        Task<List<Player>> GetStartingElevensMen();
    }
}
