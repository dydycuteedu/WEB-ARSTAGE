using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ARSTAGE.Models
{
    public class ProductModel
    {
        [Key]
        public int ProductID { get; set; }

        [Required]
        [ForeignKey("Post")]
        public int PostID { get; set; } // Khóa ngoại tham chiếu đến Post
        [Required]
        public string ImagePath { get; set; }
        public PostModel Post { get; set; }

    }
}
