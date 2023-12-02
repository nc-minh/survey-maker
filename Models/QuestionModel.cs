using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurveyMaker.Models;

public class QuestionModel
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "nvarchar")]
    [StringLength(500)]
    public string? QuestionTitle { get; set; }

    [Column(TypeName = "nvarchar")]
    [StringLength(500)]
    public string? QuestionType { get; set; }
    public bool? IsRequired { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public bool? IsDone { get; set; } = false;

    public int FormId { get; set; }
    public FormModel? Form { get; set; }

    public List<OptionModel>? Options { get; set; }

    public List<AnswerModel>? Answers { get; set; }
}