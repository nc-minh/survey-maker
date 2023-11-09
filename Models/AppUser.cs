using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace SurveyMaker.Models
{
    public class AppUser : IdentityUser
    {
        [Column(TypeName = "nvarchar")]
        public string? HomeAdress { get; set; }

        // [Required]       
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }
    }
}
