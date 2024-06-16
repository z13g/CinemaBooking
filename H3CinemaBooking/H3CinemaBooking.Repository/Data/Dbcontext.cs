using H3CinemaBooking.Repository.DTO;
using H3CinemaBooking.Repository.Models;
//using H3CinemaBooking.Repository.Models.ManyModel;
using Microsoft.EntityFrameworkCore;

namespace H3CinemaBooking.Repository.Data
{
    public class Dbcontext : DbContext
    {
        public Dbcontext(DbContextOptions<Dbcontext> options) : base(options)
        {

        }

        public DbSet<UserDetail> Costumers { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<CinemaHall> CinemaHalls { get; set; }
        public DbSet<Show> Shows { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<BookingSeat> BookingSeats { get; set; }
        public DbSet<UserDetail> UserDetails { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Region> Regions { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Genre>()
                .HasMany(g => g.Movies)
                .WithMany(m => m.Genres)
                .UsingEntity<Dictionary<string, object>>(
                    "MovieGenre",
                j => j.HasOne<Movie>().WithMany().HasForeignKey("MovieID"),
                j => j.HasOne<Genre>().WithMany().HasForeignKey("GenreID"));

            modelBuilder.Entity<CinemaHall>()
                .HasOne<Cinema>()
                .WithMany()
                .HasForeignKey(ch => ch.CinemaID)
                .IsRequired();

            modelBuilder.Entity<Cinema>()
                .HasOne(c => c.Area)
                .WithMany(a => a.Cinemas)
                .HasForeignKey(c => c.AreaID); 

            modelBuilder.Entity<CinemaHall>()
                .HasMany(h => h.Seats)
                .WithOne(s => s.CinemaHall)
                .HasForeignKey(s => s.HallID);

            modelBuilder.Entity<Show>()
                .HasOne<Movie>()
                .WithMany()
                .HasForeignKey(s => s.MovieID)
                .IsRequired();

            modelBuilder.Entity<Show>()
                .HasOne<CinemaHall>()
                .WithMany()
                .HasForeignKey(s => s.HallID)
                .IsRequired();

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.userDetail)
                .WithMany(c => c.Bookings)
                .HasForeignKey(b => b.UserDetailID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Show)
                .WithMany(s => s.Bookings)
                .HasForeignKey(b => b.ShowID)
                .IsRequired();

            modelBuilder.Entity<BookingSeat>()
                .HasOne(bs => bs.Booking)
                .WithMany(b => b.BookingSeats)
                .HasForeignKey(bs => bs.BookingID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BookingSeat>()
                .HasOne(bs => bs.Seat)
                .WithMany(s => s.BookingSeats)
                .HasForeignKey(bs => bs.SeatID);

            modelBuilder.Entity<BookingSeat>()
                .Property(bs => bs.Status)
                .HasConversion<int>(); // Denne konvertere Status til Int i Databasen


            //Seeding Sheet - Her laver jeg test data til alle mine tabeller

            //Genre Sheet
            modelBuilder.Entity<Genre>().HasData(new Genre { GenreID = 1, GenreName = "Action" }, new Genre { GenreID = 2, GenreName = "Sci-Fi" }, new Genre { GenreID = 3, GenreName = "Fantasy" });

            //Movie Sheet
            modelBuilder.Entity<Movie>().HasData(new Movie { MovieID = 1, Title = "Jagt Sæson 2", Duration = 123, Director = "Peter Molde-Amelung", MovieLink = "/static/assets/images/Jagtsaeson2_ImedgangogModgang.jpeg", TrailerLink = "https://www.youtube.com/embed/s16LRoDE7ow?start=0" },new Movie { MovieID = 2, Title = "Challenger", Duration = 251, Director = "Luca Guadagnino", MovieLink = "/static/assets/images/Challenger_Movie.jpeg", TrailerLink = "https://www.youtube.com/embed/AI1mL_LjYyU?start=0" },new Movie { MovieID = 3, Title = "Kung-Fu Panda 4", Duration = 126, Director = "Mike Mitchell", MovieLink = "/static/assets/images/Kungfu-Panda4.jpeg", TrailerLink = "https://www.youtube.com/embed/L0iKmb7q_Lg?start=0" }, new Movie { MovieID = 4, Title = "The Fall Guy", Duration = 96, Director = "David Leitch", MovieLink = "/static/assets/images/FallGuy.jpeg", TrailerLink = "https://www.youtube.com/embed/j7jPnwVGdZ8?start=0" },new Movie { MovieID = 5, Title = "Garfield: Lasagna Chronicles", Duration = 105, Director = "Phil Lord and Christopher Miller", MovieLink = "/static/assets/images/Garfield_LasagnaChronicles.jpeg", TrailerLink = "https://www.youtube.com/embed/wbGsTR5MvKQ?start=0" },new Movie { MovieID = 6, Title = "Immaculate", Duration = 110, Director = "Signe Baumane", MovieLink = "/static/assets/images/Immaculate.jpeg", TrailerLink = "https://www.youtube.com/embed/Oae2FT2H3EM?start=0" },new Movie { MovieID = 7, Title = "Kingdom of the Planet of the Apes", Duration = 134, Director = "Esben Toft Jacobsen", MovieLink = "/static/assets/images/KingdomPlanetApes.jpeg", TrailerLink = "https://www.youtube.com/embed/Kdr5oedn7q8?start=0" }, new Movie { MovieID = 8, Title = "Furiosa: A Mad Max Saga", Duration = 98, Director = "Christina Rosendahl", MovieLink = "/static/assets/images/FuriosaMadMax.jpeg", TrailerLink = "https://www.youtube.com/embed/FVswuip0-co?start=0" });

            //Region Sheet
            modelBuilder.Entity<Region>().HasData(new Region { RegionID = 1, RegionName = "Storkøbenhavn" }, new Region { RegionID = 2, RegionName = "Jylland" }, new Region { RegionID = 3, RegionName = "Sjælland og Øer" }, new Region { RegionID = 4, RegionName = "Fyn" });

            //MovieGenre Sheet
            //  modelBuilder.Entity<MovieGenre>().HasData(new MovieGenre { MovieID = 1, GenreID = 1}, new MovieGenre { MovieID = 1, GenreID = 2 }, new MovieGenre { MovieID = 1, GenreID = 3 }, new MovieGenre { MovieID = 2, GenreID = 1 }, new MovieGenre { MovieID = 2, GenreID = 2 }, new MovieGenre { MovieID = 3, GenreID = 1 });

            //Area Sheet
            modelBuilder.Entity<Area>().HasData(new Area { AreaID = 1, AreaName = "Storkøbenhavn", RegionID = 1 }, new Area { AreaID = 2, AreaName = "Aalborg", RegionID = 2}, new Area { AreaID = 3, AreaName = "Aarhus", RegionID = 2}, new Area { AreaID = 4, AreaName = "Esbjerg", RegionID = 2}, new Area { AreaID = 5, AreaName = "Frederikssund", RegionID = 3}, new Area { AreaID = 6, AreaName = "Herning", RegionID = 2 }, new Area { AreaID = 7, AreaName = "Hillerød", RegionID = 3}, new Area { AreaID = 8, AreaName = "Kolding", RegionID = 2 }, new Area { AreaID = 9, AreaName = "Køge", RegionID = 3}, new Area { AreaID = 10, AreaName = "Nykøbing Falster", RegionID = 3}, new Area { AreaID = 11, AreaName = "Næstved", RegionID = 3}, new Area { AreaID = 12, AreaName = "Odense", RegionID = 4}, new Area { AreaID = 13, AreaName = "Randers", RegionID = 2 }, new Area { AreaID = 14, AreaName = "Viborg", RegionID = 2 });

            //Cinema Sheet
            modelBuilder.Entity<Cinema>().HasData(new Cinema { CinemaID = 1, Name = "Palads", Location = "NørreBro", NumberOfHalls = 8, AreaID = 1 }, new Cinema { CinemaID = 2, Name = "Fields", Location = "Fields", NumberOfHalls = 12, AreaID = 1 }, new Cinema { CinemaID = 3, Name = "Falkoner Bio", Location = "Falkoner", NumberOfHalls = 6, AreaID = 1 }, new Cinema { CinemaID = 4, Name = "Aalborg City Syd", Location = "City Syd", NumberOfHalls = 6, AreaID = 2}, new Cinema { CinemaID = 5, Name = "Trøjborg", Location = "Aarhus", NumberOfHalls = 8, AreaID = 3}, new Cinema { CinemaID = 6, Name = "Dagmar", Location = "Dagmar Gade", NumberOfHalls = 6, AreaID = 1}, new Cinema { CinemaID = 7, Name = "Imperial", Location = "København", NumberOfHalls = 8, AreaID = 1}, new Cinema { CinemaID = 8, Name = "Lyngby Kinopalæet", Location = "Lyngby", NumberOfHalls = 12, AreaID = 1}, new Cinema { CinemaID = 9, Name = "Taastrup", Location = "Taastrup", NumberOfHalls = 10, AreaID = 1}, new Cinema { CinemaID = 10, Name = "Waves", Location = "Greve", NumberOfHalls = 6, AreaID = 1}, new Cinema { CinemaID = 11, Name = "Aalborg Kennedy", Location = "Aalborg", NumberOfHalls = 8, AreaID = 2}, new Cinema { CinemaID = 12, Name = "Aarhus C", Location = "Aarhus", NumberOfHalls = 6, AreaID = 3}, new Cinema { CinemaID = 13, Name = "Esbjerg Broen", Location = "Esbjerg", NumberOfHalls = 4, AreaID = 4}, new Cinema { CinemaID = 14, Name = "Herning", Location = "Herning", NumberOfHalls = 8, AreaID = 6}, new Cinema { CinemaID = 15, Name = "Kolding", Location = "Kolding", NumberOfHalls = 8, AreaID = 8}, new Cinema { CinemaID = 16, Name = "Randers", Location = "Randers", NumberOfHalls = 6, AreaID = 13}, new Cinema {CinemaID = 17, Name = "Viborg", Location = "Viborg", NumberOfHalls = 6, AreaID = 14 }, new Cinema { CinemaID = 18, Name = "Frederikssund", Location = "Frederikssund", NumberOfHalls = 6, AreaID = 5}, new Cinema { CinemaID = 19, Name = "Hillerød", Location = "Hillerød", NumberOfHalls = 6, AreaID = 7}, new Cinema { CinemaID = 20, Name = "Køge", Location = "Køge", NumberOfHalls = 6, AreaID = 9}, new Cinema { CinemaID = 21, Name = "Nykøbing Falster", Location = "Nykøbing Falster", NumberOfHalls = 2, AreaID = 10}, new Cinema {CinemaID = 22, Name="Næstved", Location = "Næstved", NumberOfHalls = 4, AreaID = 11}, new Cinema { CinemaID = 23, Name ="Odense", Location = "Odense", NumberOfHalls = 8, AreaID = 12});

            //CinemaHall Sheet
            modelBuilder.Entity<CinemaHall>().HasData(new CinemaHall { HallsID = 1, CinemaID = 1, HallName = "Sal 1" }, new CinemaHall { HallsID = 2, CinemaID = 2, HallName = "Sal 1" }, new CinemaHall { HallsID = 3, CinemaID = 3, HallName = "Sal 1" }, new CinemaHall { HallsID = 4, CinemaID = 4, HallName = "Sal 1"}, new CinemaHall { HallsID = 5, CinemaID = 5, HallName = "Sal 1"}, new CinemaHall { HallsID = 6, CinemaID = 4, HallName = "Sal 2"}, new CinemaHall { HallsID = 7, CinemaID = 11, HallName = "Sal 2"});

            //Seats Sheet

            var seats = new List<Seat>();
            var HallIds = new List<int> { 1, 2, 3, 4, 5, 6, 7 };
            int seatId = 1;

            foreach (var hallId in HallIds)
            {
                char row = 'A';
                int seatNumber = 1;

                for (int i = 1; i <= 200; i++)
                {
                    if (seatNumber > 20)  // hvis der er mere end 20 seat på en række går den til næste
                    {
                        seatNumber = 1;
                        row++;  //Den næste i Alphabet
                    }
                    seats.Add(new Seat { SeatID = seatId++, HallID = hallId, SeatRow = row, SeatNumber = seatNumber++ });
                }
            }
            modelBuilder.Entity<Seat>().HasData(seats.ToList());

            //Show Sheet
            List<Show> shows = new List<Show>();
            Random rnd = new Random();

            DateTime startDate = DateTime.Today;
            DateTime endDate = startDate.AddDays(11);

            // Initialize id to a negative value
            int id = 1;

            foreach (int hallId in HallIds)
            {
                for (int movieId = 1; movieId <= 8; movieId++)
                {
                    for (DateTime date = startDate; date < endDate; date = date.AddDays(1))
                    {
                        int numberOfShows = rnd.Next(1, 4);

                        for (int i = 0; i < numberOfShows; i++)
                        {
                            int hour = rnd.Next(8, 23);

                            shows.Add(new Show { ShowID = id++, HallID = hallId, MovieID = movieId, ShowDateTime = new DateTime(date.Year, date.Month, date.Day, hour, 0, 0), Price = 100 });
                        }
                    }

                }
            }
            modelBuilder.Entity<Show>().HasData(shows.ToArray());


            //Role Sheet
            modelBuilder.Entity<Roles>().HasData(new Roles { RoleID = 1, RoleName = "Costumer" }, new Roles { RoleID = 2, RoleName = "Admin" });

            // UserDetail Sheet
            var UserDetails = new List<UserDetail>();
            var hashingService = new HashingService();
            var passwords = new List<string> { "Test123!", "Test321!", "Test0987654!" };
            
            int UserDetailID = 1;  // Start UserDetail IDs from 1
            
            string AdmSalt = hashingService.GenerateSalt();
            string AdmHash = hashingService.HashPassword(passwords[1], AdmSalt);

            modelBuilder.Entity<UserDetail>().HasData(new UserDetail { UserDetailID = 10, Name = "AdminGod", Email = "TestAdmin@gmail.com", PasswordHash = AdmHash, PasswordSalt = AdmSalt, PhoneNumber = "56895423", RoleID = 2, IsActive = true });
            
            foreach (var password in passwords)
            {
                string salt = hashingService.GenerateSalt();
                string hash = hashingService.HashPassword(password, salt);

                UserDetails.Add(new UserDetail
                {
                    UserDetailID = UserDetailID++,
                    Name = "Lucas" + (UserDetailID),
                    Email = $"test{UserDetailID}@example.com",
                    PasswordHash = hash,
                    PasswordSalt = salt,
                    PhoneNumber = "12345789" + (UserDetailID).ToString(),
                    RoleID = 1,
                    IsActive = true
                });
            }

            modelBuilder.Entity<UserDetail>().HasData(UserDetails);
            
            //Booking Sheet
            //modelBuilder.Entity<Booking>().HasData(new Booking { BookingID = 1, ShowID = 1, UserDetailID = 1, Price = 125, NumberOfSeats = 8, IsActive = true }, new Booking { BookingID = 2, ShowID = 2, UserDetailID = 2, Price = 110, NumberOfSeats = 12, IsActive = true }, new Booking { BookingID = 3, ShowID = 3, UserDetailID = 3, Price = 100, NumberOfSeats = 6, IsActive = true }); ;

            //BookingSeat Sheet

        }
    }
}
