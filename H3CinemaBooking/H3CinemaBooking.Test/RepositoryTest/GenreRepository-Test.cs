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
    public class GenreRepository_Test
    {
        DbContextOptions<Dbcontext> options;
        Dbcontext context;
        IPropertyValidationService validationService;

        public GenreRepository_Test()
        {
            options = new DbContextOptionsBuilder<Dbcontext>()
                .UseInMemoryDatabase(databaseName: "DummyDatabase")
                .Options;

            context = new Dbcontext(options);
            context.Database.EnsureDeleted(); // Clear the in-memory database
            context.Database.EnsureCreated(); // Recreate the database

            validationService = new PropertyValidationService();

            // Populate initial data
            Genre g1 = new Genre() { GenreID = 5, GenreName = "Test" };
            Genre g2 = new Genre() { GenreID = 6, GenreName = "Comedy" };

            context.Genres.AddRange(g1, g2);
            context.SaveChanges();
        }

        [Fact]
        public void Create_AddsNewGenre()
        {
            // Arrange
            var repository = new GenreRepository(context, validationService);
            var newGenre = new Genre { GenreName = "Drama" };

            // Act
            var result = repository.Create(newGenre);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Drama", result.GenreName);
            Assert.True(context.Genres.Any(g => g.GenreName == "Drama"));
        }

        [Fact]
        public void Create_WithInvalidProperties_ThrowsException()
        {
            // Arrange
            var repository = new GenreRepository(context, validationService);
            var genre = new Genre { GenreName = "" }; // Invalid empty name

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => repository.Create(genre));
            Assert.Equal("Invalid genre properties.", exception.Message);
        }



        [Fact]
        public void GetById_ReturnsCorrectGenre()
        {
            // Arrange
            var repository = new GenreRepository(context, validationService);

            // Act
            var result = repository.GetById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Action", result.GenreName);
        }

        [Fact]
        public void GetById_WhenNoGenreFound_ReturnsNull()
        {
            // Arrange
            var repository = new GenreRepository(context, validationService);

            // Act
            var result = repository.GetById(999);

            // Assert
            Assert.Null(result); // Ensures method handles "not found" by returning null
        }


        [Fact]
        public void GetAll_ReturnsAllGenres()
        {
            // Arrange
            var repository = new GenreRepository(context, validationService);

            // Act
            var results = repository.GetAll();

            // Assert
            Assert.NotNull(results);
            Assert.Equal(5, results.Count); // Make sure it matches the number of genres initially added
        }

        [Fact]
        public void GetAll_WhenNoGenres_ReturnsEmptyList()
        {
            // Arrange
            var repository = new GenreRepository(context, validationService);
            context.Genres.RemoveRange(context.Genres.ToList()); // Ensure no genres exist
            context.SaveChanges();

            // Act
            var results = repository.GetAll();

            // Assert
            Assert.NotNull(results);
            Assert.Empty(results); // Checks that an empty list is returned, not null
        }


        [Fact]
        public void UpdateByID_UpdatesExistingGenre()
        {
            // Arrange
            var repository = new GenreRepository(context, validationService);
            var updatedGenre = new Genre { GenreName = "Horror" };

            // Act
            repository.UpdateByID(1, updatedGenre);
            var result = repository.GetById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Horror", result.GenreName);
        }

        [Fact]
        public void UpdateByID_WithInvalidProperties_ThrowsException()
        {
            // Arrange
            var repository = new GenreRepository(context, validationService);
            var updatedGenre = new Genre { GenreID = 1, GenreName = "" };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => repository.UpdateByID(1, updatedGenre));
            Assert.Equal("Invalid genre properties.", exception.Message);
        }

        [Fact]
        public void UpdateByID_WhenGenreNotFound_ThrowsException()
        {
            // Arrange
            var repository = new GenreRepository(context, validationService);
            var updatedGenre = new Genre { GenreID = 99, GenreName = "Sci-Fi" };

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => repository.UpdateByID(99, updatedGenre));
            Assert.Equal("No genre found with ID 99.", exception.Message);
        }

        [Fact]
        public void DeleteGenre_WhenExists()
        {
            //Arrange
            GenreRepository repository = new GenreRepository(context, validationService);
            //Act - Method calling
            var result = repository.DeleteGenreByID(1);
            Assert.True(result);
        }

        [Fact]
        public void DeleteGenre_WhenDoesNotExist()
        {
            // Arrange
            var repository = new GenreRepository(context, validationService);

            // Act
            var result = repository.DeleteGenreByID(99);

            // Assert
            Assert.False(result); // No genre deleted because it does not exist
        }


    }
}
