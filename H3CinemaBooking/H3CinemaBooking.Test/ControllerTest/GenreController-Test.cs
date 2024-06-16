using H3CinemaBooking.API.Controllers;
using H3CinemaBooking.Repository.Interfaces;
using H3CinemaBooking.Repository.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace H3CinemaBooking.Test.ControllerTest
{
    public class GenreController_Test
    {
        private readonly Mock<IGenreRepository> _mockRepo;
        private readonly GenreController _controller;

        public GenreController_Test()
        {
            _mockRepo = new Mock<IGenreRepository>();
            _controller = new GenreController(_mockRepo.Object);
        }

        [Fact]
        public void GetAll_ReturnsOk_WhenGenresExist()
        {
            // Arrange
            var genres = new List<Genre>
            {
                new Genre { GenreID = 1, GenreName = "TestGenre1" },
                new Genre { GenreID = 2, GenreName = "TestGenre2" }
            };
            _mockRepo.Setup(repo => repo.GetAll()).Returns(genres);

            // Act
            var result = _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Genre>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public void GetAll_ReturnsNoContent_WhenNoGenresExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAll()).Returns(new List<Genre>());

            // Act
            var result = _controller.GetAll();

            // Assert
            Assert.IsType<NoContentResult>(result.Result);
        }

        [Fact]
        public void GetById_ReturnsOk_WhenGenreExists()
        {
            // Arrange
            var genre = new Genre { GenreID = 1, GenreName = "TestGenre" };
            _mockRepo.Setup(repo => repo.GetById(1)).Returns(genre);

            // Act
            var result = _controller.GetByID(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Genre>(okResult.Value);
            Assert.Equal(1, returnValue.GenreID);
        }

        [Fact]
        public void GetById_ReturnsNotFound_WhenGenreDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetById(It.IsAny<int>())).Returns((Genre)null);

            // Act
            var result = _controller.GetByID(100);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void Post_ReturnsOk_WhenGenreIsCreated()
        {
            // Arrange
            var genre = new Genre { GenreID = 1, GenreName = "TestGenre" };
            _mockRepo.Setup(repo => repo.Create(genre));

            // Act
            var result = _controller.Post(genre);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Genre>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<Genre>(okResult.Value);
            Assert.Equal(1, returnValue.GenreID);
        }

        [Fact]
        public void Post_ReturnsBadRequest_WhenGenreIsNull()
        {
            // Act
            var result = _controller.Post(null);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Genre>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Genre data is required", badRequestResult.Value);
        }

        [Fact]
        public void Update_ReturnsOk_WhenGenreIsUpdated()
        {
            // Arrange
            var genre = new Genre { GenreID = 1, GenreName = "UpdatedGenre" };
            var existingGenre = new Genre { GenreID = 1, GenreName = "TestGenre" };
            _mockRepo.Setup(repo => repo.GetById(1)).Returns(existingGenre);

            // Act
            var result = _controller.Update(1, genre);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Genre>(okResult.Value);
            Assert.Equal("UpdatedGenre", returnValue.GenreName);
        }

        [Fact]
        public void Update_ReturnsNotFound_WhenGenreDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetById(It.IsAny<int>())).Returns((Genre)null);

            // Act
            var result = _controller.Update(1, new Genre { GenreID = 1 });

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Update_ReturnsBadRequest_WhenGenreIsNull()
        {
            // Act
            var result = _controller.Update(1, null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void Delete_ReturnsOk_WhenGenreIsDeleted()
        {
            // Arrange
            var genre = new Genre { GenreID = 1, GenreName = "TestGenre" };
            _mockRepo.Setup(repo => repo.GetById(1)).Returns(genre);
            _mockRepo.Setup(repo => repo.DeleteGenreByID(1));

            // Act
            var result = _controller.Delete(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = okResult.Value.GetType().GetProperty("message").GetValue(okResult.Value, null);
            Assert.Equal("Genre deleted successfully", returnValue);
        }

        [Fact]
        public void Delete_ReturnsNotFound_WhenGenreDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetById(It.IsAny<int>())).Returns((Genre)null);

            // Act
            var result = _controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
