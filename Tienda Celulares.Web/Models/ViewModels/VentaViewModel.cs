using Tienda_Celulares.Web.Models; // Ajusta según tu namespace

namespace Tienda_Celulares.Web.Models.ViewModel
{
    public class VentaViewModel
    {
        public int IdVenta { get; set; }

        public DateTime FechaHora { get; set; }

        public decimal TotalVenta { get; set; }

        public string? Estado { get; set; }

        public int IdCliente { get; set; }

        public string? ClienteNombre { get; set; }

        public int IdEmpleado { get; set; }

        public int IdMetodo { get; set; }

        public int IdTienda { get; set; }

        
        public string NombreCliente { get; set; } = string.Empty;
    }
}
