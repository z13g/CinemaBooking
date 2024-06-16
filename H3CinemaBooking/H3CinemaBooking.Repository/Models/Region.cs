using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace H3CinemaBooking.Repository.Models
{
    public class Region
    {
        public int RegionID { get; set; }
        public string RegionName { get; set; }
        [JsonIgnore]
        public virtual List<Area>? Areas { get; set; }
    }
}
