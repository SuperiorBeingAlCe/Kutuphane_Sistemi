using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kitapsin.Server.Dto;
using kitapsin.Server.Services;
using Moq;

namespace kitapsin.Tests.ServiceTests
{
    public class PenaltyServiceTest
    {
        private readonly Mock<IPenaltyService> _penaltyServiceMock;

        public PenaltyServiceTest()
        {
            _penaltyServiceMock = new Mock<IPenaltyService>();
        }

        [Fact]
        public async Task AddAsync_ShouldReturnPenaltyResponse()
        {
            // Arrange
            var dto = new DtoPenaltyCreate
            {
                UserId = 1,
                Reason = "Geç teslim",
                Amount = 50
            };
            var expectedResponse = new DtoPenaltyResponse
            {
                Id = 1,
                Reason = "Geç teslim",
                Amount = 50,
                IssuedAt = System.DateTime.Now,
                IsPaid = false
            };

            _penaltyServiceMock.Setup(s => s.AddAsync(dto))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _penaltyServiceMock.Object.AddAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResponse.Id, result.Id);
            Assert.Equal(expectedResponse.Reason, result.Reason);
            Assert.Equal(expectedResponse.Amount, result.Amount);
            Assert.False(result.IsPaid);
        }

        [Fact]
        public async Task PayAndRemoveAsync_ShouldReturnTrue_WhenPenaltyPaid()
        {
            // Arrange
            int penaltyId = 1;
            _penaltyServiceMock.Setup(s => s.PayAndRemoveAsync(penaltyId))
                .ReturnsAsync(true);

            // Act
            var result = await _penaltyServiceMock.Object.PayAndRemoveAsync(penaltyId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task PayAndRemoveAsync_ShouldReturnFalse_WhenPenaltyNotFound()
        {
            // Arrange
            int penaltyId = 99;
            _penaltyServiceMock.Setup(s => s.PayAndRemoveAsync(penaltyId))
                .ReturnsAsync(false);

            // Act
            var result = await _penaltyServiceMock.Object.PayAndRemoveAsync(penaltyId);

            // Assert
            Assert.False(result);
        }
    }
}
