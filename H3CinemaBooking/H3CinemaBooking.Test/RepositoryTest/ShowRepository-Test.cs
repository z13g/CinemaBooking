using H3CinemaBooking.Repository.Data;
using H3CinemaBooking.Repository.Interfaces;
using H3CinemaBooking.Repository.Models;
using H3CinemaBooking.Repository.Repositories;
using H3CinemaBooking.Repository.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3CinemaBooking.Test.RepositoryTest
{
    public class ShowRepository_Test
    {
        private readonly DbContextOptions<Dbcontext> options;
        private readonly Dbcontext context;
        IPropertyValidationService validationService;

        public ShowRepository_Test()
        {
            options = new DbContextOptionsBuilder<Dbcontext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            context = new Dbcontext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            validationService = new PropertyValidationService();

            // Populate initial data with some shows
            var shows = new List<Show>
            {
                new Show { ShowID = 2000, HallID = 1, MovieID = 1, Price = 130, ShowDateTime = DateTime.Now.AddDays(1) },
                new Show { ShowID = 2001, HallID = 1, MovieID = 2, Price = 1300, ShowDateTime = DateTime.Now.AddDays(2) }
            };

            context.Shows.AddRange(shows);
            context.SaveChanges();
        }

        [Fact]
        public void GetAll_ReturnsAllShows()
        {
            // Arrange
            var repository = new ShowRepository(context, validationService);

            // Act
            var results = repository.GetAll();

            // Assert
            Assert.NotNull(results);
        }

        [Fact]
        public void GetAll_WhenNoSeats_ReturnsEmptyList()
        {
            // Arrange
            var repository = new ShowRepository(context, validationService);
            context.Shows.RemoveRange(context.Shows.ToList()); // Ensure no Seat exist
            context.SaveChanges();

            // Act
            var results = repository.GetAll();

            // Assert
            Assert.NotNull(results);
            Assert.Empty(results);
        }

        [Fact]
        public void Create_AddsNewShow()
        {
            // Arrange
            var repository = new ShowRepository(context, validationService);
            var newShow = new Show { HallID = 2, MovieID = 3, Price = 130, ShowDateTime = DateTime.Now.AddDays(3) };

            // Act
            var result = repository.Create(newShow);

            // Assert
            Assert.NotNull(result);
            Assert.True(context.Shows.Any(s => s.ShowID == result.ShowID));
        }

        [Fact]
        public void Create_ThrowsException_WithInvalidProperties()
        {
            // Arrange
            var repository = new ShowRepository(context, validationService);
            var invalidShow = new Show { MovieID = 3, Price = 130, ShowDateTime = DateTime.Now.AddDays(3) };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => repository.Create(invalidShow));
            Assert.Equal("Invalid show properties.", exception.Message);
        }

        [Fact]
        public void GetById_ReturnsCorrectShow()
        {
            // Arrange
            var repository = new ShowRepository(context, validationService);

            // Act
            var result = repository.GetById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.HallID);
        }

        [Fact]
        public void GetById_WhenNoShowFound_ReturnsNull()
        {
            // Arrange
            var repository = new SeatRepository(context, validationService);

            // Act
            var result = repository.GetById(3000);

            // Assert
            Assert.Null(result); // Ensures method handles "not found" by returning null
        }

        [Fact]
        public void UpdateByID_UpdatesExistingShow()
        {
            // Arrange
            var repository = new ShowRepository(context, validationService);
            var updatedShow = new Show { ShowID = 1, HallID = 1, MovieID = 1, Price = 120, ShowDateTime = DateTime.Now.AddDays(1) };

            // Act
            repository.UpdateByID(1, updatedShow);
            var result = repository.GetById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(120, result.Price);
        }

        [Fact]
        public void UpdateByID_WithInvalidProperties_ThrowsException()
        {
            // Arrange
            var repository = new ShowRepository(context, validationService);
            var updatedShow = new Show { ShowID = 3000, MovieID = 1, Price = 120, ShowDateTime = DateTime.Now.AddDays(1) };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => repository.UpdateByID(1, updatedShow));
            Assert.Equal("Invalid show properties.", exception.Message);
        }

        [Fact]
        public void UpdateByID_WhenShowNotFound_ThrowsException()
        {
            // Arrange
            var repository = new ShowRepository(context, validationService);
            var updatedShow = new Show { ShowID = 3000, HallID = 1, MovieID = 1, Price = 120, ShowDateTime = DateTime.Now.AddDays(1) };

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => repository.UpdateByID(3000, updatedShow));
            Assert.Equal("No show found with ID 3000.", exception.Message);
        }

        [Fact]
        public void DeleteByID_RemovesShow()
        {
            // Arrange
            var repository = new ShowRepository(context, validationService);

            // Act
            var result = repository.DeleteByID(1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void DeleteByID_Throw_Exception()
        {
            // Arrange
            var repository = new ShowRepository(context, validationService);

            // Act
            var result = repository.DeleteByID(3000);

            // Assert
            Assert.False(result);
        }
    }
}
