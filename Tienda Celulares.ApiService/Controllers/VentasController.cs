using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tienda_Celulares.ApiService.Data;
using CRUD.Shared.Models;




namespace Tienda_Celulares.ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VentasController(AppDbContext context)
        {
            _context = context;
        }

        // ===========================
        // LISTAR
        // ===========================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VentaViewModel>>> GetAll()
        {
            var ventas = await _context.Ventas
                .Include(v => v.Cliente)
                    .ThenInclude(c => c.Persona)
                .Select(v => new VentaViewModel
                {
                    IdVenta = v.id_venta,
                    FechaHora = v.fecha_hora,
                    TotalVenta = v.total_venta,
                    Estado = v.estado,
                    IdCliente = v.id_cliente,
                    IdEmpleado = v.id_empleado,
                    IdMetodo = v.id_metodo,
                    IdTienda = v.id_tienda,
                    NombreCliente = v.Cliente != null && v.Cliente.Persona != null
                        ? v.Cliente.Persona.Nombre
                        : "Consumidor Final"
                })
                .ToListAsync();

            return Ok(ventas);
        }

        // ===========================
        // OBTENER POR ID
        // ===========================
        [HttpGet("{id}", Name = "GetVentaById")]
        public async Task<ActionResult<VentaViewModel>> GetById(int id)
        {
            var venta = await _context.Ventas
                .Include(v => v.Cliente)
                    .ThenInclude(c => c.Persona)
                .FirstOrDefaultAsync(v => v.id_venta == id);

            if (venta == null) return NotFound();

            var vm = new VentaViewModel
            {
                IdVenta = venta.id_venta,
                FechaHora = venta.fecha_hora,
                TotalVenta = venta.total_venta,
                Estado = venta.estado,
                IdCliente = venta.id_cliente,
                IdEmpleado = venta.id_empleado,
                IdMetodo = venta.id_metodo,
                IdTienda = venta.id_tienda,
                NombreCliente = venta.Cliente?.Persona?.Nombre ?? "Consumidor Final"
            };

            return Ok(vm);
        }

        // ===========================
        // CREAR
        // ===========================
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] VentaViewModel model)
        {
            if (model == null) return BadRequest();

            // Validaciones básicas (ajusta según reglas de negocio)
            var venta = new Venta
            {
                fecha_hora = DateTime.Now,
                total_venta = model.TotalVenta,
                estado = model.Estado ?? "pendiente",
                id_cliente = model.IdCliente,
                id_empleado = model.IdEmpleado,
                id_metodo = model.IdMetodo,
                id_tienda = model.IdTienda
            };

            _context.Ventas.Add(venta);
            await _context.SaveChangesAsync();

            // Devolver Created con la ruta al recurso creado
            var vm = new VentaViewModel
            {
                IdVenta = venta.id_venta,
                FechaHora = venta.fecha_hora,
                TotalVenta = venta.total_venta,
                Estado = venta.estado,
                IdCliente = venta.id_cliente,
                IdEmpleado = venta.id_empleado,
                IdMetodo = venta.id_metodo,
                IdTienda = venta.id_tienda,
                NombreCliente = venta.id_cliente.HasValue
                    ? (await _context.Clientes.Include(c => c.Persona)
                                              .FirstOrDefaultAsync(c => c.IdPersona == venta.id_cliente))?.Persona?.Nombre
                      ?? "Consumidor Final"
                    : "Consumidor Final"
            };

            return CreatedAtRoute("GetVentaById", new { id = venta.id_venta }, vm);
        }

        // ===========================
        // EDITAR
        // ===========================
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] VentaViewModel model)
        {
            if (model == null) return BadRequest();

            var venta = await _context.Ventas.FindAsync(id);
            if (venta == null) return NotFound();

            // Actualizar campos permitidos
            venta.estado = model.Estado ?? venta.estado;
            venta.total_venta = model.TotalVenta;

            // Si necesitas actualizar relaciones (cliente, empleado, etc.) valida existencia antes
            venta.id_cliente = model.IdCliente;
            venta.id_empleado = model.IdEmpleado;
            venta.id_metodo = model.IdMetodo;
            venta.id_tienda = model.IdTienda;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ===========================
        // ELIMINAR
        // ===========================
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var venta = await _context.Ventas.FindAsync(id);
            if (venta == null) return NotFound();

            // Si existen detalles de venta y quieres borrarlos en cascada, asegúrate de que la FK tenga ON DELETE CASCADE
            // o elimina manualmente los detalles antes de borrar la venta.
            var detalles = await _context.DetalleVentas.Where(d => d.id_venta == id).ToListAsync();
            if (detalles.Any())
            {
                _context.DetalleVentas.RemoveRange(detalles);
            }

            _context.Ventas.Remove(venta);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

    // ViewModel usado por el controlador
    public class VentaViewModel
    {
        public int IdVenta { get; set; }
        public DateTime FechaHora { get; set; }
        public decimal TotalVenta { get; set; }
        public string? Estado { get; set; }

        public int? IdCliente { get; set; }
        public int? IdEmpleado { get; set; }
        public int? IdMetodo { get; set; }
        public int? IdTienda { get; set; }

        public string NombreCliente { get; set; } = string.Empty;
    }
}

