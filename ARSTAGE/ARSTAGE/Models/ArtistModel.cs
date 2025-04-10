using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMG.Models
{
    public class ArtistModel
    {
        [Key]
        [ForeignKey("User")]
        public string ArtistId { get; set; }
        
        public double Rating {  get; set; }
        public bool IsAvailable {  get; set; }
        public string PortfolioLink {  get; set; }
        public AppUserModel User { get; set; }
        public ArtistConfirmationModel  Confirmation { get; set; } 
        public List<ReviewModel> Reviews { get; set; }

    }
}
