using Humanizer.Localisation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace H3CinemaBooking.Repository.Models
{
    public class Movie
    {
        public int MovieID { get; set; }
        public string Title { get; set; }
        public int Duration { get; set; }
        public string Director { get; set; }
        public string MovieLink { get; set; }
        public string TrailerLink { get; set; }
        public List<Genre> ?Genres { get; set; }
    }
}
