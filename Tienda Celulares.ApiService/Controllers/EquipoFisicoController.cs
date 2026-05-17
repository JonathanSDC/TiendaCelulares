using CRUD.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Tienda_Celulares.ApiService.Data;
using Microsoft.EntityFrameworkCore;


namespace Tienda_Celulares.ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EquipoFisicoController : ControllerBase
    {
        private readonly AppDbContext _context;
        public EquipoFisicoController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _context.EquiposFisicos.Include(e => e.Producto).Include(e => e.Tienda).ToListAsync());

        [HttpGet("{serie}")]
        public async Task<IActionResult> Get(string serie)
        {
            var e = await _context.EquiposFisicos.FindAsync(serie);
            if (e == null) return NotFound();
            return Ok(e);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EquipoFisico equipo)
        {
            if (await _context.EquiposFisicos.AnyAsync(x => x.NumeroSerie == equipo.NumeroSerie))
                return Conflict("Número de serie ya existe.");

            _context.EquiposFisicos.Add(equipo);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { serie = equipo.NumeroSerie }, equipo);
        }

        [HttpPut("{serie}")]
        public async Task<IActionResult> Update(string serie, [FromBody] EquipoFisico updated)
        {
            if (serie != updated.NumeroSerie) return BadRequest("Serie mismatch.");
            var existing = await _context.EquiposFisicos.FindAsync(serie);
            if (existing == null) return NotFound();

            existing.Color = updated.Color;
            existing.Estado = updated.Estado;
            existing.IdTienda = updated.IdTienda;
            existing.IdProducto = updated.IdProducto;

            _context.EquiposFisicos.Update(existing);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{serie}")]
        public async Task<IActionResult> Delete(string serie)
        {
            var existing = await _context.EquiposFisicos.FindAsync(serie);
            if (existing == null) return NotFound();
            _context.EquiposFisicos.Remove(existing);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        




    
    }

}
