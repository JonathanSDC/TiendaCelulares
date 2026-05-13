using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.Shared.Dto
{
    public class InventarioDto
    {
        public int IdInventario { get; set; }

        // Producto
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; } = string.Empty;

        // Tienda
        public int IdTienda { get; set; }
        public string NombreTienda { get; set; } = string.Empty;

        // Stock
        public int Cantidad { get; set; }
        public int StockMinimo { get; set; }
    }

}
