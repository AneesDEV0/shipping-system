using BlazorApp.Models;

namespace BlazorApp.Services
{
    public class CategoryService 
    {
        public List<Category> Categories { get; set; }
        public CategoryService() {
            Categories = new List<Category>
            {
                new Category { Id = 1, Name = "Electronics" },
                new Category { Id = 2, Name = "Wearables" },
                new Category { Id = 3, Name = "Cameras" }
            };

        }
        public List<Category> GetAllCategories()
        {
            return Categories;
        }
        public Category GetCategoryById(int id)
        {
            return Categories.FirstOrDefault(c => c.Id == id);
        }
    }
}
