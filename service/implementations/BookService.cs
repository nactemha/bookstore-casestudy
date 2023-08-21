

using ecommerce.data;
using ecommerce.models;

namespace ecommerce.service
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }
        
        public async Task<IEnumerable<BookEntity>> GetAllBooksAsync()
        {

            return await _bookRepository.GetAllAsync();

        }

        public async Task<BookEntity> GetBookByIdAsync(int id)
        {
            return await _bookRepository.GetByIdAsync(id);
        }

        public async Task AddBookAsync(BookEntity book)
        {
            await _bookRepository.AddAsync(book);
            await _bookRepository.SaveChangesAsync();
        }

        public async Task UpdateBookAsync(BookEntity book)
        {
            _bookRepository.Update(book);
            await _bookRepository.SaveChangesAsync();
        }

        public async Task DeleteBookAsync(int bookId)
        {
            var book = await _bookRepository.GetByIdAsync(bookId);
            if (book == null)
            {
                throw new InvalidOperationException("Book not found.");
            }
            _bookRepository.Delete(book.Id);
            await _bookRepository.SaveChangesAsync();
        }
    }
}