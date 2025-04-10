using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMG.Models
{
    public class MessageModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Sender")]
        public string SenderId { get; set; }
        [Required]
        [ForeignKey("Receiver")]
        public string ReceiverId { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public bool HasFiles { get; set; }  // Có đính kèm file hay không

        public AppUserModel Sender { get; set; }  // Liên kết với bảng AspNetUsers (người gửi)
        
        public AppUserModel Receiver { get; set; }  // Liên kết với bảng AspNetUsers (người nhận)
        public List<MessageFileModel>? Files { get; set; }
    }
}
