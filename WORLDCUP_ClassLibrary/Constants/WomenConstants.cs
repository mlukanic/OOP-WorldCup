using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WORLDCUP_ClassLibrary.Constants
{
    public class WomenConstants
    {
        public const string WOMEN_MATCHES_URI = "https://worldcup-vua.nullbit.hr/women/matches";
        public const string WOMEN_COUNTRY_URI = "https://worldcup-vua.nullbit.hr/women/matches/country?fifa_code=";
        public const string WOMEN_RESULTS_URI = "https://worldcup-vua.nullbit.hr/women/teams/results";
        public const string WOMEN_TEAMS_URI = "https://worldcup-vua.nullbit.hr/women/teams";
        public const string WOMEN_GROUP_RESULTS_URI = "https://worldcup-vua.nullbit.hr/women/teams/group_results";

        public const string WOMEN_MATCHES_PATH = @"WCWomen\matches.json";
        public const string WOMEN_RESULTS_PATH = @"WCWomen\results.json";
        public const string WOMEN_TEAMS_PATH = @"WCWomen\teams.json";
        public const string WOMEN_GROUP_RESULTS_PATH = @"WCWomen\group_results.json";
    }
}
