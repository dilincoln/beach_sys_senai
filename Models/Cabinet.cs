using System.ComponentModel.DataAnnotations;

namespace Prova.Models
{
    public class Cabinet
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome obrigatório")]
        public string? Name { get; set; }

        public bool Available { get; set; } = true;

        [Required(ErrorMessage = "Latitude obrigatória")]
        public double? Latitude { get; set; }

        [Required(ErrorMessage = "Longitude obrigatória")]
        public double? Longitude { get; set; }
    }
}
