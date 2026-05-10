using System.ComponentModel.DataAnnotations;

namespace Tienda_Celulares.ApiService.Models
{
    public class MetodoPago
    {
        [Key]
        public int IdMetodo { get; set; }

        public string TipoMetodo { get; set; } = string.Empty;

        // Relaciones
        public ICollection<Venta>? Ventas { get; set; }
    }
}
