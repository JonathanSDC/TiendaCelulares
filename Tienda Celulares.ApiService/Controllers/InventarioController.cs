using CRUD.Shared.Dto;
using CRUD.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tienda_Celulares.ApiService.Data;


namespace Tienda_Celulares.ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventarioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InventarioController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/inventario
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventarioDto>>> GetInventario()
        {
            var inventario = await _context.Inventarios
                .Include(i => i.Producto)
                .Include(i => i.Tienda)
                .Select(i => new InventarioDto
                {
                    IdInventario = i.IdInventario,
                    IdProducto = i.IdProducto,
                    NombreProducto = i.Producto.nombre_modelo,
                    IdTienda = i.IdTienda,
                    NombreTienda = i.Tienda.Nombre,
                    Cantidad = i.Cantidad,
                    StockMinimo = i.StockMinimo
                })
                .ToListAsync();

            return Ok(inventario);
        }

        // GET: api/inventario/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<InventarioDto>> GetInventarioById(int id)
        {
            var inventario = await _context.Inventarios
                .Include(i => i.Producto)
                .Include(i => i.Tienda)
                .Where(i => i.IdInventario == id)
                .Select(i => new InventarioDto
                {
                    IdInventario = i.IdInventario,
                    IdProducto = i.IdProducto,
                    NombreProducto = i.Producto.nombre_modelo,
                    IdTienda = i.IdTienda,
                    NombreTienda = i.Tienda.Nombre,
                    Cantidad = i.Cantidad,
                    StockMinimo = i.StockMinimo
                })
                .FirstOrDefaultAsync();

            if (inventario == null)
                return NotFound();

            return Ok(inventario);
        }

        // POST: api/inventario
        [HttpPost]
        public async Task<ActionResult<InventarioDto>> CrearInventario([FromBody] InventarioDto dto)
        {
            var inventario = new Inventario
            {
                IdProducto = dto.IdProducto,
                IdTienda = dto.IdTienda,
                Cantidad = dto.Cantidad,
                StockMinimo = dto.StockMinimo
            };

            _context.Inventarios.Add(inventario);
            await _context.SaveChangesAsync();

            dto.IdInventario = inventario.IdInventario;
            return CreatedAtAction(nameof(GetInventarioById), new { id = inventario.IdInventario }, dto);
        }

        // PUT: api/inventario/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarInventario(int id, [FromBody] InventarioDto dto)
        {
            var inventario = await _context.Inventarios.FindAsync(id);
            if (inventario == null)
                return NotFound();

            inventario.Cantidad = dto.Cantidad;
            inventario.StockMinimo = dto.StockMinimo;

            _context.Inventarios.Update(inventario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/inventario/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarInventario(int id)
        {
            var inventario = await _context.Inventarios.FindAsync(id);
            if (inventario == null)
                return NotFound();

            _context.Inventarios.Remove(inventario);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
