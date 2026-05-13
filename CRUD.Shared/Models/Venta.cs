using System.ComponentModel.DataAnnotations;

namespace CRUD.Shared.Models
{
    public class Venta
    {
        [Key]
        public int id_venta { get; set; }

        public DateTime fecha_hora { get; set; }
        public decimal total_venta { get; set; }
        public string? estado { get; set; }

        public int? id_cliente { get; set; }
        public int? id_empleado { get; set; }
        public int? id_metodo { get; set; }
        public int? id_tienda { get; set; }

        public Cliente? Cliente { get; set; }
        public Empleado? Empleado { get; set; }
        public MetodoPago? MetodoPago { get; set; }

        public ICollection<DetalleVenta> DetalleVentas { get; set; } = new List<DetalleVenta>();
    }
}
