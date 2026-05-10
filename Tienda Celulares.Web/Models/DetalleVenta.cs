using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;

using Tienda_Celulares.Web.Models;


namespace Tienda_Celulares.Web.Models
{
    public class DetalleVenta
    {
        [Key]
        public int id_detalle { get; set; }      // PK identity en la BD

        public int id_venta { get; set; }       // FK -> venta.id_venta
        public int? id_producto { get; set; }   // FK -> producto.id_producto (puede ser null si usa numero_serie)
        public string? numero_serie { get; set; }

        public int cantidad { get; set; }
        public decimal precio_unitario { get; set; }

        public Venta? Venta { get; set; }
        public Producto? Producto { get; set; }
    }
}
