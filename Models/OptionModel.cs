using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurveyMaker.Models;

public class OptionModel
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "nvarchar")]
    [StringLength(500)]
    public string? Content { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public int QuestionId { get; set; }
    public QuestionModel? Question { get; set; }

    public List<AnswerModel>? Answers { get; set; }
}