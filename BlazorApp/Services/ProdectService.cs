using BlazorApp.Models;

namespace BlazorApp.Services
{
    public class ProdectService
    {
        public List<ProductModel> Prodects { get; set; }
        public ProdectService()
        {
            Prodects = new List<ProductModel>
            {
                new ProductModel { Id = 1, ProdectName = "Laptop", Price = 999.99m, Description = "A high-performance laptop.", ImageName = "laptop.jpg", CategoryId = 1 },
                new ProductModel { Id = 2, ProdectName = "Smartphone", Price = 699.99m, Description = "A latest model smartphone.", ImageName = "smartphone.jpg", CategoryId = 1 },
                new ProductModel { Id = 3, ProdectName = "Headphones", Price = 199.99m, Description = "Noise-cancelling headphones.", ImageName = "headphones.jpg", CategoryId = 2 },
                new ProductModel { Id = 4, ProdectName = "Camera", Price = 499.99m, Description = "A digital camera with high resolution.", ImageName = "camera.jpg", CategoryId = 3 },
                new ProductModel { Id = 5, ProdectName = "Smartwatch", Price = 299.99m, Description = "A smartwatch with various features.", ImageName = "smartwatch.jpg", CategoryId = 2 }
            };
        }

        public List<ProductModel> GetAllProdects()
        {
            return Prodects;
        }
        public List<ProductModel> GetProdectsByCategory(int categoryId)
        {
            return Prodects.Where(p => p.CategoryId == categoryId).ToList();
        }
        public ProductModel GetProdectById(int id)
        {
            return Prodects.FirstOrDefault(p => p.Id == id);
        }
    }
}
