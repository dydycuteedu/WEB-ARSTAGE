using System.ComponentModel.DataAnnotations.Schema;

namespace ARSTAGE.Models
{
    public class LikeModel
    {
        [ForeignKey("User")]
        public string UserID { get; set; }
        [ForeignKey("Post")]
        public int PostID { get; set; }

        public AppUserModel User { get; set; }  // Navigation property
        public PostModel Post { get; set; }     // Navigation property
    }
}
