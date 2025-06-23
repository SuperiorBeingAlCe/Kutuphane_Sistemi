using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kitapsin.Server.Controllers;
using kitapsin.Server.Dto;
using kitapsin.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace kitapsin.Tests.ControllerTests
{
    public class LoanControllerTest
    {
        private readonly Mock<ILoanService> _mockService;
        private readonly LoanController _controller;

        public LoanControllerTest()
        {
            _mockService = new Mock<ILoanService>();
            _controller = new LoanController(_mockService.Object);
        }

        [Fact]
        public async Task AddAsync_ReturnsCreatedLoan()
        {
            // Arrange
            var dto = new DtoLoanCreate
            {
                BookId = 1,
                UserId = 1,
                DueDate = DateTime.UtcNow
            };

            var response = new DtoLoanResponse
            {
                Id = 10,
                BookId = 1,
                UserId = 1,
                LoanDate = dto.DueDate,
                ReturnDate = null
            };

            _mockService.Setup(s => s.AddAsync(dto)).ReturnsAsync(response);

            // Act
            var result = await _controller.AddAsync(dto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returned = Assert.IsType<DtoLoanResponse>(createdResult.Value);
            Assert.Equal(10, returned.Id);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsNoContent_WhenSuccess()
        {
            _mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

            var result = await _controller.DeleteAsync(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsNotFound_WhenFails()
        {
            _mockService.Setup(s => s.DeleteAsync(999)).ReturnsAsync(false);

            var result = await _controller.DeleteAsync(999);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
