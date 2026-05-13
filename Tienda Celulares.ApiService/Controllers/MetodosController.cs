using CRUD.Shared.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tienda_Celulares.ApiService.Data;

namespace Tienda_Celulares.ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MetodosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MetodosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MetodoPagoDto>>> GetMetodos()
        {
            var metodos = await _context.MetodosPago
                .Select(m => new MetodoPagoDto
                {
                    Id = m.IdMetodo,
                    Nombre = m.TipoMetodo
                })
                .ToListAsync();

            return Ok(metodos);
        }
    }

}
