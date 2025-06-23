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
    public class AuthorServiceTest
    {
        private readonly Mock<IAuthorService> _authorServiceMock;

        public AuthorServiceTest()
        {
            _authorServiceMock = new Mock<IAuthorService>();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAuthors()
        {
            // Arrange
            var authors = new List<DtoAuthorResponse>
                {
                    new DtoAuthorResponse { Id = 1, Name = "Yazar 1" },
                    new DtoAuthorResponse { Id = 2, Name = "Yazar 2" }
                };
            _authorServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(authors);

            // Act
            var result = await _authorServiceMock.Object.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, ((List<DtoAuthorResponse>)result).Count);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnAuthor_WhenExists()
        {
            // Arrange
            var author = new DtoAuthorResponse { Id = 1, Name = "Yazar 1" };
            _authorServiceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(author);

            // Act
            var result = await _authorServiceMock.Object.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task SearchByTitleAsync_ShouldReturnAuthors()
        {
            // Arrange
            var authors = new List<DtoAuthorResponse>
                {
                    new DtoAuthorResponse { Id = 1, Name = "Yazar 1" }
                };
            _authorServiceMock.Setup(s => s.SearchByTitleAsync("Yazar")).ReturnsAsync(authors);

            // Act
            var result = await _authorServiceMock.Object.SearchByTitleAsync("Yazar");

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedAuthor()
        {
            // Arrange
            var dto = new DtoAuthorCreate { Name = "Yeni Yazar" };
            var created = new DtoAuthorResponse { Id = 3, Name = "Yeni Yazar" };
            _authorServiceMock.Setup(s => s.CreateAsync(dto)).ReturnsAsync(created);

            // Act
            var result = await _authorServiceMock.Object.CreateAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Yeni Yazar", result.Name);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnTrue_WhenSuccess()
        {
            // Arrange
            var dto = new DtoAuthorUpdate { Name = "Güncellenmiş Yazar" };
            _authorServiceMock.Setup(s => s.UpdateAsync(1, dto)).ReturnsAsync(true);

            // Act
            var result = await _authorServiceMock.Object.UpdateAsync(1, dto);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenSuccess()
        {
            // Arrange
            _authorServiceMock.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _authorServiceMock.Object.DeleteAsync(1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetBooksByAuthorIdAsync_ShouldReturnBooks()
        {
            // Arrange
            var books = new List<DtoBookResponse>
                {
                    new DtoBookResponse { Id = 1, Title = "Kitap 1", AuthorId = 1 }
                };
            _authorServiceMock.Setup(s => s.GetBooksByAuthorIdAsync(1)).ReturnsAsync(books);

            // Act
            var result = await _authorServiceMock.Object.GetBooksByAuthorIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }
    }
}
