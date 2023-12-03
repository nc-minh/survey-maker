using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurveyMaker.Models;

public class AnswerModel
{
    [Key]
    public int Id { get; set; }

    public string? content { get; set; }

    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public int ResponseId { get; set; }
    public int QuestionId { get; set; }
    public int? OptionId { get; set; }

    public ResponseModel? Response { get; set; }
    public QuestionModel? Question { get; set; }
    public OptionModel? Option { get; set; }
}