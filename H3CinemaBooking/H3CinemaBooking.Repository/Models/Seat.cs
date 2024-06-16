using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace H3CinemaBooking.Repository.Models
{
    public class Seat
    {
        [Key]
        public int SeatID { get; set; }
        public int HallID { get; set; }
        public int SeatNumber { get; set; } 
        public char SeatRow { get; set; }
        [JsonIgnore]
        public CinemaHall ?CinemaHall { get; set; }
        [JsonIgnore]
        public virtual List<BookingSeat> ?BookingSeats { get; set; }
    }
}
