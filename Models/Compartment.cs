using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Prova.Models
{
    public class Compartment
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        [DisplayName("Em uso por:")]
        public int? UserId { get; set; }

        [ForeignKey("Cabinet")]
        [DisplayName("Armário")]
        public int CabinetId { get; set; }

        [Required(ErrorMessage = "Largura obrigatória")]
        [Range(1, 20, ErrorMessage = "Largura inválida, digite um valor entre 1 e 20")]
        [DisplayName("Largura")]
        public int? width { get; set; }

        [Required(ErrorMessage = "Altura obrigatória")]
        [Range(1, 20, ErrorMessage = "Altura inválida, digite um valor entre 1 e 20")]
        [DisplayName("Altura")]
        public int? height { get; set; }

        [Required(ErrorMessage = "Profundidade obrigatória")]
        [Range(1, 20, ErrorMessage = "Profundidade inválida, digite um valor entre 1 e 20")]
        [DisplayName("Profundidade")]
        public int? depth { get; set; }
    }
}
