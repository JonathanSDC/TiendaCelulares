using CRUD.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.Shared.Dto
{
    public class TiendaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        
        // Dirección completa
        public string Calle { get; set; } = string.Empty;
        public string Ciudad { get; set; } = string.Empty;
        public string Departamento { get; set; } = string.Empty;
        public string Pais { get; set; } = string.Empty;
    }

}
