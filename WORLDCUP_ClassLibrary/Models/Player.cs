using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WORLDCUP_ClassLibrary.Models.Enums;
using WORLDCUP_ClassLibrary.Models.Helpers;

namespace WORLDCUP_ClassLibrary.Models
{
    public class Player : IComparable<Player>
    {
        public string? Name { get; set; }
        public bool Captain { get; set; }
        public int ShirtNumber { get; set; }
        public string? Position { get; set; }
        public string? CountryOrigin { get; set; }
        public Gender PlayerGender { get; set; }
        public int GoalsScored { get; set; }
        public int YellowCardsReceived { get; set; }
        public Positions PlayerPositions { get; set; }

        public int CompareTo(Player? other) => ShirtNumber.CompareTo(other?.ShirtNumber);
        public void DeclarePositionEnum() => PlayerPositions = PlayerPosition.GetPositionEnum(Position);

        public override bool Equals(object? obj)
        {
            return obj is Player player &&
                   Name == player.Name &&
                   ShirtNumber == player.ShirtNumber;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, ShirtNumber);
        }

        public override string? ToString()
        {
            return $"{ShirtNumber} - {Name} - {Position} {(Captain ? "- Kapetan" : "")}";
        }
    }
}
