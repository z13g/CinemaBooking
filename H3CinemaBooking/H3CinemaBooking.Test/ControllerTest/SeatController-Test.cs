using H3CinemaBooking.API.Controllers;
using H3CinemaBooking.Repository.Interfaces;
using H3CinemaBooking.Repository.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace H3CinemaBooking.Test.ControllerTest
{
    public class SeatController_Test
    {
        private readonly Mock<ISeatRepository> _mockRepo;
        private readonly SeatController _controller;

        public SeatController_Test()
        {
            _mockRepo = new Mock<ISeatRepository>();
            _controller = new SeatController(_mockRepo.Object);
        }

        [Fact]
        public void GetAll_ReturnsOk_WhenSeatsExist()
        {
            // Arrange
            var seats = new List<Seat>
            {
                new Seat { SeatID = 1, HallID = 1, SeatNumber = 1, SeatRow = 'A' },
                new Seat { SeatID = 2, HallID = 1, SeatNumber = 2, SeatRow = 'A' }
            };
            _mockRepo.Setup(repo => repo.GetAll()).Returns(seats);

            // Act
            var result = _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Seat>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public void GetAll_ReturnsNoContent_WhenNoSeatsExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAll()).Returns(new List<Seat>());

            // Act
            var result = _controller.GetAll();

            // Assert
            Assert.IsType<NoContentResult>(result.Result);
        }

        [Fact]
        public void GetByID_ReturnsOk_WhenSeatExists()
        {
            // Arrange
            var seat = new Seat { SeatID = 1, HallID = 1, SeatNumber = 1, SeatRow = 'A' };
            _mockRepo.Setup(repo => repo.GetById(1)).Returns(seat);

            // Act
            var result = _controller.GetByID(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Seat>(okResult.Value);
            Assert.Equal(1, returnValue.SeatID);
        }

        [Fact]
        public void GetByID_ReturnsNotFound_WhenSeatDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetById(It.IsAny<int>())).Returns((Seat)null);

            // Act
            var result = _controller.GetByID(100);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void Post_ReturnsOk_WhenSeatIsCreated()
        {
            // Arrange
            var seat = new Seat { SeatID = 1, HallID = 1, SeatNumber = 1, SeatRow = 'A' };
            _mockRepo.Setup(repo => repo.Create(seat));

            // Act
            var result = _controller.Post(seat);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Seat>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<Seat>(okResult.Value);
            Assert.Equal(1, returnValue.SeatID);
        }

        [Fact]
        public void Post_ReturnsBadRequest_WhenSeatIsNull()
        {
            // Act
            var result = _controller.Post(null);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Seat>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Seat data is required", badRequestResult.Value);
        }

        [Fact]
        public void PostBulk_ReturnsOk_WhenSeatsAreCreated()
        {
            // Arrange
            var seats = new List<Seat>
            {
                new Seat { SeatID = 1, HallID = 1, SeatNumber = 1, SeatRow = 'A' },
                new Seat { SeatID = 2, HallID = 1, SeatNumber = 2, SeatRow = 'A' }
            };
            _mockRepo.Setup(repo => repo.CreateBulk(seats));

            // Act
            var result = _controller.PostBulk(seats);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Seat>>(actionResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public void PostBulk_ReturnsBadRequest_WhenSeatsAreNull()
        {
            // Act
            var result = _controller.PostBulk(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Seat data is required", badRequestResult.Value);
        }

        [Fact]
        public void Delete_ReturnsOk_WhenSeatIsDeleted()
        {
            // Arrange
            var seat = new Seat { SeatID = 1, HallID = 1, SeatNumber = 1, SeatRow = 'A' };
            _mockRepo.Setup(repo => repo.GetById(1)).Returns(seat);
            _mockRepo.Setup(repo => repo.DeleteByID(1));

            // Act
            var result = _controller.Delete(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = okResult.Value.GetType().GetProperty("message").GetValue(okResult.Value, null);
            Assert.Equal("Seat deleted successfully", returnValue);
        }

        [Fact]
        public void Delete_ReturnsNotFound_WhenSeatDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetById(It.IsAny<int>())).Returns((Seat)null);

            // Act
            var result = _controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Update_ReturnsOk_WhenSeatIsUpdated()
        {
            // Arrange
            var seat = new Seat { SeatID = 1, HallID = 1, SeatNumber = 1, SeatRow = 'B' };
            var existingSeat = new Seat { SeatID = 1, HallID = 1, SeatNumber = 1, SeatRow = 'A' };
            _mockRepo.Setup(repo => repo.GetById(1)).Returns(existingSeat);
            _mockRepo.Setup(repo => repo.UpdateByID(1, seat)).Returns(seat);

            // Act
            var result = _controller.Update(seat);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Seat>(okResult.Value);
            Assert.Equal('B', returnValue.SeatRow);
        }

        [Fact]
        public void Update_ReturnsNotFound_WhenSeatDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.UpdateByID(It.IsAny<int>(), It.IsAny<Seat>())).Throws(new ArgumentException("Seat not found."));

            // Act
            var result = _controller.Update(new Seat { SeatID = 1, HallID = 1, SeatNumber = 1, SeatRow = 'A' });

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Seat not found.", notFoundResult.Value);
        }

        [Fact]
        public void Update_ReturnsBadRequest_WhenSeatIsNull()
        {
            // Act
            var result = _controller.Update(null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }
    }
}