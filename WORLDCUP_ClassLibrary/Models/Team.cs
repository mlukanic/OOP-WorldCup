using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WORLDCUP_ClassLibrary.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string? Country { get; set; }
        public object? AlternateName { get; set; }
        public string? FifaCode { get; set; }
        public int GroupId { get; set; }
        public string? GroupLetter { get; set; }
    }
}
