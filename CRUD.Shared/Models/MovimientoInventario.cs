using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.Shared.Models
{
    public class MovimientoInventario
    {
        public int IdMovimiento { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string Referencia { get; set; } = string.Empty;

        public string? NumeroSerie { get; set; }
        public int? IdProducto { get; set; }
        public int Cantidad { get; set; }

        // FK y navegación para la tienda general (si la necesitas)
        public int? IdTienda { get; set; }
        [ForeignKey(nameof(IdTienda))]
        public Tienda? Tienda { get; set; }

        // Producto (asegúrate que existe IdProducto arriba)
        [ForeignKey(nameof(IdProducto))]
        public Producto? Producto { get; set; }

        // Origen
        public int? IdTiendaOrigen { get; set; }
        [ForeignKey(nameof(IdTiendaOrigen))]
        public Tienda? TiendaOrigen { get; set; }

        // Destino
        public int? IdTiendaDestino { get; set; }
        [ForeignKey(nameof(IdTiendaDestino))]
        public Tienda? TiendaDestino { get; set; }
    }

}
