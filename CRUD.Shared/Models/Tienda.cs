using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD.Shared.Models
{
    public class Tienda
    {
        [Key]
        public int IdTienda { get; set; }

        [Required]
        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        // FK hacia Direccion
        [ForeignKey("Direccion")]
        public int IdDireccion { get; set; }

        // Navegación
        public Direccion? Direccion { get; set; }

        // Relación con Inventario
        public ICollection<Inventario> Inventarios { get; set; } = new List<Inventario>();

        // Relación con Equipo Físico
        public ICollection<EquipoFisico> EquiposFisicos { get; set; } = new List<EquipoFisico>();

    }
}


