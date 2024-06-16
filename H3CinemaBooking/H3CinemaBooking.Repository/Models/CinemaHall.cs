using Microsoft.CodeAnalysis;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace H3CinemaBooking.Repository.Models
{
    public class CinemaHall
    {
        [Key]
        public int HallsID { get; set; }
        public string HallName { get; set; }
        [ForeignKey("CinemaID")]
        public int CinemaID { get; set; }
        [JsonIgnore]
        public List<Seat> ?Seats { get; set; }
    }
}
