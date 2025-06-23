
using AutoMapper;
using kitapsin.Server.Dto;
using kitapsin.Server.Exceptions;
using kitapsin.Server.Models;
using kitapsin.Server.Repositories;
using kitapsin.Server.Services;
using Moq;

namespace kitapsin.Tests.ServiceTests
{
    public class BookServiceTest
    {
        private readonly Mock<IBookRepository> _mockBookRepo;
        private readonly Mock<IAuthorRepository> _mockAuthorRepo;
        private readonly Mock<ICategoryRepository> _mockCategoryRepo;
        private readonly BookService _bookService;
        private readonly Mock<IPublisherRepository> _mockPublisherRepo;
        public BookServiceTest()
        {
            _mockBookRepo = new Mock<IBookRepository>();
            _mockAuthorRepo = new Mock<IAuthorRepository>();
            _mockCategoryRepo = new Mock<ICategoryRepository>();
            _bookService = new BookService(
                _mockBookRepo.Object,
                _mockAuthorRepo.Object,
                _mockCategoryRepo.Object,
                  _mockPublisherRepo.Object
            );
        }

        [Fact]
        public async Task GetByIdAsync_BookExists_ReturnsMappedBook()
        {
            // Arrange
            var book = new Book { Id = 1, Title = "Test Book" };
            var response = new DtoBookResponse { Id = 1, Title = "Test Book" };

            _mockBookRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(book);

            // Act
            var result = await _bookService.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Book", result.Title);
        }

        [Fact]
        public async Task GetByIdAsync_BookDoesNotExist_ThrowsException()
        {
            // Arrange
            _mockBookRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
               .ThrowsAsync(new MyCustomException("Kitap bulunamadı."));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<MyCustomException>(() => _bookService.GetByIdAsync(999));
            Assert.Contains("Kitap bulunamadı", exception.Message);
        }

        [Fact]
        public async Task CreateAsync_InvalidTitle_ThrowsMyCustomException()
        {
            // Arrange
            var invalidDto = new DtoBookCreate
            {
                Title = string.Empty,
                AuthorId = 1,
                CategoryId = 1
            };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<MyCustomException>(() => _bookService.CreateAsync(invalidDto));
            Assert.Equal("Geçersiz kitap başlığı.", ex.Message);
        }

        [Fact]
        public async Task CreateAsync_MissingAuthor_ThrowsMyCustomException()
        {
            // Arrange
            var dto = new DtoBookCreate { Title = "Test", AuthorId = 99, CategoryId = 1 };

            var expectedMessage = $"Kitap için yazar bulunamadı. AuthorId={dto.AuthorId}";

            _mockAuthorRepo.Setup(r => r.GetByIdAsync(dto.AuthorId))
                .ThrowsAsync(new MyCustomException(expectedMessage));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<MyCustomException>(() => _bookService.CreateAsync(dto));

           
            Assert.Equal(expectedMessage, ex.Message);
        }

        [Fact]
        public async Task CreateAsync_ValidData_CreatesBookAndReturnsResponse()
        {
            // Arrange
            var dto = new DtoBookCreate
            {
                Title = "Test Book",
                AuthorId = 1,
                CategoryId = 1,
                PublisherId = 1,
                PublicationYear = 2025,
                ISBN = "ISBN123",
                Quantity = 5
            };



            // Mock author exists
            _mockAuthorRepo.Setup(r => r.GetByIdAsync(dto.AuthorId))
               .ReturnsAsync(new Author { Id = dto.AuthorId, Name = "Author Name" });
            // Mock category exists
            _mockCategoryRepo.Setup(r => r.GetByIdAsync(dto.CategoryId))
                 .ReturnsAsync(new Category { Id = dto.CategoryId, Name = "Category Name" });

            // Capture the added book
            Book? savedBook = null;
            _mockBookRepo.Setup(r => r.AddAsync(It.IsAny<Book>()))
                 .Callback<Book>(b =>
                 {
                     savedBook = b;
                     // Kritik kısım: Author ve Category objelerini set et
                     savedBook.Author = new Author { Id = dto.AuthorId, Name = "Author Name" };
                     savedBook.Category = new Category { Id = dto.CategoryId, Name = "Category Name" };
                 })
                 .Returns(Task.CompletedTask);

            _mockBookRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            _mockBookRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) =>
                {
                    if (savedBook == null)
                        throw new InvalidOperationException("savedBook is null!");
                    savedBook.Id = id;
                    return savedBook;
                });

            // Act
            var result = await _bookService.CreateAsync(dto);

            // Assert
            _mockBookRepo.Verify(r => r.AddAsync(It.IsAny<Book>()), Times.Once);
            _mockBookRepo.Verify(r => r.SaveChangesAsync(), Times.Once);

            Assert.Equal(dto.Title, result.Title);
            Assert.Equal(dto.AuthorId, result.AuthorId);
            Assert.Equal(dto.CategoryId, result.CategoryId);
            Assert.Equal(dto.PublisherId, result.PublisherId);
        }
    }
}
