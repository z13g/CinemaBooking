using H3CinemaBooking.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace H3CinemaBooking.Repository.Service
{
    public class PropertyValidationService : IPropertyValidationService
    {
        public bool ValidateProperties(dynamic obj, string[] propertiesToSkip)
        {
            if (obj == null) return false;

            PropertyInfo[] properties = obj.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (Array.Exists(propertiesToSkip, element => element == property.Name))
                {
                    continue;
                }

                object value = property.GetValue(obj);
                if (value == null || (value is int intValue && intValue <= 0) || (value is string stringValue && string.IsNullOrWhiteSpace(stringValue)))
                {
                    Console.WriteLine($"Property {property.Name} is null");
                    return false;
                }
            }

            return true;
        }

    }

}
