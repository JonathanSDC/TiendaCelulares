using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tienda_Celulares.ApiService.Data;
using CRUD.Shared.Models;
using CRUD.Shared.Models.ViewModel;


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

        // DELETE: api/clientes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound("Cliente no encontrado.");
            }

            // También eliminar la persona asociada si corresponde
            var persona = await _context.Personas.FindAsync(cliente.IdPersona);

            _context.Clientes.Remove(cliente);
            if (persona != null)
            {
                _context.Personas.Remove(persona);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        // PUT: api/clientes/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, ClienteViewModel modelo)
        {
            var cliente = await _context.Clientes
                .Include(c => c.Persona)
                .FirstOrDefaultAsync(c => c.IdPersona == id);

            if (cliente == null)
            {
                return NotFound("Cliente no encontrado.");
            }

            // Actualizar persona
            if (cliente.Persona != null)
            {
                cliente.Persona.Nombre = modelo.Nombre;
                cliente.Persona.Apellido = modelo.Apellido;
                cliente.Persona.Telefono = modelo.Telefono;
                cliente.Persona.Email = modelo.Email;
            }

            // Actualizar cliente
            cliente.TipoCliente = modelo.TipoCliente;
            cliente.FechaRegistro = modelo.FechaRegistro;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteViewModel>> GetById(int id)
        {
            var cliente = await _context.Clientes
                .Include(c => c.Persona)
                .Where(c => c.IdPersona == id)
                .Select(c => new ClienteViewModel
                {
                    IdPersona = c.IdPersona,
                    Nombre = c.Persona != null ? c.Persona.Nombre : "",
                    Apellido = c.Persona != null ? c.Persona.Apellido : "",
                    Telefono = c.Persona != null ? c.Persona.Telefono : "",
                    Email = c.Persona != null ? c.Persona.Email : "",
                    TipoCliente = c.TipoCliente ?? "Natural",
                    FechaRegistro = c.FechaRegistro
                })
                .FirstOrDefaultAsync();

            if (cliente == null) return NotFound();

            return Ok(cliente);
        }




    }
}