using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IMG.Models
{
    public class MessageFileModel
    {
        [Key]
        public int FileID { get; set; }

        [ForeignKey("Message")]
        public int MessageID { get; set; }

        [Required]
        [MaxLength(255)]
        public string FileName { get; set; }

        [Required]
        [MaxLength(500)]
        public string FilePath { get; set; }

        public MessageModel Message { get; set; }
    }
}
