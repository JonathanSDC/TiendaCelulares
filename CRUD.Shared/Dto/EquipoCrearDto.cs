using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.Shared.Dto
{
    public class EquipoCrearDto
    {
        public string NumeroSerie { get; set; } = string.Empty;
        public string? Estado { get; set; }
        public string? Color { get; set; }
        public int IdProducto { get; set; }
        public int IdTienda { get; set; }
    }

}
