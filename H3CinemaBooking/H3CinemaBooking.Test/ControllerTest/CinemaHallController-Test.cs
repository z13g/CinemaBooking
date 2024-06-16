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
    public class CinemaHallController_Test
    {
        private readonly Mock<IGenericRepository<CinemaHall>> _mockRepo;
        private readonly CinemaHallController _controller;

        public CinemaHallController_Test()
        {
            _mockRepo = new Mock<IGenericRepository<CinemaHall>>();
            _controller = new CinemaHallController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOk_WhenCinemaHallsExist()
        {
            // Arrange
            var cinemaHalls = new List<CinemaHall>
            {
                new CinemaHall { HallsID = 1, HallName = "TestHall1", CinemaID = 1 },
                new CinemaHall { HallsID = 2, HallName = "TestHall2", CinemaID = 2 }
            };
            _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(cinemaHalls);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<CinemaHall>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetAll_ReturnsNoContent_WhenNoCinemaHallsExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<CinemaHall>());

            // Act
            var result = await _controller.GetAll();

            // Assert
            Assert.IsType<NoContentResult>(result.Result);
        }

        [Fact]
        public void GetByID_ReturnsOk_WhenCinemaHallExists()
        {
            // Arrange
            var cinemaHall = new CinemaHall { HallsID = 1, HallName = "TestHall", CinemaID = 1 };
            _mockRepo.Setup(repo => repo.GetById(1)).Returns(cinemaHall);

            // Act
            var result = _controller.GetByID(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<CinemaHall>(okResult.Value);
            Assert.Equal(1, returnValue.HallsID);
        }

        [Fact]
        public void GetByID_ReturnsNotFound_WhenCinemaHallDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetById(It.IsAny<int>())).Returns((CinemaHall)null);

            // Act
            var result = _controller.GetByID(100);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void Post_ReturnsOk_WhenCinemaHallIsCreated()
        {
            // Arrange
            var cinemaHall = new CinemaHall { HallsID = 1, HallName = "TestHall", CinemaID = 1 };
            _mockRepo.Setup(repo => repo.Create(cinemaHall));

            // Act
            var result = _controller.Post(cinemaHall);

            // Assert
            var actionResult = Assert.IsType<ActionResult<CinemaHall>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<CinemaHall>(okResult.Value);
            Assert.Equal(1, returnValue.HallsID);
        }

        [Fact]
        public void Post_ReturnsBadRequest_WhenCinemaHallIsNull()
        {
            // Act
            var result = _controller.Post(null);

            // Assert
            var actionResult = Assert.IsType<ActionResult<CinemaHall>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("CinemaHall data is required", badRequestResult.Value);
        }

        [Fact]
        public void Update_ReturnsOk_WhenCinemaHallIsUpdated()
        {
            // Arrange
            var cinemaHall = new CinemaHall { HallsID = 1, HallName = "UpdatedHall", CinemaID = 2 };
            var existingCinemaHall = new CinemaHall { HallsID = 1, HallName = "TestHall", CinemaID = 1 };
            _mockRepo.Setup(repo => repo.GetById(1)).Returns(existingCinemaHall);

            // Act
            var result = _controller.Update(1, cinemaHall);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<CinemaHall>(okResult.Value);
            Assert.Equal("UpdatedHall", returnValue.HallName);
        }

        [Fact]
        public void Update_ReturnsNotFound_WhenCinemaHallDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetById(It.IsAny<int>())).Returns((CinemaHall)null);

            // Act
            var result = _controller.Update(1, new CinemaHall { HallsID = 1 });

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Update_ReturnsBadRequest_WhenCinemaHallIsNull()
        {
            // Act
            var result = _controller.Update(1, null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void Delete_ReturnsOk_WhenCinemaHallIsDeleted()
        {
            // Arrange
            var cinemaHall = new CinemaHall { HallsID = 1, HallName = "TestHall", CinemaID = 1 };
            _mockRepo.Setup(repo => repo.GetById(1)).Returns(cinemaHall);
            _mockRepo.Setup(repo => repo.DeleteById(1));

            // Act
            var result = _controller.Delete(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = okResult.Value.GetType().GetProperty("message").GetValue(okResult.Value, null);
            Assert.Equal("CinemaHall deleted successfully", returnValue);
        }

        [Fact]
        public void Delete_ReturnsNotFound_WhenCinemaHallDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetById(It.IsAny<int>())).Returns((CinemaHall)null);

            // Act
            var result = _controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
