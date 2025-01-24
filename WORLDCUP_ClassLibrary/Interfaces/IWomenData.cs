using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WORLDCUP_ClassLibrary.Models;

namespace WORLDCUP_ClassLibrary.Interfaces
{
    public interface IWomenData
    {
        Task<List<Matches>?> GetAllMatchResultsAsyncWomen();
        Task<List<Matches>?> GetSpecificCountryMatchResultsAsyncWomen(string CountryCode);
        Task<List<CountryStats>?> GetAllCountryStatsAsyncWomen();
        Task<List<Team>> GetAllTeamStatsAsyncWomen();
        Task<List<GroupDetails>> GetAllGroupResultsAsyncWomen();
        Task<List<Player>> GetGoalsAndYellowCardsAsyncWomen(string CountryCode, int SpecificMatch = -1);
        Task<List<Player>> GetFavoriteTeamPlayersWomen();
        Task<List<Player>> GetStartingElevensWomen();
    }
}
