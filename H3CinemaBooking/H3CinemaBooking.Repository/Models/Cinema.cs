using Microsoft.CodeAnalysis;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace H3CinemaBooking.Repository.Models
{
    /// <summary>
    /// Make 2 Projects (API Web) & (Class Libery)
    /// 1) Create Models
    /// 2) Install Packages (3-4) 
    ///     - Microsoft.entityFrameworkCore.SqlServer
    ///     - Microsoft.entityFrameworkCore.Tools
    ///     - Microsoft.entityFrameworkCore.Design
    ///     - Microsoft.VisualStudio.Web.CodeGeneration.Design
    ///     - Microsoft.EntityFrameworkCore
    /// 3) Create Database Context Class
    /// 4) Make Connection string, Program.cs add our Database service
    /// 5) go to console => add-migration name
    /// 6) go to console => Update-Database
    /// 7) Done
    /// 8) If The F....S....n....w
    /// </summary>
    public class Cinema
    {
        public int CinemaID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int NumberOfHalls { get; set; }

        // Foreign key
        public int AreaID { get; set; }

        // Navigation property
        [ForeignKey("AreaID")]
        [JsonIgnore]
        public virtual Area? Area { get; set; }

        //[JsonIgnore]
        //public virtual CinemaHall? CinemaHall { get; set; }
    }
}
