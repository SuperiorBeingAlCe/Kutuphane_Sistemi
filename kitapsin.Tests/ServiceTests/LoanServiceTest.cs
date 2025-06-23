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
    public class LoanServiceTest
    {
        private readonly Mock<ILoanService> _loanServiceMock;

        public LoanServiceTest()
        {
            _loanServiceMock = new Mock<ILoanService>();
        }

        [Fact]
        public async Task AddAsync_ShouldReturnDtoLoanResponse()
        {
            // Arrange
            var dtoCreate = new DtoLoanCreate
            {
                UserId = 1,
                BookId = 2,
                BookTitle = "Test Book",
                DueDate = DateTime.UtcNow.AddDays(7)
            };

            var expectedResponse = new DtoLoanResponse
            {
                Id = 1,
                BookId = 2,
                BookTitle = "Test Book",
                LoanDate = DateTime.UtcNow,
                DueDate = dtoCreate.DueDate,
                ReturnDate = null,
                
            };

            _loanServiceMock.Setup(s => s.AddAsync(dtoCreate))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _loanServiceMock.Object.AddAsync(dtoCreate);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResponse.BookId, result.BookId);
            Assert.Equal(expectedResponse.BookTitle, result.BookTitle);
            Assert.Equal(expectedResponse.DueDate, result.DueDate);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenLoanExists()
        {
            // Arrange
            int loanId = 1;
            _loanServiceMock.Setup(s => s.DeleteAsync(loanId))
                .ReturnsAsync(true);

            // Act
            var result = await _loanServiceMock.Object.DeleteAsync(loanId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenLoanDoesNotExist()
        {
            // Arrange
            int loanId = 99;
            _loanServiceMock.Setup(s => s.DeleteAsync(loanId))
                .ReturnsAsync(false);

            // Act
            var result = await _loanServiceMock.Object.DeleteAsync(loanId);

            // Assert
            Assert.False(result);
        }
    }
}
