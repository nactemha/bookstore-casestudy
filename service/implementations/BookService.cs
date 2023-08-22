

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
        
        public async Task<IEnumerable<BookModel>> GetAllBooksAsync()
        {

            var result= await _bookRepository.GetAllAsync();
            return result.Select(b => new BookModel
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Price = b.Price,
            });

        }

        public async Task<BookModel> GetBookByIdAsync(int id)
        {
            var result= await _bookRepository.GetByIdAsync(id);
            return new BookModel
            {
                Id = result.Id,
                Title = result.Title,
                Author = result.Author,
                Price = result.Price,
            };
        }

        public async Task AddBookAsync(BookModel book)
        {
            var requestedBook =new BookEntity{
                Title = book.Title,
                Author = book.Author,
                Price = book.Price,
            };
            await _bookRepository.AddAsync(requestedBook);
            await _bookRepository.SaveChangesAsync();
        }

        public async Task UpdateBookAsync(BookModel book)
        {
            var requestedBook = await _bookRepository.GetByIdAsync(book.Id);
            if (requestedBook == null)
            {
                throw new InvalidOperationException("Book not found.");
            }
            requestedBook.Title = book.Title;
            requestedBook.Author = book.Author;
            requestedBook.Price = book.Price;
            _bookRepository.Update(requestedBook);
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