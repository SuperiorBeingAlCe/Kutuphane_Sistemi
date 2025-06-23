
using kitapsin.Server.Controllers;
using kitapsin.Server.Dto;
using kitapsin.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace kitapsin.Tests.ControllerTests
{
    public class PenaltyControllerTest
    {
        private readonly Mock<IPenaltyService> _mockService;
        private readonly PenaltyController _controller;

        public PenaltyControllerTest()
        {
            _mockService = new Mock<IPenaltyService>();
            _controller = new PenaltyController(_mockService.Object);
        }

        [Fact]
        public async Task AddAsync_ReturnsCreatedPenalty()
        {
            // Arrange
            var createDto = new DtoPenaltyCreate { UserId = 1, Amount = 50.0m, Reason = "Geç iade" };
            var createdDto = new DtoPenaltyResponse { Id = 10, UserId = 1, Amount = 50.0m, Reason = "Geç iade" };

            _mockService.Setup(s => s.AddAsync(createDto)).ReturnsAsync(createdDto);

            // Act
            var result = await _controller.AddAsync(createDto);

            // Assert
            var actionResult = Assert.IsType<ActionResult<DtoPenaltyResponse>>(result);
            var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnValue = Assert.IsType<DtoPenaltyResponse>(createdResult.Value);

            Assert.Equal(10, returnValue.Id);
            Assert.Equal(50.0m, returnValue.Amount);
        }

        [Fact]
        public async Task PayAndRemoveAsync_ReturnsNoContent_WhenSuccess()
        {
            _mockService.Setup(s => s.PayAndRemoveAsync(5)).ReturnsAsync(true);

            var result = await _controller.PayAndRemoveAsync(5);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PayAndRemoveAsync_ReturnsNotFound_WhenFail()
        {
            _mockService.Setup(s => s.PayAndRemoveAsync(99)).ReturnsAsync(false);

            var result = await _controller.PayAndRemoveAsync(99);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
