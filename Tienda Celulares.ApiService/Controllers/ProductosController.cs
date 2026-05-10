using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
using Tienda_Celulares.ApiService.Data;
using Tienda_Celulares.ApiService.Models;


namespace Tienda_Celulares.ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductosController(AppDbContext context)
        {
            _context = context;
        }

        // LISTAR
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> Get()
        {
            return await _context.Productos
            .Include(p => p.Marca)     // Carga los datos de la tabla marca
            .Include(p => p.Categoria) // Carga los datos de la tabla categoria
            .ToListAsync();
        }

        // BUSCAR POR ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetById(int id)
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
                return NotFound();

            return producto;
        }

        // CREAR
        [HttpPost]
        public async Task<ActionResult<Producto>> Post(Producto producto)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();


            return CreatedAtAction(nameof(GetById),
                new { id = producto.id_producto }, producto);
        }

        // EDITAR
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Producto producto)
        {
            if (id != producto.id_producto)
                return BadRequest();

            _context.Entry(producto).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ELIMINAR
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
                return NotFound();

            _context.Productos.Remove(producto);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}