using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tienda_Celulares.ApiService.Data;
using Tienda_Celulares.ApiService.Models;
using Tienda_Celulares.ApiService.Models.ViewModel;


namespace Tienda_de_Celulares.ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClientesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> Get()
        {
            var clientes = await _context.Clientes
         .Include(c => c.Persona)
         .Select(c => new ClienteViewModel
         {
             IdPersona = c.IdPersona,
             // Usamos condicionales simples en lugar de ?. para evitar el error CS8072
             Nombre = c.Persona != null ? c.Persona.Nombre : "Sin Nombre",
             Apellido = c.Persona != null ? c.Persona.Apellido : "",
             Telefono = c.Persona != null ? c.Persona.Telefono : "",
             Email = c.Persona != null ? c.Persona.Email : "",
             TipoCliente = c.TipoCliente ?? "Natural",
             FechaRegistro = c.FechaRegistro
         })
         .ToListAsync();

            return Ok(clientes);
        }

        [HttpPost]
        public async Task<IActionResult> Post(ClienteViewModel modelo)
        {
            // Verificar si el email ya existe antes de empezar la transacción
            var existeEmail = await _context.Personas.AnyAsync(p => p.Email == modelo.Email);
            if (existeEmail)
            {
                return BadRequest("El correo electrónico ya está registrado con otro cliente.");
            }


            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Creamos y guardamos la Persona primero (para obtener el ID)
                var nuevaPersona = new Persona
                {
                    Nombre = modelo.Nombre,
                    Apellido = modelo.Apellido,
                    Telefono = modelo.Telefono,
                    Email = modelo.Email
                    // id_direccion se queda null por ahora según tu SQL
                };

                _context.Personas.Add(nuevaPersona);
                await _context.SaveChangesAsync();

                // 2. Ahora que ya tenemos el ID de la persona, creamos el Cliente
                var nuevoCliente = new Cliente
                {
                    IdPersona = nuevaPersona.IdPersona, // El ID generado automáticamente
                    FechaRegistro = modelo.FechaRegistro,
                    TipoCliente = modelo.TipoCliente
                };

                _context.Clientes.Add(nuevoCliente);
                await _context.SaveChangesAsync();

                // 3. Si todo salió bien, confirmamos la transacción en la BD
                await transaction.CommitAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                // Esto extraerá el error real de SQL (como "llave foránea" o "dato demasiado largo")
                var innerError = ex.InnerException?.Message ?? ex.Message;
                return BadRequest($"Error real de SQL: {innerError}");
            }
        }
    }
}