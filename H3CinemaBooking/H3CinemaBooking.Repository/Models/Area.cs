using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace H3CinemaBooking.Repository.Models
{
    public class Area
    {
        public int AreaID { get; set; }
        public string AreaName { get; set; }
        public int RegionID { get; set; }
        [JsonIgnore]
        public virtual List<Cinema> ?Cinemas { get; set; }
    }
}
