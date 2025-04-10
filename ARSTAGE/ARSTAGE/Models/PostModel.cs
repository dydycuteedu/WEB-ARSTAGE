using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMG.Models
{
    public class PostModel
    {
        [Key]
        public int PostID {  get; set; }
        [Required]
        [ForeignKey("Artist")]
        public string ArtistID {  get; set; }
        public bool Visibility {  get; set; } // (public/private)
        public bool IsPremium {  get; set; }
        public DateTime CreateDate { get; set; }
        public string Description {  get; set; }
        public int NumberOfLike {  get; set; }
        public int NumberOfSale {  get; set; }

        public ArtistModel Artist { get; set; }
        public List<ProductModel> Products { get; set; } = new List<ProductModel>();
        public List<PostCategoryModel> PostCategories { get; set; } = new();
        public List<LikeModel> Likes { get; set; } = new List<LikeModel>();
        public List<CommentModel> Comments { get; set; } = new List<CommentModel> ();
        public List<FavouriteModel> Favorites { get; set; } = new List<FavouriteModel>();
        
    }
}
