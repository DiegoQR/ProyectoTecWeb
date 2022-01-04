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
    public class EditorialService : IEditorialService
    {
        IBookStoreRepository _bookStoreRepository;
        private IMapper _mapper;

        private HashSet<string> allowedOrderByParameters = new HashSet<string>()
        {
            "id",
            "name",
            "country",
            "eMail"
        };

        public EditorialService(IBookStoreRepository bookStoreRepository, IMapper mapper)
        {
            _bookStoreRepository = bookStoreRepository;
            _mapper = mapper;
        }

        public async Task<EditorialModel> CreateEditorialAsync(EditorialModel editorialToCreate)
        {
            var editorialEntity = _mapper.Map<EditorialEntity>(editorialToCreate);
            _bookStoreRepository.CreateEditorial(editorialEntity);
            var result = await _bookStoreRepository.SaveChangesAsync();

            if (!result)
            {
                throw new Exception("Database Error");
            }

            return _mapper.Map<EditorialModel>(editorialEntity);
        }

        public async Task<DeleteModel> DeleteEditorialAsync(int editorialId)
        {
            await ValidateEditorialAsync(editorialId);

            var deleteResult = await _bookStoreRepository.DeleteEditorialAsync(editorialId);
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
                    Message = "The editorial was deleted."
                };
            }
            else
            {
                return new DeleteModel()
                {
                    IsSuccess = saveResult,
                    Message = "The editorial was not deleted."
                };
            }
        }

        public async Task<EditorialModel> GetEditorialAsync(int editorialId, bool showBooks)
        {
            await ValidateEditorialAsync(editorialId);
            var editorial = await _bookStoreRepository.GetEditorialAsync(editorialId, showBooks);

            return _mapper.Map<EditorialModel>(editorial);
        }

        public async Task<IEnumerable<EditorialModel>> GetEditorialsAsync(string orderBy)
        {
            if (!allowedOrderByParameters.Contains(orderBy.ToLower()))
            {
                throw new BadRequestOperationException($"the field: {orderBy} is not supported, please use one of these {string.Join(",", allowedOrderByParameters)}");
            }
            var editorialsEtities = await _bookStoreRepository.GetEditorialsAsync(orderBy);
            var editorialsModels = _mapper.Map<IEnumerable<EditorialModel>>(editorialsEtities);
            return editorialsModels;
        }

        public async Task<EditorialModel> UpdateEditorialAsync(int editorialId, EditorialModel editorialEdited)
        {
            await ValidateEditorialAsync(editorialId);

            var editorialEditEntity = _mapper.Map<EditorialEntity>(editorialEdited);
            editorialEditEntity.Id = editorialId;

            _bookStoreRepository.UpdateEditorial(editorialEditEntity);
            var results = await _bookStoreRepository.SaveChangesAsync();

            if (!results)
            {
                throw new Exception("Database Error");
            }
            return editorialEdited;
        }

        private async Task ValidateEditorialAsync(int editorialId)
        {
            var editorial = await _bookStoreRepository.GetEditorialAsync(editorialId, false);
            if (editorial == null)
            {
                throw new NotFoundOperationException($"The editorial with id: {editorialId} does not exists.");
            }
        }
    }
}
