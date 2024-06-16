using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3CinemaBooking.Repository.Models
{
    public class BookingSeat
    {
        [Key]
        public int BookingSeatID { get; set; }

        [ForeignKey("Booking")]
        public int BookingID { get; set; }
        public Booking Booking { get; set; }

        [ForeignKey("Seat")]
        public int SeatID { get; set; }
        public Seat Seat { get; set; }

        public SeatStatus Status { get; set; }
    }

}
