using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WORLDCUP_ClassLibrary.Constants
{
    public class MenConstants
    {
        public const string MEN_MATCHES_URI = "https://worldcup-vua.nullbit.hr/men/matches";
        public const string MEN_COUNTRY_URI = "https://worldcup-vua.nullbit.hr/men/matches/country?fifa_code=";
        public const string MEN_RESULTS_URI = "https://worldcup-vua.nullbit.hr/men/teams/results";
        public const string MEN_TEAMS_URI = "https://worldcup-vua.nullbit.hr/men/teams";
        public const string MEN_GROUP_RESULTS_URI = "https://worldcup-vua.nullbit.hr/men/teams/group_results";

        public const string MEN_MATCHES_PATH = @"WCMen\matches.json";
        public const string MEN_RESULTS_PATH = @"WCMen\results.json";
        public const string MEN_TEAMS_PATH = @"WCMen\teams.json";
        public const string MEN_GROUP_RESULTS_PATH = @"WCMen\group_results.json";
    }
}
