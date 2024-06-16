using H3CinemaBooking.API.Controllers;
using H3CinemaBooking.Repository.Interfaces;
using H3CinemaBooking.Repository.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace H3CinemaBooking.Test.ControllerTest
{
    public class CinemaController_Test
    {
        private readonly Mock<IGenericRepository<Cinema>> _mockRepo;
        private readonly CinemaController _controller;

        public CinemaController_Test()
        {
            _mockRepo = new Mock<IGenericRepository<Cinema>>();
            _controller = new CinemaController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOk_WhenCinemasExist()
        {
            // Arrange
            var cinemas = new List<Cinema> { new Cinema { CinemaID = 1, Name = "TestCinema", Location = "TestLocation", NumberOfHalls = 5, AreaID = 1 } };
            _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(cinemas);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Cinema>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task GetAll_ReturnsNoContent_WhenNoCinemasExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Cinema>());

            // Act
            var result = await _controller.GetAll();

            // Assert
            Assert.IsType<NoContentResult>(result.Result);
        }

        [Fact]
        public void GetById_ReturnsOk_WhenCinemaExists()
        {
            // Arrange
            var cinema = new Cinema { CinemaID = 1, Name = "TestCinema", Location = "TestLocation", NumberOfHalls = 5, AreaID = 1 };
            _mockRepo.Setup(repo => repo.GetById(1)).Returns(cinema);

            // Act
            var result = _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Cinema>(okResult.Value);
            Assert.Equal(1, returnValue.CinemaID);
        }

        [Fact]
        public void GetById_ReturnsNotFound_WhenCinemaDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetById(It.IsAny<int>())).Returns((Cinema)null);

            // Act
            var result = _controller.GetById(100);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void Post_ReturnsOk_WhenCinemaIsCreated()
        {
            // Arrange
            var cinema = new Cinema { CinemaID = 1, Name = "TestCinema", Location = "TestLocation", NumberOfHalls = 5, AreaID = 1 };
            _mockRepo.Setup(repo => repo.Create(cinema));

            // Act
            var result = _controller.Post(cinema);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Cinema>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<Cinema>(okResult.Value);
            Assert.Equal(1, returnValue.CinemaID);
        }

        [Fact]
        public void Post_ReturnsBadRequest_WhenCinemaIsNull()
        {
            // Act
            var result = _controller.Post(null);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Cinema>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Cinema data is required", badRequestResult.Value);
        }

        [Fact]
        public void Update_ReturnsOk_WhenCinemaIsUpdated()
        {
            // Arrange
            var cinema = new Cinema { CinemaID = 1, Name = "UpdatedCinema", Location = "UpdatedLocation", NumberOfHalls = 10, AreaID = 2 };
            var existingCinema = new Cinema { CinemaID = 1, Name = "TestCinema", Location = "TestLocation", NumberOfHalls = 5, AreaID = 1 };
            _mockRepo.Setup(repo => repo.GetById(1)).Returns(existingCinema);

            // Act
            var result = _controller.Update(1, cinema);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Cinema>(okResult.Value);
            Assert.Equal("UpdatedCinema", returnValue.Name);
        }

        [Fact]
        public void Update_ReturnsNotFound_WhenCinemaDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetById(It.IsAny<int>())).Returns((Cinema)null);

            // Act
            var result = _controller.Update(1, new Cinema { CinemaID = 1 });

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Update_ReturnsBadRequest_WhenCinemaIsNull()
        {
            // Act
            var result = _controller.Update(1, null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void Delete_ReturnsOk_WhenCinemaIsDeleted()
        {
            // Arrange
            var cinema = new Cinema { CinemaID = 1, Name = "TestCinema", Location = "TestLocation", NumberOfHalls = 5, AreaID = 1 };
            _mockRepo.Setup(repo => repo.GetById(1)).Returns(cinema);
            _mockRepo.Setup(repo => repo.DeleteById(1));

            // Act
            var result = _controller.Delete(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = okResult.Value.GetType().GetProperty("message").GetValue(okResult.Value, null);
            Assert.Equal("Cinema deleted successfully", returnValue);
        }

        [Fact]
        public void Delete_ReturnsNotFound_WhenCinemaDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetById(It.IsAny<int>())).Returns((Cinema)null);

            // Act
            var result = _controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}