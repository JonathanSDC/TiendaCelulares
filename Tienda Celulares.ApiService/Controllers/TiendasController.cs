using CRUD.Shared.Dto;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TiendaDto>>> GetTiendas()
        {
            var tiendas = await _context.Tiendas
                .Select(t => new TiendaDto
                {
                    Id = t.IdTienda,
                    Nombre = t.Nombre
                    
                })
                .ToListAsync();

            return Ok(tiendas);
        }
    }
}
