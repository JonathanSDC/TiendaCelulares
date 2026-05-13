using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.Shared.Models
{
    public class EquipoFisico
    {
        public string NumeroSerie { get; set; } = string.Empty;
        public string Estado { get; set; } = "Disponible";
        public string Color { get; set; } = string.Empty;

        public int IdProducto { get; set; }
        public Producto? Producto { get; set; }

        public int IdTienda { get; set; }
        public Tienda? Tienda { get; set; }
    }

}
