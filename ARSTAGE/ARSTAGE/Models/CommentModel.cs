using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ARSTAGE.Models
{
    public class CommentModel
    {
        [Key]
        public int CommentID { get; set; }
        [Required]
        [ForeignKey("Post")]
        public int PostID { get; set; }

        [ForeignKey("User")]
        public string? UserID { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime CommentDate { get; set; }

        public AppUserModel? User { get; set; }
        public PostModel Post { get; set; }
    }

}
