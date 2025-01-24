using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WORLDCUP_ClassLibrary.Models
{
    public class MatchTeam
    {
        public required string Country { get; set; }
        public required string Code { get; set; }
        public int Goals { get; set; }
        public int Penalties { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is MatchTeam team &&
                   Country == team.Country &&
                   Code == team.Code;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Country);
        }
    }
}
