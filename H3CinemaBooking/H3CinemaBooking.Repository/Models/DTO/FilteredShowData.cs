using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3CinemaBooking.Repository.Models.DTO_s
{
    public class FilteredShowDTO
    {
        public string Date { get; set; }
        public List<ShowDetailsDTO> Shows { get; set; }
    }
}
