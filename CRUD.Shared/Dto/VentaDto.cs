using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.Shared.Dto
{
    public class VentaDto
    {
        public int IdVenta { get; set; }
        public DateTime FechaHora { get; set; }
        public decimal TotalVenta { get; set; }
        public string? Estado { get; set; }
        public int? IdCliente { get; set; }
        public string ClienteNombre { get; set; } = string.Empty;
        public int? IdEmpleado { get; set; }
        public int? IdMetodo { get; set; }
        public int? IdTienda { get; set; }
        public List<DetalleVentaDto> Detalles { get; set; } = new();
    }
}
