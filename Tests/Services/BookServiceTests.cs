using ecommerce.data;
using ecommerce.models;
using ecommerce.service;
using Moq;
using Xunit;

namespace ecommerce.Tests.services{
public class BookServiceTests
{
    private Mock<IBookRepository> _mockRepo;
    private BookService _service;

    public BookServiceTests()
    {
        _mockRepo = new Mock<IBookRepository>();
        _service = new BookService(_mockRepo.Object);
    }

    [Fact]
    public async Task GetAllBooksAsync_ReturnsListOfBooks()
    {
        // Arrange
        var books = new List<BookEntity> { new BookEntity { Id = 1, Title = "Book1" }, new BookEntity { Id = 2, Title = "Book2" } };
        _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(books);

        // Act
        var result = await _service.GetAllBooksAsync();

        // Assert
        Assert.Equal(books, result);
    }

}
}