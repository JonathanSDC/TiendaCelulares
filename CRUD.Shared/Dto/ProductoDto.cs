using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.Shared.Dto
{
    public class ProductoDto
    {
        public int Id { get; set; }
        public string NombreModelo { get; set; } = string.Empty;
        public decimal PrecioActual { get; set; }
        public int Stock { get; set; }
    }
}
