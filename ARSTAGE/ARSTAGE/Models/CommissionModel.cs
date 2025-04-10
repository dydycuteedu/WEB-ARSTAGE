using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMG.Models
{
    public class CommissionModel
    {
        [Key]
        public int CommissionID {  get; set; }
        [ForeignKey("User")]
        public string UserID {  get; set; }
        [ForeignKey("Artist")]
        public string ArtistID {  get; set; }
        public string Description {  get; set; }
        public string SampleLink {  get; set; }
        public double Price {  get; set; }
        public string Status { get; set; } // (pending/processing/reject/completed)
        public DateTime CrateDate { get; set; }
        public DateTime EndDate { get; set; }

        public AppUserModel User { get; set; }
        public ArtistModel Artist { get; set; }
    

    }
}
