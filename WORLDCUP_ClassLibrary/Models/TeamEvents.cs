using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WORLDCUP_ClassLibrary.Models
{
    public class TeamEvents
    {
        public int Id { get; set; }
        public string? TypeOfEvent { get; set; }
        public string? Player { get; set; }
        public string? Time { get; set; }
    }
}
