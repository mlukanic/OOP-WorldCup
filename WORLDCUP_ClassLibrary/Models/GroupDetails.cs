using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WORLDCUP_ClassLibrary.Models
{
    public class GroupDetails
    {
        public int Id { get; set; }
        public string Letter { get; set; }
        public List<CountryStats> OrderedTeams { get; set; }
    }
}
