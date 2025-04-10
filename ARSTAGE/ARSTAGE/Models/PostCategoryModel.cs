using System.ComponentModel.DataAnnotations.Schema;

namespace IMG.Models
{
    public class PostCategoryModel
    {
        [ForeignKey("Post")]
        public int PostID {  get; set; }
        [ForeignKey("Category")]
        public int CategoryID {  get; set; }
        public PostModel Post { get; set; }  // Liên kết với bảng PostModels (PostModel)

        public CategoryModel Category { get; set; }  // Liên kết với bảng CategoryModels (CategoryModel)

    }
}
