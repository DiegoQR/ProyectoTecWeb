using AutoMapper;
using BookStoreAPI.Data.Entities;
using BookStoreAPI.Data.Repositories;
using BookStoreAPI.Exceptions;
using BookStoreAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreAPI.Service
{
    public class BookService : IBookService
    {
        private IBookStoreRepository _bookStoreRepository;
        private IMapper _mapper;

        private HashSet<string> allowedOrderByParameters = new HashSet<string>()
        {
             "id",
            "name",
            "genre",
            "author",
            "price",
            "quantitySold"
        };

        public BookService(IBookStoreRepository bookStoreRepository, IMapper mapper)
        {
            _bookStoreRepository = bookStoreRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookModel>> GetAllBooksAsync(string orderBy)
        {
            if (!allowedOrderByParameters.Contains(orderBy.ToLower()))
            {
                throw new BadRequestOperationException($"the field: {orderBy} is not supported, please use one of these {string.Join(",", allowedOrderByParameters)}");
            }
            var allBooks = await _bookStoreRepository.GetAllBooksAsync(orderBy);

            return _mapper.Map<IEnumerable<BookModel>>(allBooks);
        }

        public async Task<IEnumerable<BookModel>> GetBooksAsync(int editorialId)
        {
            await ValidateEditorialAsync(editorialId);
            var books = await _bookStoreRepository.GetBooksAsync(editorialId);
            return _mapper.Map<IEnumerable<BookModel>>(books);
        }

        public async Task<BookModel> GetBookAsync(int bookId)
        {
            await ValidateBookAsync(bookId);
            var book = await _bookStoreRepository.GetBookAsync(bookId);

            var bookModel = _mapper.Map<BookModel>(book);

            bookModel.EditorialId = book.Editorial.Id;
            return bookModel;
        }

        public async Task<BookModel> CreateBookAsync(int editorialId, BookModel newBook)
        {
            await ValidateEditorialAsync(editorialId);
            newBook.EditorialId = editorialId;
            var bookEntity = _mapper.Map<BookEntity>(newBook);

            _bookStoreRepository.CreateBook(bookEntity);

            var result = await _bookStoreRepository.SaveChangesAsync();

            if (!result)
            {
                throw new Exception("Database Error");
            }

            return _mapper.Map<BookModel>(bookEntity);
        }

        public async Task<DeleteModel> DeleteBookAsync(int bookId)
        {
            await ValidateBookAsync(bookId);

            var deleteResult = await _bookStoreRepository.DeleteBookAsync(bookId);
            var saveResult = await _bookStoreRepository.SaveChangesAsync();


            if (!saveResult || !deleteResult)
            {
                throw new Exception("Database Error");
            }


            if (saveResult)
            {
                return new DeleteModel()
                {
                    IsSuccess = saveResult,
                    Message = "The book was deleted."
                };
            }
            else
            {
                return new DeleteModel()
                {
                    IsSuccess = saveResult,
                    Message = "The book was not deleted."
                };
            }
        }

        public async Task<BookModel> UpdateBookAsync(int editorialId, int bookId, BookModel BookUpdated)
        {
            await ValidateBookAsync(bookId);
            await ValidateEditorialAsync(editorialId);

            BookUpdated.Id = bookId;
            BookUpdated.EditorialId = editorialId;

            var bookEntity = _mapper.Map<BookEntity>(BookUpdated);
            _bookStoreRepository.UpdateBook(bookId, bookEntity);

            var result = await _bookStoreRepository.SaveChangesAsync();
            if (!result)
            {
                throw new Exception("Database Error");
            }

            return BookUpdated;
        }

        private async Task ValidateEditorialAsync(int editorialId)
        {
            var editorial = await _bookStoreRepository.GetEditorialAsync(editorialId, false);
            if (editorial == null)
            {
                throw new NotFoundOperationException($"The editorial with id: {editorialId} does not exists.");
            }
        }

        private async Task ValidateBookAsync(int playerId)
        {
            var editorial = await _bookStoreRepository.GetBookAsync(playerId);
            if (editorial == null)
            {
                throw new NotFoundOperationException($"The book with id: {playerId} does not exists.");
            }
        }

       
    }
}
