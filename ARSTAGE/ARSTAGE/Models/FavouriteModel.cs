using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMG.Models
{
    public class FavouriteModel
    {
        public string UserID {  get; set; }
        public int PostID {  get; set; }
        public AppUserModel User { get; set; } // Liên kết với bảng AspNetUsers (AppUserModel)
        public PostModel Post { get; set; } // Liên kết với bảng PostModels (PostModel)
    }
}
