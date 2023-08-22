using ecommerce.models;

namespace ecommerce.service
{
    public interface IBookService
    {
        Task<IEnumerable<BookModel>> GetAllBooksAsync();
        Task<BookModel> GetBookByIdAsync(int id);
        Task AddBookAsync(BookModel book);
        Task UpdateBookAsync(BookModel book);
        Task DeleteBookAsync(int bookId);
    }
}