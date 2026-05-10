using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Tienda_Celulares.Web.Models
{
    public class MetodoPago
    {
        
        public int IdMetodo { get; set; }

        public string TipoMetodo { get; set; } = string.Empty;

        // Relaciones
        public ICollection<Venta>? Ventas { get; set; }
    }
}
