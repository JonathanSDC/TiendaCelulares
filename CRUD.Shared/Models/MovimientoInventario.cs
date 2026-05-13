using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.Shared.Models
{
    public class MovimientoInventario
    {
        public int IdMovimiento { get; set; }
        public string Tipo { get; set; } = string.Empty; // entrada, salida, ajuste
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string Referencia { get; set; } = string.Empty;

        public string? NumeroSerie { get; set; } // si aplica
        public int? IdProducto { get; set; }     // si aplica
        public int Cantidad { get; set; }
    }

}
