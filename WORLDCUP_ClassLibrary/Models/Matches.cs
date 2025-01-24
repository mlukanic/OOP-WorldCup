using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WORLDCUP_ClassLibrary.Models
{
    public class Matches
    {
        public required string Venue { get; set; }
        public required string Location { get; set; }
        public required string Status { get; set; }
        public required string Time { get; set; }
        public required string FifaId { get; set; }
        public required Weather Weather { get; set; }
        public required string Attendance { get; set; }
        public required List<string> Officials { get; set; }
        public required string StageName { get; set; }
        public required string HomeTeamCountry { get; set; }
        public required string AwayTeamCountry { get; set; }
        public required DateTime Datetime { get; set; }
        public required string Winner { get; set; }
        public required string WinnerCode { get; set; }
        public required MatchTeam HomeTeam { get; set; }
        public required MatchTeam AwayTeam { get; set; }
        public required List<TeamEvents> HomeTeamEvents { get; set; }
        public required List<TeamEvents> AwayTeamEvents { get; set; }
        public required TeamStatistics HomeTeamStatistics { get; set; }
        public required TeamStatistics AwayTeamStatistics { get; set; }
        public required DateTime LastEventUpdateAt { get; set; }
        public required object LastScoreUpdateAt { get; set; }
    }
}

