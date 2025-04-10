using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMG.Models
{
    public class FollowModel
    {
      
        [ForeignKey("Follower")]
        public string FollowerID { get; set; } // Người theo dõi
        
        [ForeignKey("Following")]
        public string FollowingID { get; set; } // Người được theo dõi
        public AppUserModel Follower { get; set; } // Liên kết với người theo dõi (User)

        public ArtistModel Following { get; set; } // Liên kết với người được theo dõi
    }
}
