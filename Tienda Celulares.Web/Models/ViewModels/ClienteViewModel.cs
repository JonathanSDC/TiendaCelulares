using System.ComponentModel.DataAnnotations;

namespace TiendaCelulares.Models.ViewModels
{
    public class ClienteViewModel
    {
        public int IdPersona { get; set; }

        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public string Apellido { get; set; } = string.Empty;

        public string? Telefono { get; set; }

        public string? Email { get; set; }

        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.Today;

        public string? TipoCliente { get; set; }

        public int? IdDireccion { get; set; }
    }
}