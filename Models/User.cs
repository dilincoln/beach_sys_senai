using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Prova.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome completo obrigatório")]
        [DisplayName("Nome completo")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "CPF obrigatório.")]
        [DisplayName("CPF")]
        [RegularExpression(
            @"^\d{3}\.\d{3}\.\d{3}-\d{2}$",
            ErrorMessage = "CPF inválido. Informe o CPF com ponto e traço."
        )]
        public string? Cpf { get; set; }

        [Required(ErrorMessage = "E-mail obrigatório.")]
        [DisplayName("E-mail")]
        public string? Email { get; set; }

        public ICollection<Compartment>? Compartments { get; set; }
    }
}
