using kitapsin.Server.Dto;
using kitapsin.Server.Exceptions;
using kitapsin.Server.Models;
using kitapsin.Server.Repositories;
using kitapsin.Server.Services;
using Moq;

namespace kitapsin.Tests.ServiceTests
{
    public class PublisherServiceTest
    {
        private readonly Mock<IPublisherRepository> _publisherRepositoryMock;
        private readonly PublisherService _publisherService;

        public PublisherServiceTest()
        {
            _publisherRepositoryMock = new Mock<IPublisherRepository>();
            _publisherService = new PublisherService(_publisherRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllPublishersAsync_ReturnsAllPublishers()
        {
            // Arrange
            var publishers = new List<Publisher>
                {
                    new Publisher { Id = 1, Name = "Yayıncı 1" },
                    new Publisher { Id = 2, Name = "Yayıncı 2" }
                };
            _publisherRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(publishers);

            // Act
            var result = (await _publisherService.GetAllAsync()).ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Yayıncı 1", result[0].Name);
        }

        [Fact]
        public async Task GetPublisherByIdAsync_ReturnsPublisher_WhenFound()
        {
            // Arrange
            var publisher = new Publisher { Id = 1, Name = "Yayıncı 1" };
            _publisherRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(publisher);

           
            var result = await _publisherService.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Yayıncı 1", result.Name);
        }

        [Fact]
        public async Task GetPublisherByIdAsync_ThrowsException_WhenNotFound()
        {
            // Arrange
            _publisherRepositoryMock
        .Setup(r => r.GetByIdAsync(99))
        .ThrowsAsync(new MyCustomException("Publisher bulunamadı"));

            // Act & Assert
            await Assert.ThrowsAsync<MyCustomException>(() => _publisherService.GetByIdAsync(99));
        }

        [Fact]
        public async Task AddPublisherAsync_CallsRepository()
        {
            // Arrange
            var dto = new DtoPublisherCreate { Name = "Yeni Yayıncı" };
            var publisher = new Publisher { Id = 1, Name = dto.Name };
            _publisherRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Publisher>())).Returns(Task.CompletedTask);
            _publisherRepositoryMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            await _publisherService.CreateAsync(dto);

            // Assert
            _publisherRepositoryMock.Verify(r => r.AddAsync(It.Is<Publisher>(p => p.Name == dto.Name)), Times.Once);
            _publisherRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeletePublisherAsync_CallsRepository()
        {
            // Arrange
            var publisherId = 1;
            var publisher = new Publisher { Id = publisherId, Name = "Yayıncı 1" };
            _publisherRepositoryMock.Setup(r => r.GetByIdAsync(publisherId)).ReturnsAsync(publisher);
            _publisherRepositoryMock.Setup(r => r.DeleteAsync(It.IsAny<Publisher>())).Returns(Task.CompletedTask);
            _publisherRepositoryMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            await _publisherService.DeleteAsync(publisherId);

            // Assert
            _publisherRepositoryMock.Verify(r => r.DeleteAsync(It.Is<Publisher>(p => p.Id == publisherId)), Times.Once);
            _publisherRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }
    }
}
