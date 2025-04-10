using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMG.Models
{
    public class ArtistConfirmationModel
    {
        [Key]
        public int ConfirmationID { get; set; }
        [Required]
        [ForeignKey("Artist")]
        public string ArtistID { get; set; }
        public string FileName { get; set; }
        public string Samplelink { get; set; }
        public DateTime SendDate { get; set; }
        public string Description { get; set; }
        public ArtistModel Artist { get; set; }
    }
}
