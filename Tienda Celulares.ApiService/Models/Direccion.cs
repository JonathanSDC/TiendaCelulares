using System.ComponentModel.DataAnnotations;

namespace Tienda_Celulares.ApiService.Models
{
    public class Direccion
    {
        [Key]
        public int id_direccion { get; set; }

        public string calle { get; set; } = string.Empty;
        public string ciudad { get; set; } = string.Empty;
        public string departamento { get; set; } = string.Empty;
        public string pais { get; set; } = string.Empty;
    }

}
