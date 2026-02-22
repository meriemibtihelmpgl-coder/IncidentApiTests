using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Incident
    {
        public int Id { get; set; }
        [Required]
        [StringLength(30,MinimumLength =3 ,ErrorMessage = "la taille du titre doit etre compris entre 3 et 30")]
        public String Title { get; set; } = null!;
        [Required]
        [StringLength(200)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public string Severity { get; set; } = string.Empty;
      //  [Required]
        public string Status { get; set; } = string.Empty;
      //  [Required]
        public DateTime CreatedAt { get; set; }
        


    }
}
