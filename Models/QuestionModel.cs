using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurveyMaker.Models;

public class QuestionModel
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "nvarchar")]
    public string? QuestionTitle { get; set; }

    [Column(TypeName = "nvarchar")]
    public string? QuestionType { get; set; }
    public bool? IsRequired { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public int FormId { get; set; }
    public FormModel? Form { get; set; }
}