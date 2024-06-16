using Microsoft.CodeAnalysis;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace H3CinemaBooking.Repository.Models
{
    public class Show
    {
        [Key]
        public int ShowID { get; set; }
        public int HallID { get; set; }
        public int MovieID { get; set; }
        public double Price { get; set; }
        public DateTime ShowDateTime { get; set; }

        [JsonIgnore]
        public List<Booking> ?Bookings { get; set; } = new List<Booking>();

    }
}
