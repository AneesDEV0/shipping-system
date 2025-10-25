using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Models
{
    public class ProductModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(100, ErrorMessage = "Product name cannot exceed 100 characters.")]
        public string ProdectName { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, 100000, ErrorMessage = "Price must be between 0.01 and 100000.")]
        public decimal Price { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Image name is required.")]
        [RegularExpression(@"^[a-zA-Z0-9_\-]+\.(jpg|jpeg|png|gif)$",
            ErrorMessage = "Image name must be a valid file with .jpg, .jpeg, .png, or .gif extension.")]
        public string ImageName { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid category.")]
        public int CategoryId { get; set; }
    }
}
