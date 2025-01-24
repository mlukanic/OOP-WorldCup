using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WORLDCUP_ClassLibrary.Models
{
    public class Weather
    {
        public required string Humidity { get; set; }
        public required string TempCelsius { get; set; }
        public required string TempFarenheit { get; set; }
        public string? WindSpeed { get; set; }
        public string? Description { get; set; }
    }
}
