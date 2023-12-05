using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurveyMaker.Models;

public class tblDisableModel
{
    [Key]
    public int Id { get; set; }

    public bool? Disabled { get; set; } = false;
    public DateTime? LongTime { get; set; }

    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public int ResponseId { get; set; }

    public ResponseModel? Response { get; set; }
}