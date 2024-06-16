using H3CinemaBooking.API.Controllers;
using H3CinemaBooking.Repository.Interfaces;
using H3CinemaBooking.Repository.Models;
using H3CinemaBooking.Repository.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace H3CinemaBooking.Test.ControllerTest
{
    public class BookingController_Test
    {
        private readonly Mock<IBookingRepository> _mockBookingRepo;
        private readonly Mock<IBookingService> _mockBookingService;
        private readonly Mock<IJWTokenService> _mockJWTokenService;
        private readonly BookingController _controller;

        public BookingController_Test()
        {
            _mockBookingRepo = new Mock<IBookingRepository>();
            _mockBookingService = new Mock<IBookingService>();
            _mockJWTokenService = new Mock<IJWTokenService>();
            _controller = new BookingController(_mockBookingRepo.Object, _mockBookingService.Object, _mockJWTokenService.Object);
        }

        [Fact]
        public void GetAll_ReturnsOk_WhenBookingsExist()
        {
            // Arrange
            var bookings = new List<Booking>
            {
                new Booking { BookingID = 1, ShowID = 1, UserDetailID = 1, NumberOfSeats = 2, Price = 20.0, IsActive = true },
                new Booking { BookingID = 2, ShowID = 2, UserDetailID = 2, NumberOfSeats = 3, Price = 30.0, IsActive = true }
            };
            _mockBookingRepo.Setup(repo => repo.GetAll()).Returns(bookings);

            // Act
            var result = _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Booking>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public void GetAll_ReturnsNoContent_WhenNoBookingsExist()
        {
            // Arrange
            _mockBookingRepo.Setup(repo => repo.GetAll()).Returns(new List<Booking>());

            // Act
            var result = _controller.GetAll();

            // Assert
            Assert.IsType<NoContentResult>(result.Result);
        }

        [Fact]
        public void GetByID_ReturnsOk_WhenBookingExists()
        {
            // Arrange
            var booking = new Booking { BookingID = 1, ShowID = 1, UserDetailID = 1, NumberOfSeats = 2, Price = 20.0, IsActive = true };
            _mockBookingRepo.Setup(repo => repo.GetById(1)).Returns(booking);

            // Act
            var result = _controller.GetByID(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Booking>(okResult.Value);
            Assert.Equal(1, returnValue.BookingID);
        }

        [Fact]
        public void GetByID_ReturnsNotFound_WhenBookingDoesNotExist()
        {
            // Arrange
            _mockBookingRepo.Setup(repo => repo.GetById(It.IsAny<int>())).Returns((Booking)null);

            // Act
            var result = _controller.GetByID(100);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void Post_ReturnsOk_WhenBookingIsCreated()
        {
            // Arrange
            var booking = new Booking { BookingID = 1, ShowID = 1, UserDetailID = 1, NumberOfSeats = 2, Price = 20.0, IsActive = true };
            _mockBookingRepo.Setup(repo => repo.Create(booking));

            // Act
            var result = _controller.Post(booking);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Booking>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<string>(okResult.Value);
            Assert.Equal("Booking created successfully.", returnValue);
        }

        [Fact]
        public void Post_ReturnsBadRequest_WhenBookingIsNull()
        {
            // Act
            var result = _controller.Post((Booking)null);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Booking>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Booking data is required", badRequestResult.Value);
        }

        [Fact]
        public void PostReserve_ReturnsOk_WhenReservationIsCreated()
        {
            // Arrange
            var reserveSeatDTO = new ReserveSeatDTO
            {
                ShowID = 1,
                UserID = 1,
                SeatList = new List<SeatReservationStatusDTO> { new SeatReservationStatusDTO { SeatID = 1 }, new SeatReservationStatusDTO { SeatID = 2 } }
            };

            // Act
            var result = _controller.Post(reserveSeatDTO);

            // Assert
            var actionResult = Assert.IsType<ActionResult<ReserveSeatDTO>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<string>(okResult.Value);
            Assert.Equal("Reservation created successfully.", returnValue);
        }

        [Fact]
        public void PostReserve_ReturnsBadRequest_WhenReservationIsNull()
        {
            // Act
            var result = _controller.Post((ReserveSeatDTO)null);

            // Assert
            var actionResult = Assert.IsType<ActionResult<ReserveSeatDTO>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Reservation data is required", badRequestResult.Value);
        }

        [Fact]
        public void PostReserve_ReturnsBadRequest_OnException()
        {
            // Arrange
            var reserveSeatDTO = new ReserveSeatDTO
            {
                ShowID = 1,
                UserID = 1,
                SeatList = new List<SeatReservationStatusDTO> { new SeatReservationStatusDTO { SeatID = 1 }, new SeatReservationStatusDTO { SeatID = 2 } }
            };

            _mockBookingService.Setup(service => service.ReserveSeats(It.IsAny<ReserveSeatDTO>())).Throws(new ArgumentException("Error"));

            // Act
            var result = _controller.Post(reserveSeatDTO);

            // Assert
            var actionResult = Assert.IsType<ActionResult<ReserveSeatDTO>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Error", badRequestResult.Value);
        }

        [Fact]
        public void Delete_ReturnsOk_WhenBookingIsDeleted()
        {
            // Arrange
            _mockBookingRepo.Setup(repo => repo.DeleteByID(1)).Returns(true);

            // Act
            var result = _controller.Delete(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = okResult.Value.GetType().GetProperty("message").GetValue(okResult.Value, null);
            Assert.Equal("Booking deleted successfully", returnValue);
        }

        [Fact]
        public void Delete_ReturnsNotFound_WhenBookingDoesNotExist()
        {
            // Arrange
            _mockBookingRepo.Setup(repo => repo.DeleteByID(It.IsAny<int>())).Returns(false);

            // Act
            var result = _controller.Delete(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("ID was not found!", notFoundResult.Value);
        }
    }
}
