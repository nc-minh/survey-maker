using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurveyMaker.Models;

public class ResponseModel
{
    [Key]
    public int Id { get; set; }

    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public int FormId { get; set; }
    public string UserId { get; set; } = null!;
    public FormModel? Form { get; set; }
}