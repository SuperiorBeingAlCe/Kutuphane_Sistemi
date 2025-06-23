using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kitapsin.Server.Controllers;
using kitapsin.Server.Dto;
using kitapsin.Server.Exceptions;
using kitapsin.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace kitapsin.Tests.ControllerTests
{
    public class UserControllerTest
    {
        private readonly Mock<IUserService> _mockService;
        private readonly UserController _controller;

        public UserControllerTest()
        {
            _mockService = new Mock<IUserService>();
            _controller = new UserController(_mockService.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllUsers()
        {
            // Arrange
            var users = new List<DtoUserResponse> { new() { Id = 1, FullName = "Superior" } };
            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(users);

            // Act
            var result = await _controller.GetAllAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(users, okResult.Value);
        }

        [Fact]
        public async Task GetByIdAsync_UserFound_ReturnsUser()
        {
            var user = new DtoUserResponse { Id = 1, FullName = "Superior" };
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(user);

            var result = await _controller.GetByIdAsync(1);

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(user, ok.Value);
        }

        [Fact]
        public async Task GetByIdAsync_UserNotFound_ReturnsNotFound()
        {
            _mockService.Setup(s => s.GetByIdAsync(99))
  .ThrowsAsync(new MyCustomException("Admin bulunamadı"));

            var result = await _controller.GetByIdAsync(99);

            // Tip kontrolü
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);

            // Mesaj kontrolü (isteğe bağlı ama sağlamlık için iyidir)
            Assert.Equal("Admin bulunamadı", notFoundResult.Value);

        }

        [Fact]
        public async Task GetByCardNumberAsync_Found_ReturnsUser()
        {
            var user = new DtoUserResponse { Id = 1, CardNumber = "ABC123" };
            _mockService.Setup(s => s.GetByCardNumberAsync("ABC123")).ReturnsAsync(user);

            var result = await _controller.GetByCardNumberAsync("ABC123");

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(user, ok.Value);
        }

        [Fact]
        public async Task GetByCardNumberAsync_NotFound_ThrowsException_ReturnsNotFound()
        {
            // Arrange
            var cardNumber = "12345";
            _mockService.Setup(s => s.GetByCardNumberAsync(cardNumber))
                .ThrowsAsync(new MyCustomException("Kullanıcı bulunamadı"));

            // Act
            var result = await _controller.GetByCardNumberAsync(cardNumber);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Kullanıcı bulunamadı", notFoundResult.Value);
        }

        [Fact]
        public async Task SearchByNameAsync_ReturnsMatchingUsers()
        {
            var list = new List<DtoUserResponse> { new() { FullName = "Superior" } };
            _mockService.Setup(s => s.SearchByNameAsync("Superior")).ReturnsAsync(list);

            var result = await _controller.SearchByNameAsync("Superior");

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(list, ok.Value);
        }

        [Fact]
        public async Task CreateAsync_ReturnsCreatedUser()
        {
            var dto = new DtoUserCreate { FullName = "Superior" };
            var created = new DtoUserResponse { Id = 42, FullName = "Superior" };

            _mockService.Setup(s => s.CreateAsync(dto)).ReturnsAsync(created);

            var result = await _controller.CreateAsync(dto);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal("GetByIdAsync", createdResult.ActionName);
            Assert.Equal(created, createdResult.Value);
        }

        [Fact]
        public async Task UpdateAsync_Success_ReturnsNoContent()
        {
            _mockService.Setup(s => s.UpdateAsync(1, It.IsAny<DtoUserUpdate>())).ReturnsAsync(true);

            var result = await _controller.UpdateAsync(1, new DtoUserUpdate());

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateAsync_Failure_ReturnsNotFound()
        {
            _mockService.Setup(s => s.UpdateAsync(1, It.IsAny<DtoUserUpdate>())).ReturnsAsync(false);

            var result = await _controller.UpdateAsync(1, new DtoUserUpdate());

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteAsync_Success_ReturnsNoContent()
        {
            _mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

            var result = await _controller.DeleteAsync(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteAsync_Failure_ReturnsNotFound()
        {
            _mockService.Setup(s => s.DeleteAsync(99)).ReturnsAsync(false);

            var result = await _controller.DeleteAsync(99);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetLoansAsync_ReturnsLoans()
        {
            var loans = new List<DtoLoanResponse> { new() { Id = 1 } };
            _mockService.Setup(s => s.GetLoansAsync(1)).ReturnsAsync(loans);

            var result = await _controller.GetLoansAsync(1);

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(loans, ok.Value);
        }

        [Fact]
        public async Task GetPenaltiesAsync_ReturnsPenalties()
        {
            var penalties = new List<DtoPenaltyResponse> { new() { Id = 1 } };
            _mockService.Setup(s => s.GetPenaltiesAsync(1)).ReturnsAsync(penalties);

            var result = await _controller.GetPenaltiesAsync(1);

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(penalties, ok.Value);
        }

        [Fact]
        public async Task GetBorrowedBooksAsync_ReturnsBooks()
        {
            var books = new List<DtoBookResponse> { new() { Id = 1 } };
            _mockService.Setup(s => s.GetBorrowedBooksAsync(1)).ReturnsAsync(books);

            var result = await _controller.GetBorrowedBooksAsync(1);

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(books, ok.Value);
        }
    }
}
