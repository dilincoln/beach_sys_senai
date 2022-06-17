using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Prova.DTOs;

public class DisassociateCompartmentFormData
{
    [Required(ErrorMessage = "Selecione um compartimento")]
    [DisplayName("Compartimentos ocupados")]
    public int CompartmentId { get; set; }
}
