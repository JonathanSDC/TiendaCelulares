using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.Shared.Models
{
    public class Inventario
    {
        public int IdInventario { get; set; }
        public int IdProducto { get; set; }
        public Producto? Producto { get; set; }

        public int IdTienda { get; set; }
        public Tienda? Tienda { get; set; }

        public int Cantidad { get; set; }
        public int StockMinimo { get; set; }
    }

}
