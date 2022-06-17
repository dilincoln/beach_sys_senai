using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Prova.Models
{
    public class Compartment
    {
        [Key]
        [DisplayName("Número do compartimento")]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int? UserId { get; set; }

        [ForeignKey("Cabinet")]
        [DisplayName("Armário")]
        public int CabinetId { get; set; }

        [Required(ErrorMessage = "Largura obrigatória")]
        [Range(1, 20, ErrorMessage = "Largura inválida, digite um valor entre 1 e 20cm")]
        [DisplayName("Largura(cm)")]
        public int? Width { get; set; }

        [Required(ErrorMessage = "Altura obrigatória")]
        [Range(1, 20, ErrorMessage = "Altura inválida, digite um valor entre 1 e 20cm")]
        [DisplayName("Altura(cm)")]
        public int? Height { get; set; }

        [Required(ErrorMessage = "Profundidade obrigatória")]
        [Range(1, 20, ErrorMessage = "Profundidade inválida, digite um valor entre 1 e 20cm")]
        [DisplayName("Profundidade(cm)")]
        public int? Depth { get; set; }

        public virtual User? User { get; set; }
    }
}
