using System.ComponentModel.DataAnnotations;

namespace Tienda_Celulares.ApiService.Models
{
    public class Categoria
    {
        [Key]
        public int id_categoria { get; set; }
        public string nombre_categoria { get; set; } = string.Empty;
    }
}
