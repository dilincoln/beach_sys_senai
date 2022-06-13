using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Prova.Models
{
    public class Cabinet
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome obrigatório")]
        [DisplayName("Nome do armário")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Latitude obrigatória")]
        [Range(-90, 90, ErrorMessage = "Latitude inválida, digite um valor entre -90 e 90")]
        public double? Latitude { get; set; }

        [Required(ErrorMessage = "Longitude obrigatória")]
        [Range(-180, 180, ErrorMessage = "Longitude inválida, digite um valor entre -180 e 180")]
        public double? Longitude { get; set; }

        [DisplayName("Localização no mapa")]
        public virtual string MapsUrl
        {
            get
            {
                // Return embed map url without API key
                return $"https://maps.google.com/maps?q={Latitude},{Longitude}&t=&z=13&ie=UTF8&iwloc=&output=embed";
            }
        }

        public ICollection<Compartment> Compartments { get; set; }
    }
}
