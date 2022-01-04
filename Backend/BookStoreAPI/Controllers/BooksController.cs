using BookStoreAPI.Exceptions;
using BookStoreAPI.Models;
using BookStoreAPI.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreAPI.Controllers
{
    [Route("api/[controller]")]
    public class BooksController : Controller
    {
        private IBookService _bookService;
        private IFileService _fileService;

        public BooksController(IBookService bookService, IFileService fileService)
        {
            _bookService = bookService;
            _fileService = fileService;
        }

        // GET: api/books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookModel>>> GeAllBoksAsync(string orderBy = "Name")
        {
            try
            {
                var allBooks = await _bookService.GetAllBooksAsync(orderBy);
                return Ok(allBooks);
            }
            catch (BadRequestOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Unexpected error happened: {ex.Message}");
            }
        }

        // GET: api/books/editorial/{editorialId}
        [HttpGet("editorial/{editorialId:int}") ]
        public async Task<ActionResult<IEnumerable<BookModel>>> GetBooksAsync(int editorialId)
        {
            try
            {
                var books = await _bookService.GetBooksAsync(editorialId);
                return Ok(books);
            }
            catch(NotFoundOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Unexpected error happened: {ex.Message}");
            }
        }

        // GET: api/books/{bookId}
        [HttpGet("{bookId:int}")]
        public async Task<ActionResult<BookModel>> GetBookAsync(int bookId)
        {
            try
            {
                var book = await _bookService.GetBookAsync(bookId);
                return Ok(book);
            }
            catch(NotFoundOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Unexpected error happened: {ex.Message}");
            }
        }

        // POST: api/books/editorial/form/{editorialId:int}
        [HttpPost("editorial/form/{editorialId:int}")]
        public async Task<ActionResult<BookModel>> CreateBookAsync(int editorialId, [FromBody] BookFormModel newBook)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var file = newBook.Image;
                string imagePath = _fileService.UploadFile(file, "book");

                var createdBook = await _bookService.CreateBookAsync(editorialId, newBook);
                return CreatedAtRoute("GetBook", new { bookId = newBook.Id }, newBook);
            }
            catch (NotFoundOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Unexpected error happened: {ex.Message}");
            }
        }

        // POST: api/books/editorial/{editorialId:int}
        [HttpPost("editorial/{editorialId:int}")]
        public async Task<ActionResult<BookModel>> CreateBookAsync(int editorialId, [FromBody] BookModel newBook)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdBook = await _bookService.CreateBookAsync(editorialId, newBook);
                return CreatedAtRoute("GetBook", new { bookId = newBook.Id }, newBook);
            }
            catch (NotFoundOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Unexpected error happened: {ex.Message}");
            }
        }

        // DELETE: api/books/{bookId}
        [HttpDelete("{bookId:int}")]
        public async Task<ActionResult<DeleteModel>> DeleteBookAsync(int bookId)
        {
            try
            {
                return Ok(await _bookService.DeleteBookAsync(bookId));
            }
            catch (NotFoundOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Unexpected error happened: {ex.Message}");
            }
        }

        // POST: api/books/{bookId}/editorial/{editorialId}
        [HttpPost("{bookId}/editorial/{editorialId}")]
        public async Task<ActionResult<BookModel>> UpdateBookAsync(int bookId, int editorialId, [FromBody] BookModel updatedBook)
        {
            try
            {
                var book = await _bookService.UpdateBookAsync(editorialId, bookId, updatedBook);
                return Ok(book);
            }
            catch (NotFoundOperationException ex)
            {
                return NotFound(ex.Message); ;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Unexpected error happened: {ex.Message}");
            }
        }
    }
}
