using FoodDeliverySampleApplication.API;
using FoodDeliverySampleApplication.DAL;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodDeliverySampleApplication.Logic
{
    public interface ICategoryRepository
    {
        Task<ActionResult<IEnumerable<CategoryResponse>>> GetCategoriesAsync();
        Task<ActionResult<IEnumerable<Category>>> GetAllCategoriesWithItemsAsync();
        Task<ActionResult<Category>> AddCategoryAsync(CategoryDTO request);
        Task<ActionResult<CategoryDTO>> UpdateCategoryAsync(int id,CategoryDTO request);
        Task<ActionResult<Category>> DeleteCategoryAsync(int id);
        Task<ActionResult<Category>> UpdateCategoryByPropertyAsync(int id, JsonPatchDocument patchCategory);
    }
}
