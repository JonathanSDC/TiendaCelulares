using global::Tienda_Celulares.ApiService.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRUD.Shared.Models;


namespace Tienda_Celulares.ApiService.Controllers
{
    
    namespace Tienda_Celulares.ApiService.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class MarcasController : ControllerBase
        {
            private readonly AppDbContext _context;

            public MarcasController(AppDbContext context)
            {
                _context = context;
            }

            // LISTAR MARCAS
            [HttpGet]
            public async Task<ActionResult<IEnumerable<Marca>>> Get()
            {
                var marcas = await _context.Marcas
            .OrderBy(m => m.Nombre) // usa la propiedad C# mapeada
            .ToListAsync();

                return Ok(marcas);
            }

            // BUSCAR MARCA POR ID
            [HttpGet("{id}")]
            public async Task<ActionResult<Marca>> GetById(int id)
            {
                var marca = await _context.Marcas.FindAsync(id);
                if (marca == null) return NotFound();
                return marca;
            }
        }
    }
}
