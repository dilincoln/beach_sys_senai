using System.ComponentModel.DataAnnotations;

namespace Prova.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome completo obrigatório")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "CPF obrigatório.")]
        public string? Cpf { get; set; }

        [Required(ErrorMessage = "E-mail obrigatório.")]
        public string? Email { get; set; }
    }
}
