using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurveyMaker.Models;

public class FormModel
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "nvarchar")]
    public string? Title { get; set; }

    [Column(TypeName = "nvarchar")]
    public string? Description { get; set; }
    public bool? IsLoginRequired { get; set; }
    public int? SubmissionLimit { get; set; } = 0;
    public bool? IsRelease { get; set; } = false;
    public string? CreatedBy { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public List<QuestionModel>? Questions { get; set; }
    public List<ResponseModel>? Responses { get; set; }
}