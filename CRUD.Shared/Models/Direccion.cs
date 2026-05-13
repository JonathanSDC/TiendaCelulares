using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD.Shared.Models
{
    public class Direccion
    {
        [Key]
        public int IdDireccion { get; set; }

        [Column("calle")]
        public string Calle { get; set; } = string.Empty;

        [Column("ciudad")]
        public string Ciudad { get; set; } = string.Empty;

        [Column("departamento")]
        public string Departamento { get; set; } = string.Empty;

        [Column("pais")]
        public string Pais { get; set; } = string.Empty;

        // Una dirección puede estar asociada a varias tiendas
        public ICollection<Tienda> Tiendas { get; set; } = new List<Tienda>();
    }

}
