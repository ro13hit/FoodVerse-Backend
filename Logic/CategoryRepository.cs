using FoodDeliverySampleApplication.API;
using FoodDeliverySampleApplication.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.JsonPatch;

namespace FoodDeliverySampleApplication.Logic
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly FoodDeliveryContext _context;

        public CategoryRepository(FoodDeliveryContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetCategoriesAsync()
        {
            var categories = await _context.Category.Where(q=>q.IsDeleted != 1).Select(o=> new CategoryResponse { 
               CategoryId = o.CategoryId, Name = o.Name, Description = o.Description }).OrderByDescending(q=>q.CategoryId).ToListAsync();
            return categories;
        }

        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategoriesWithItemsAsync()
        {
            var categories = await _context.Category.Where(q => q.IsDeleted == 0).ToListAsync();
            foreach (Category category in categories)
            {
                var Items = _context.Item.Where(q => q.CategoryId == category.CategoryId && q.IsDeleted == 0).ToList();
                foreach (Item item in Items)
                {
                    category.Items.Add(item);
                }
            }
            return categories;
        }

        public async Task<ActionResult<Category>> AddCategoryAsync(CategoryDTO request)
        {
            Category category = new();
            if (request.Name == null || request.Description == null)
            {
                return null;
            }
            category.Name = request.Name;
            category.Description = request.Description;
            _context.Category.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }
        public async Task<ActionResult<CategoryDTO>> UpdateCategoryAsync(int id,CategoryDTO request)
        {
            var category = await _context.Category.FindAsync(id);
            if (category != null)
            {
                category.Name = request.Name;
                category.Description = request.Description;
                await _context.SaveChangesAsync();
                return request;
            }
            return null;
        }

        public async Task<ActionResult<Category>> UpdateCategoryByPropertyAsync(int id, JsonPatchDocument patchCategory)
        {
            var category = await _context.Category.FirstOrDefaultAsync(q=> q.CategoryId == id);
            if(category!=null)
            {
                patchCategory.ApplyTo(category);
                await _context.SaveChangesAsync();
                return category;
            }
            return null;
        }

        public async Task<ActionResult<Category>> DeleteCategoryAsync(int id)
        {
            var categoryToDelete = await _context.Category.FindAsync(id);
            if (categoryToDelete == null)
            {
                return null;
            }
            _context.Category.Remove(categoryToDelete);
            await _context.SaveChangesAsync();
            return categoryToDelete;
        }
    }
}
