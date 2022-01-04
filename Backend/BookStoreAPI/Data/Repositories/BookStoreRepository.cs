using BookStoreAPI.Data.Entities;
using BookStoreAPI.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreAPI.Data.Repositories
{
    public class BookStoreRepository : IBookStoreRepository
    {
        private BookStoreDbContext _dbContext;

        public BookStoreRepository(BookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //Editorials

        public async Task<IEnumerable<EditorialEntity>> GetEditorialsAsync(string orderBy)
        {
            IQueryable<EditorialEntity> query = _dbContext.Editorials;
            query = query.AsNoTracking();

            switch (orderBy)
            {
                case "id":
                    query = query.OrderBy(e => e.Id);
                break;
                case "name":
                    query = query.OrderBy(e => e.Name);
                break;
                case "eMail":
                    query = query.OrderBy(e => e.EMail);
                break;
                case "country":
                    query = query.OrderBy(e => e.Country);
                break;
                default:
                    query = query.OrderBy(e => e.Id);
                break;
            }

            return await query.ToListAsync();
        }

        public async Task<EditorialEntity> GetEditorialAsync(int editorialId, bool showBooks)
        {
            IQueryable<EditorialEntity> query = _dbContext.Editorials;
            query = query.AsNoTracking();
            if (showBooks)
            {
                query = query.Include(e => e.Books);
            }

            var editorial = await query.FirstOrDefaultAsync(e => e.Id == editorialId);
            return editorial;
        }

        public void CreateEditorial(EditorialEntity editorial)
        {
            _dbContext.Editorials.Add(editorial);
        }

        public async Task<bool> DeleteEditorialAsync(int editorialId)
        {
            IQueryable<EditorialEntity> query = _dbContext.Editorials;
            query = query.Include(e => e.Books);
            var editorialToDelete = await query.FirstOrDefaultAsync(e => e.Id == editorialId);

            //Aplicar efecto cascada, si se borra un editorial se borra tambien todos los libros que pertenezcan a esa editorial
            foreach (var book in editorialToDelete.Books)
            {
                await DeleteBookAsync(book.Id);
            }

            _dbContext.Editorials.Remove(editorialToDelete);
            return true;
        }

        public bool UpdateEditorial(EditorialEntity editorialUpdated)
        {
            var editorialToUpdate = _dbContext.Editorials.FirstOrDefault(e => e.Id == editorialUpdated.Id);
            editorialToUpdate.Name = editorialUpdated.Name ?? editorialToUpdate.Name;
            editorialToUpdate.Description = editorialUpdated.Description ?? editorialToUpdate.Description;
            editorialToUpdate.Address = editorialUpdated.Address ?? editorialToUpdate.Address;
            editorialToUpdate.Country = editorialUpdated.Country ?? editorialToUpdate.Country;
            editorialToUpdate.EMail = editorialUpdated.EMail ?? editorialToUpdate.EMail;
            editorialToUpdate.ImagePath = editorialUpdated.ImagePath ?? editorialToUpdate.ImagePath;

            return true;
        }

        //Books

        public async Task<IEnumerable<BookEntity>> GetAllBooksAsync(string orderBy)
        {
            IQueryable<BookEntity> query = _dbContext.Books;
            query = query.AsNoTracking();
            query = query.Include(b => b.Editorial);
            switch (orderBy)
            {
                case "id":
                    query = query.OrderBy(b => b.Id);
                    break;
                case "name":
                    query = query.OrderBy(b => b.Name);
                    break;
                case "genre":
                    query = query.OrderBy(b => b.Genre);
                    break;
                case "author":
                    query = query.OrderBy(b => b.Author);
                    break;
                case "price":
                    query = query.OrderBy(b => b.Price);
                    break;
                case "quantitySold":
                    query = query.OrderBy(b => b.QuantitySold);
                    break;
                default:
                    query = query.OrderBy(b => b.Id);
                    break;
            }
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<BookEntity>> GetBooksAsync(int editorialId)
        {
            IQueryable<BookEntity> query = _dbContext.Books;
            query = query.Where(b => b.Editorial.Id == editorialId);
            query = query.Include(b => b.Editorial);
            query = query.AsNoTracking();
            return await query.ToListAsync();
        }

        public async Task<BookEntity> GetBookAsync(int bookId)
        {
            IQueryable<BookEntity> query = _dbContext.Books;
            query = query.AsNoTracking();
            query = query.Include(b => b.Editorial);
            var book = await query.FirstOrDefaultAsync(b => b.Id == bookId);
            return book;
        }

        public void CreateBook(BookEntity bookToCreate)
        {
            _dbContext.Entry(bookToCreate.Editorial).State = EntityState.Unchanged;
            _dbContext.Books.Add(bookToCreate);
        }

        public async Task<bool> DeleteBookAsync(int bookId)
        {
            var bookToDelete = await _dbContext.Books.FirstOrDefaultAsync(b => b.Id == bookId);
            _dbContext.Remove(bookToDelete);

            return true;
        }

        public bool UpdateBook(int bookId, BookEntity updatedBook)
        {
            var bookToUpdate = _dbContext.Books.FirstOrDefault(b => b.Id == bookId);
            bookToUpdate.Name = updatedBook.Name ?? bookToUpdate.Name;
            bookToUpdate.Genre = updatedBook.Genre ?? bookToUpdate.Genre;
            bookToUpdate.Author = updatedBook.Author ?? bookToUpdate.Author;
            bookToUpdate.Price = updatedBook.Price ?? bookToUpdate.Price;
            bookToUpdate.Description = updatedBook.Description ?? bookToUpdate.Description;
            bookToUpdate.ImagePath = updatedBook.ImagePath ?? bookToUpdate.ImagePath;

            var editorialChanged = _dbContext.Editorials.FirstOrDefault(e => e.Id == updatedBook.Editorial.Id);

            bookToUpdate.Editorial = editorialChanged ?? bookToUpdate.Editorial;

            return true;
        }

        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                var res = await _dbContext.SaveChangesAsync();
                return res > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
