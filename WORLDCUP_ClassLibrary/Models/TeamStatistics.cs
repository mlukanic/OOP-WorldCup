using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WORLDCUP_ClassLibrary.Models
{
    public class TeamStatistics
    {
        public required string Country { get; set; }
        public int? AttemptsOnGoal { get; set; }
        public int? OnTarget { get; set; }
        public int? OffTarget { get; set; }
        public int? Blocked { get; set; }
        public int? Woodwork { get; set; }
        public int? Corners { get; set; }
        public int? Offsides { get; set; }
        public int? BallPossession { get; set; }
        public int? PassAccuracy { get; set; }
        public int? NumPasses { get; set; }
        public int? PassesCompleted { get; set; }
        public int? DistanceCovered { get; set; }
        public int? BallsRecovered { get; set; }
        public int? Tackles { get; set; }
        public int? Clearances { get; set; }
        public int? YellowCards { get; set; }
        public int? RedCards { get; set; }
        public string? FoulsCommitted { get; set; }
        public string? Tactics { get; set; }
        public required List<Player> StartingEleven { get; set; }
        public required List<Player> Substitutes { get; set; }
    }
}
