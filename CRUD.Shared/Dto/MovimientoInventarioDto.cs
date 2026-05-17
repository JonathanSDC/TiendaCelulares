using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.Shared.Dto
{
    public class MovimientoInventarioDto
    {
        public int IdMovimiento { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string? Referencia { get; set; }
        public string? NumeroSerie { get; set; }
        public int? IdProducto { get; set; }
        public int Cantidad { get; set; }
        public int? IdTiendaOrigen { get; set; }
        public int? IdTiendaDestino { get; set; }
    }
}
