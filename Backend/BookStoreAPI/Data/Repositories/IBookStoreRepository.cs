using BookStoreAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreAPI.Data.Repositories
{
    public interface IBookStoreRepository
    {
        //Editorials
        public Task<IEnumerable<EditorialEntity>> GetEditorialsAsync(string orderBy);
        public Task<EditorialEntity> GetEditorialAsync(int editorialId, bool showBooks);
        public void CreateEditorial(EditorialEntity editorialToCreate);
        public Task<bool> DeleteEditorialAsync(int editorialId);
        public bool UpdateEditorial(EditorialEntity editorialToUpdate);

        //Books
        public Task<IEnumerable<BookEntity>> GetAllBooksAsync(string orderBy);
        public Task<IEnumerable<BookEntity>> GetBooksAsync(int editorialId);
        public Task<BookEntity> GetBookAsync(int bookId);
        public void CreateBook(BookEntity bookToCreate);
        public Task<bool> DeleteBookAsync(int bookId);
        public bool UpdateBook(int bookId, BookEntity updatedBook);

        //Save Changes
        Task<bool> SaveChangesAsync();
    }
}
