using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WORLDCUP_ClassLibrary.Models.Enums;

namespace WORLDCUP_ClassLibrary.Models.Helpers
{
    public class PlayerPosition
    {
        private static readonly Dictionary<string, Positions> positionMap = new Dictionary<string, Positions>(StringComparer.OrdinalIgnoreCase)
        {
            { "goalie", Positions.Goalie },
            { "defender", Positions.Defender },
            { "midfield", Positions.Midfield },
            { "forward", Positions.Forward }
        };

        public static Positions GetPositionEnum(string position)
        {
            if (positionMap.TryGetValue(position, out var positionEnum))
            {
                return positionEnum;
            }
            else
            {
                throw new ArgumentException($"Invalid position: {position}");
            }
        }
    }
}
