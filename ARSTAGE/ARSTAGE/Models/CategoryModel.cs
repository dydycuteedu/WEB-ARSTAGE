using System.ComponentModel.DataAnnotations;

namespace ARSTAGE.Models
{
    public class CategoryModel
    {
        [Key]
        public int CategoryID {  get; set; }
        [Required]
        public string CategoryName { get; set; }
        public string Description { get; set; } = string.Empty;
        public List<PostCategoryModel> PostCategories { get; set; } = new();
    }
}
