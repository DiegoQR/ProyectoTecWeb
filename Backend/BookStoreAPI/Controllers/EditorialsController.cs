using BookStoreAPI.Exceptions;
using BookStoreAPI.Models;
using BookStoreAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreAPI.Controllers
{
    [Route("api/[controller]")]
    public class EditorialsController : Controller
    {
        private IEditorialService _editorialService;
        private IFileService _fileService;

        public EditorialsController(IEditorialService editorialService, IFileService fileService)
        {
            _editorialService = editorialService;
            _fileService = fileService;
        }

        // GET: api/editorials
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EditorialModel>>> GetEditorialsAsync(string orderBy = "Name")
        {
            try
            {
                return Ok(await _editorialService.GetEditorialsAsync(orderBy));
            }
            catch (BadRequestOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Unexpected error happened: {ex.Message}");
            }
        }

        // GET: api/editorials/editorialId
        [HttpGet("{editorialId:int}")]
        public async Task<ActionResult<EditorialModel>> GetEditorialAsync(int editorialId, bool showBooks = false)
        {
            try
            {
                return await _editorialService.GetEditorialAsync(editorialId,showBooks);
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
        // POST: api/editorials/form
        [HttpPost("form")]
        public async Task<ActionResult<EditorialModel>> CreateEditorialFromAsync([FromForm] EditorialFormModel newEditorial)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var file = newEditorial.Image;
                string imagePath = _fileService.UploadFile(file, "editorial");

                newEditorial.ImagePath = imagePath;

                var editorial = await _editorialService.CreateEditorialAsync(newEditorial);
                return CreatedAtRoute("GetEditorial", new { editorialId = newEditorial.Id }, newEditorial);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Unexpected error happened: {ex.Message}");
            }
        }

        // POST: api/editorials
        [HttpPost]
        public async Task<ActionResult<EditorialModel>> CreateEditorialAsync([FromBody] EditorialModel editorialToCreate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                editorialToCreate.ImagePath = "";
                var newEditorial = await _editorialService.CreateEditorialAsync(editorialToCreate);
                return CreatedAtRoute("GetEditorial", new { editorialId = newEditorial.Id }, newEditorial);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Unexpected error happened: {ex.Message}");
            }
        }

        // DELETE: api/editorials/editorialId
        [HttpDelete("{editorialId:int}")]
        public async Task<ActionResult<DeleteModel>> DeleteEditorialAsync(int editorialId)
        {
            try
            {
                return Ok(await _editorialService.DeleteEditorialAsync(editorialId));
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

        // POST: api/editorials/editorialId
        [HttpPost("{editorialId:int}")]
        public async Task<ActionResult<EditorialModel>> UpdateEditorialAsync(int editorialId,[FromBody] EditorialModel editorialEdited)
        {
            try
            {
                var editorial = await _editorialService.UpdateEditorialAsync(editorialId, editorialEdited);
                return Ok(editorial);
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