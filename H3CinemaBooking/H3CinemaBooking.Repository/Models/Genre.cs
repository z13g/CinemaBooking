using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace H3CinemaBooking.Repository.Models
{
    public class Genre
    {
        [Key]
        public int GenreID { get; set; }
        public string GenreName { get; set; }

        [JsonIgnore]
        public List<Movie>? Movies { get; set; }
    }
}
