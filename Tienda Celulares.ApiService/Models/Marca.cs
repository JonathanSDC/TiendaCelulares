using System.ComponentModel.DataAnnotations;

namespace Tienda_Celulares.ApiService.Models
{
    public class Marca
    {
        [Key]
        public int id_marca { get; set; }
        public string nombre_marca { get; set; } = string.Empty;
    }
}
