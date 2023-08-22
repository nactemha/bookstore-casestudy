using ecommerce.service;
using ecommerce.extension;
using ecommerce.models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ecommerce.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly ILogger<BooksController> _logger;
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService, ILogger<BooksController> logger)
        {
            _bookService = bookService;
            _logger = logger;
        }

        //summary
        // Get all books
        //summary
        //remarks
        // Get all books
        //remarks
        //response code="200" Get all books success
        //response code="500" Internal Server Error
        // GET: api/books
        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            try
            {
                var books = await _bookService.GetAllBooksAsync();
                return Ok(books);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error while getting all books");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Get book by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Get book by id success</response>
        /// <response code="404">Book not found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            try
            {
                var book = await _bookService.GetBookByIdAsync(id);
                if (book == null)
                {
                    return NotFound();
                }
                return Ok(book);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error while getting book by id");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Add book
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        /// <response code="201">Add book success</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        public async Task<IActionResult> AddBook(BookModel book)
        {
            try
            {
                await _bookService.AddBookAsync(book);
                return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error while adding book");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Update book
        /// </summary>
        /// <param name="id"></param>
        /// <param name="book"></param>
        /// <response code="204">Update book success</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateBook(int id, BookModel book)
        {
            try
            {
                if (id != book.Id)
                {
                    return BadRequest();
                }
                await _bookService.UpdateBookAsync(book);
                return NoContent();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error while updating book");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }

        }

        /// <summary>
        /// Delete book
        /// </summary>
        /// <param name="id"></param>
        /// <response code="204">Delete book success</response>
        /// <response code="500">Internal Server Error</response>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                await _bookService.DeleteBookAsync(id);
                return NoContent();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error while deleting book");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

    }

}


