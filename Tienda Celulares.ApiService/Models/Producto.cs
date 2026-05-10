using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tienda_Celulares.ApiService.Models
{
    [Table("producto")]
    public class Producto
    {
        [Key]
        public int id_producto { get; set; }

        public string nombre_modelo { get; set; } = string.Empty;

        public string? descripcion { get; set; }

        public decimal precio_actual { get; set; }

        public string tipo_producto { get; set; } = string.Empty;

        public int? id_marca { get; set; }

        public int? id_categoria { get; set; }

        //visualizar
        public virtual Marca? Marca { get; set; }
        public virtual Categoria? Categoria { get; set; }

        public ICollection<DetalleVenta> DetalleVentas { get; set; } = new List<DetalleVenta>();
    }
}