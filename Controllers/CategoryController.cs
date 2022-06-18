using FoodDeliverySampleApplication.DAL;
using Microsoft.AspNetCore.Mvc;
using FoodDeliverySampleApplication.API;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using FoodDeliverySampleApplication.Logic;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.JsonPatch;

namespace FoodDeliverySampleApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<CategoryController> _logger;
        public CategoryController(ICategoryRepository categoryRepository, ILogger<CategoryController> logger)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
        }

        //category routes
        [HttpGet("GetCategories")]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories()
        {
            _logger.LogInformation("Getting all categories!");
            var categories = await _categoryRepository.GetCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("GetCategoriesWithItems")]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategoriesWithItems()
        {
            _logger.LogInformation("Getting all categories with items");
            var categories = await _categoryRepository.GetAllCategoriesWithItemsAsync();
            return Ok(categories);
        }

        [Authorize]
        [HttpPost("AddCategory")]
        public async Task<ActionResult<Category>> AddCategory(CategoryDTO request)
        {
            _logger.LogInformation("Adding new category with name {name}", request.Name);
            var category = await _categoryRepository.AddCategoryAsync(request);
            if (category != null)
            {
                var message = "Category added successfully!";
                _logger.LogInformation(message);
                return Ok(new
                {
                    message,
                    request.Name,
                    request.Description
                });
            }
            _logger.LogWarning("Couldn't add categories not all required values were provided for {name}",request.Name);
            return BadRequest("Provide Values of name and description to proceed!");
        }

        [Authorize]
        [HttpPut("UpdateCategory/{id}")]
        public async Task<ActionResult<Category>> UpdateCategory(int id, CategoryDTO request)
        {
            _logger.LogInformation("Updating category information for {id}",id);
            var updateCategory = await _categoryRepository.UpdateCategoryAsync(id,request);
            if (updateCategory != null)
            {
                var message = "Update Successfull!";
                _logger.LogInformation(message);
                return Ok(
                    new
                    {
                        message,
                        request
                    });
            }
            _logger.LogInformation("Couldn't find category or fields are not correct for {id}",id);
            return BadRequest("check categoryId and update descriptions of fields!");
        }

        [Authorize]
        [HttpPatch("UpdateCategoryByProperty/{id}")]
        public async Task<ActionResult<Category>> UpdateCategoryByProperty(int id, JsonPatchDocument request)
        {
            var updateCategory = await _categoryRepository.UpdateCategoryByPropertyAsync(id, request);
            if(updateCategory!=null)
            {
                var message = "Update Successfull";
                return Ok(new
                {
                    message,
                    updateCategory
                });
            }
            return BadRequest("check categoryId and other details");
        }

        [Authorize]
        [HttpDelete("DeleteCategory/{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            _logger.LogInformation("Deleting category {id}", id);
            var categoryToDelete = await _categoryRepository.DeleteCategoryAsync(id);
            if (categoryToDelete != null)
            {
                var message = "Deleted Successfully";
                _logger.LogInformation(message);
                return Ok(
                    new
                    {
                        message,
                        categoryToDelete
                    });
            }
            _logger.LogWarning("Couldn't find category matching {id}", id);
            return NotFound("Record Not Found");
        }
    }
}
