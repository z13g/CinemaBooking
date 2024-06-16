using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3CinemaBooking.Repository.Interfaces
{
    public interface IPropertyValidationService
    {
        bool ValidateProperties(dynamic obj, string[] propertiesToSkip);
    }

}
