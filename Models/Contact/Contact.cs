using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurveyMaker.Models;
public class Contact
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "nvarchar")]
    [StringLength(50)]
    [Required(ErrorMessage = "Phải nhập {0}")]
    [Display(Name = "Họ tên")]
    public string FullName { get; set; }

    [Required(ErrorMessage = "Phải nhập {0}")]
    [StringLength(100)]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    [Display(Name = "Email")]
    public string Email { get; set; }

    public DateTime DateSent { get; set; }

    [Display(Name = "Nội dung")]
    [Required(ErrorMessage = "Phải nhập {0}")]
    public string Message { get; set; }

    [StringLength(50)]
    [Required(ErrorMessage = "Phải nhập {0}")]
    [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
    [Display(Name = "Số điện thoại")]
    public string Phone { get; set; }
}



