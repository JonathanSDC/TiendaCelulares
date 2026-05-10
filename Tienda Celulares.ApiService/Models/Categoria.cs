using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tienda_Celulares.ApiService.Models
{
    public class Categoria
    {
        [Key]
        [Column("id_categoria")]
        public int IdCategoria { get; set; }

        [Column("nombre_categoria")]
        public string Nombre { get; set; } = string.Empty;
    }
}
