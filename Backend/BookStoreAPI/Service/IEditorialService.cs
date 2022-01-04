using BookStoreAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreAPI.Service
{
    public interface IEditorialService
    {
        public Task<IEnumerable<EditorialModel>> GetEditorialsAsync(string orderBy);
        public Task<EditorialModel> GetEditorialAsync(int editorialId, bool showBooks);
        public Task<EditorialModel> CreateEditorialAsync(EditorialModel editorialToCreate);
        public Task<DeleteModel> DeleteEditorialAsync(int editorialId);
        public Task<EditorialModel> UpdateEditorialAsync(int editorialId, EditorialModel editorialEdited);
    }
}
