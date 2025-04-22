using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ARSTAGE.Models
{
    public class TransactionModel
    {
        [Key]
        public string TransactionID { get; set; } // ID giao dịch

        [Required]
        [ForeignKey("User")]
        public string UserID { get; set; } // ID người dùng thực hiện giao dịch

        [Required]
        public decimal Amount { get; set; } // Số tiền giao dịch

        [Required]
        public DateTime TransactionDate { get; set; } // Ngày giờ giao dịch

        public string Description { get; set; } // Mô tả giao dịch (nếu có)

        [Required]
        public string TransactionType { get; set; } // Loại giao dịch (Rent, Buy)

        [Required]
        public string Status { get; set; } // Trạng thái giao dịch (Completed, Pending, Failed)

        // Khóa ngoại đến User
        public AppUserModel User { get; set; }

        // Mối quan hệ với các bảng khác (nếu cần)
        [ForeignKey("Post")]
        public int? PostID { get; set; } // Khóa ngoại đến PostModel (nếu là giao dịch mua sản phẩm)
        [ForeignKey("Commission")]
        public int? CommissionID { get; set; }  // Khóa ngoại đến CommissionModel (nếu là giao dịch thuê artist)

        // Mối quan hệ với PostModel và CommissionModel
        public PostModel Post { get; set; }

        public CommissionModel Commission { get; set; }


    }
}
