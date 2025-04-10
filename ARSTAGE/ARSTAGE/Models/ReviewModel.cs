using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMG.Models
{
    public class ReviewModel
    {
        [Key]
        public int ReviewID {  get; set; }
        [Required]
        [ForeignKey("User")]
        public string UserID { get; set; }
        [Required]
        [ForeignKey("Artist")]
        public string ArtistID { get; set; }
        [Required]
        [ForeignKey("Commission")]
        public int CommissionID { get; set; }
        public string Content { get; set; }
        public int Rating {  get; set; }
        public DateTime CreateDate { get; set; }
        public AppUserModel User { get; set; }
        public ArtistModel Artist { get; set; }
        public CommissionModel Commission { get; set; }

    }
}
