using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tienda_Celulares.ApiService.Models
{
    public class Marca
    {
        [Key]
        [Column("id_marca")]
        public int IdMarca { get; set; }

        [Column("nombre_marca")]
        public string Nombre { get; set; } = string.Empty;
    }
}
