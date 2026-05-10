using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tienda_Celulares.ApiService.Data;
using Tienda_Celulares.ApiService.Models;

namespace Tienda_Celulares.ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        // LISTAR CATEGORÍAS
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> Get()
        {
            var categorias = await _context.Categorias
            .OrderBy(c => c.Nombre) // propiedad C# mapeada a nombre_categoria
            .ToListAsync();

            return Ok(categorias);
        }

        // BUSCAR CATEGORÍA POR ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Categoria>> GetById(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null) return NotFound();
            return categoria;
        }
    }
}