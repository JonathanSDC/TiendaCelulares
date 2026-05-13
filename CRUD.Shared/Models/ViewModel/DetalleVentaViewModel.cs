using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.Shared.Models.ViewModel
{
    public class DetalleVentaViewModel
    {
        public int IdDetalle { get; set; }
        public int IdProducto { get; set; }
        public string? NombreProducto { get; set; }
        public string? NumeroSerie { get; set; } // opcional
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}
