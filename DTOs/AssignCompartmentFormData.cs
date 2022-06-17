using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Prova.DTOs;

public class AssignCompartmentFormData
{
    [Required(ErrorMessage = "Usuário obrigatório")]
    [DisplayName("Usuários cadastrados")]
    public int UserId { get; set; }

    [Required(ErrorMessage = "Selecione um compartimento")]
    [DisplayName("Compartimentos disponíveis")]
    public int CompartmentId { get; set; }
}
