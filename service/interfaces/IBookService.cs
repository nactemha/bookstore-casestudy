using ecommerce.models;

namespace ecommerce.service
{
    public interface IBookService
    {
        Task<IEnumerable<BookEntity>> GetAllBooksAsync();
        Task<BookEntity> GetBookByIdAsync(int id);
        Task AddBookAsync(BookEntity book);
        Task UpdateBookAsync(BookEntity book);
        Task DeleteBookAsync(int bookId);
    }
}