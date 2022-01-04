using BookStoreAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreAPI.Service
{
    public interface IBookService
    {
        public Task<IEnumerable<BookModel>> GetAllBooksAsync(string orderBy);
        public Task<IEnumerable<BookModel>> GetBooksAsync(int editorialId);
        public Task<BookModel> GetBookAsync(int bookId);
        public Task<BookModel> CreateBookAsync(int editorialId, BookModel newBook);
        public Task<DeleteModel> DeleteBookAsync(int bookId);
        public Task<BookModel> UpdateBookAsync(int editorialId, int bookId, BookModel BookUpdated);
    }
}
