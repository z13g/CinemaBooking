using H3CinemaBooking.API.Controllers;
using H3CinemaBooking.Repository.Interfaces;
using H3CinemaBooking.Repository.Models;
using H3CinemaBooking.Repository.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace H3CinemaBooking.Test.ControllerTest
{
    public class ShowController_Test
    {
        private readonly Mock<IShowRepository> _mockRepo;
        private readonly Mock<IShowService>_mockService;
        private readonly ShowController _controller;

        public ShowController_Test()
        {
            _mockRepo = new Mock<IShowRepository>();
            _mockService = new Mock<IShowService>();
            _controller = new ShowController(_mockRepo.Object, _mockService.Object);
        }

        [Fact]
        public void GetAll_ReturnsOk_WhenShowExist()
        {
            // Arrange
            DateTime currentDateTime = DateTime.Now;
            //Make a new list of booking
            var shows = new List<Show>
            {
                new Show { ShowID = 1, HallID = 1, MovieID = 1, Price = 135, ShowDateTime = currentDateTime, Bookings = new List<Booking>() },
                new Show { ShowID = 2, HallID = 1, MovieID = 2, Price = 150, ShowDateTime = currentDateTime, Bookings = new List<Booking>() }
            };
            _mockRepo.Setup(repo => repo.GetAll()).Returns(shows);

            // Act
            var result = _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Show>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public void GetAll_ReturnsNoContent_WhenNoShowsExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAll()).Returns(new List<Show>());

            // Act
            var result = _controller.GetAll();

            // Assert
            Assert.IsType<NoContentResult>(result.Result);
        }

        [Fact]
        public void GetByID_ReturnsOk_WhenShowExists()
        {
            // Arrange
            DateTime currentDateTime = DateTime.Now;

            var show = new Show { ShowID = 1, HallID = 1, MovieID = 2, Price = 150, ShowDateTime = currentDateTime, Bookings = new List<Booking>() };
            _mockRepo.Setup(repo => repo.GetById(1)).Returns(show);

            // Act
            var result = _controller.GetByID(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Show>(okResult.Value);
            Assert.Equal(1, returnValue.ShowID);
        }

        [Fact]
        public void GetByID_ReturnsNotFound_WhenShowDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetById(It.IsAny<int>())).Returns((Show)null);

            // Act
            var result = _controller.GetByID(100);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        //Book Info

        public void GetBookInfo_ReturnsOk_WhenShowIdExists()
        {
            // Arrange
            DateTime currentDateTime = DateTime.Now;
            var bookShow = new BookShow { showId = 1, HallName = "Hall", CinemaName = "Cinema", Movie = new Movie(), ShowDateTime = currentDateTime, Seats = new List<SeatDTO>(), Price = 200 };

            _mockService.Setup(service => service.SetBookShowObjekt(1)).Returns(bookShow);

            // Act
            var result = _controller.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<BookShow>(okResult.Value);
            Assert.Equal(1, returnValue.showId);
        }

        [Fact]
        public void GetBookInfo_ReturnsNotFound_WhenShowIdDoesNotExist()
        {
            // Arrange
            //_mockRepo.Setup(repo => repo.GetById(It.IsAny<int>())).Returns((Show)null);
            _mockService.Setup(service => service.SetBookShowObjekt(It.IsAny<int>())).Returns((BookShow)null);

            // Act
            var result = _controller.Get(100);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void Post_ReturnsOk_WhenShowIsCreated()
        {
            // Arrange
            DateTime currentDateTime = DateTime.Now;
            var show = new Show { ShowID = 1, HallID = 1, MovieID = 2, Price = 150, ShowDateTime = currentDateTime, Bookings = new List<Booking>() };
            _mockRepo.Setup(repo => repo.Create(show));

            // Act
            var result = _controller.Post(show);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Show>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<string>(okResult.Value);
            Assert.Equal("Show created successfully", returnValue);
        }

        [Fact]
        public void Post_ReturnsBadRequest_WhenShowIsNull()
        {
            // Act
            var result = _controller.Post(null);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Show>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Show data is required", badRequestResult.Value);
        }

        [Fact]
        public void Update_ReturnsOk_WhenShowIsUpdated()
        {
            // Arrange
            DateTime currentDateTime = DateTime.Now;

            var show = new Show { ShowID = 1, HallID = 1, MovieID = 2, Price = 150, ShowDateTime = currentDateTime, Bookings = new List<Booking>() };
            var existingShow = new Show { ShowID = 1, HallID = 2, MovieID = 3, Price = 300, ShowDateTime = currentDateTime, Bookings = new List<Booking>() };
            _mockRepo.Setup(repo => repo.GetById(1)).Returns(existingShow);

            // Act
            var result = _controller.Update(1, show);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Show>(okResult.Value);
            Assert.Equal(1, returnValue.HallID);
        }

        [Fact]
        public void Update_ReturnsNotFound_WhenShowDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetById(It.IsAny<int>())).Returns((Show)null);

            // Act
            var result = _controller.Update(1, new Show { ShowID = 1 });

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Update_ReturnsBadRequest_WhenShowIsNull()
        {
            // Act
            var result = _controller.Update(1, null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void Delete_ReturnsOk_WhenShowIsDeleted()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.DeleteByID(1)).Returns(true);

            // Act
            var result = _controller.Delete(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = okResult.Value.GetType().GetProperty("message").GetValue(okResult.Value, null);
            Assert.Equal("Show deleted successfully", returnValue);
        }

        [Fact]
        public void Delete_ReturnsNotFound_WhenShowDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.DeleteByID(It.IsAny<int>())).Returns(false);

            // Act
            var result = _controller.Delete(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("ID was not found!", notFoundResult.Value);
        }
    }
}
