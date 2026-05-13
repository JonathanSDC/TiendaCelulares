using CRUD.Shared.Dto;
using CRUD.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Tienda_Celulares.ApiService.Data;
using Microsoft.EntityFrameworkCore;


namespace Tienda_Celulares.ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TiendasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TiendasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/tiendas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TiendaDto>>> GetTiendas()
        {
            var tiendas = await _context.Tiendas
                .Include(t => t.Direccion)
                .Select(t => new TiendaDto
                {
                    Id = t.IdTienda,
                    Nombre = t.Nombre,
                    Calle = t.Direccion.Calle,
                    Ciudad = t.Direccion.Ciudad,
                    Departamento = t.Direccion.Departamento,
                    Pais = t.Direccion.Pais
                })

                .ToListAsync();

            return Ok(tiendas);
        }

        // POST: api/tiendas
        [HttpPost]
        public async Task<ActionResult<TiendaDto>> CrearTienda([FromBody] TiendaDto dto)
        {
            var direccion = new Direccion
            {
                Calle = dto.Calle,
                Ciudad = dto.Ciudad,
                Departamento = dto.Departamento,
                Pais = dto.Pais
            };

            _context.Direcciones.Add(direccion);
            await _context.SaveChangesAsync();

            var tienda = new Tienda
            {
                Nombre = dto.Nombre,
                IdDireccion = direccion.IdDireccion
            };

            _context.Tiendas.Add(tienda);
            await _context.SaveChangesAsync();

            dto.Id = tienda.IdTienda;
            return CreatedAtAction(nameof(GetTiendas), new { id = tienda.IdTienda }, dto);
        }

    }
}
