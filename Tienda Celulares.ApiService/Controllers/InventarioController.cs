using CRUD.Shared.Dto;
using CRUD.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tienda_Celulares.ApiService.Data;

namespace Tienda_Celulares.ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventarioController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        // UN SOLO constructor público para evitar ambigüedad en DI
        public InventarioController(AppDbContext context, IConfiguration configuration)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

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
                    NombreProducto = i.Producto!.nombre_modelo,
                    IdTienda = i.IdTienda,
                    NombreTienda = i.Tienda!.Nombre,
                    Cantidad = i.Cantidad,
                    StockMinimo = i.StockMinimo
                })
                .ToListAsync();

            return Ok(inventario);
        }

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
                    NombreProducto = i.Producto!.nombre_modelo,
                    IdTienda = i.IdTienda,
                    NombreTienda = i.Tienda!.Nombre,
                    Cantidad = i.Cantidad,
                    StockMinimo = i.StockMinimo
                })
                .FirstOrDefaultAsync();

            if (inventario == null) return NotFound();
            return Ok(inventario);
        }

        [HttpPost]
        public async Task<ActionResult<InventarioDto>> CrearInventario([FromBody] InventarioDto dto)
        {
            if (dto == null) return BadRequest("Cuerpo vacío.");

            var producto = await _context.Productos.FindAsync(dto.IdProducto);
            var tienda = await _context.Tiendas.FindAsync(dto.IdTienda);
            if (producto == null || tienda == null) return BadRequest("Producto o tienda no existe.");

            var existente = await _context.Inventarios
                .FirstOrDefaultAsync(i => i.IdProducto == dto.IdProducto && i.IdTienda == dto.IdTienda);
            if (existente != null) return Conflict("Ya existe inventario para ese producto en la tienda.");

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
            dto.NombreProducto = producto.nombre_modelo;
            dto.NombreTienda = tienda.Nombre;

            return CreatedAtAction(nameof(GetInventarioById), new { id = inventario.IdInventario }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarInventario(int id, [FromBody] InventarioDto dto)
        {
            if (dto == null) return BadRequest("Cuerpo vacío.");

            var inventario = await _context.Inventarios.FindAsync(id);
            if (inventario == null) return NotFound();

            inventario.Cantidad = dto.Cantidad;
            inventario.StockMinimo = dto.StockMinimo;

            _context.Inventarios.Update(inventario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarInventario(int id)
        {
            var inventario = await _context.Inventarios.FindAsync(id);
            if (inventario == null) return NotFound();

            _context.Inventarios.Remove(inventario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("ajustar-sp")]
        public async Task<IActionResult> AjustarConSp([FromBody] MovimientoInventarioDto dto)
        {
            if (dto == null) return BadRequest("Cuerpo vacío.");

            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@Tipo", dto.Tipo ?? (object)DBNull.Value),
                    new SqlParameter("@IdProducto", dto.IdProducto ?? (object)DBNull.Value),
                    new SqlParameter("@Cantidad", dto.Cantidad),
                    new SqlParameter("@IdTiendaOrigen", dto.IdTiendaOrigen ?? (object)DBNull.Value),
                    new SqlParameter("@IdTiendaDestino", dto.IdTiendaDestino ?? (object)DBNull.Value),
                    new SqlParameter("@NumeroSerie", dto.NumeroSerie ?? (object)DBNull.Value),
                    new SqlParameter("@Referencia", dto.Referencia ?? (object)DBNull.Value)
                };

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC sp_AjustarInventario @Tipo, @IdProducto, @Cantidad, @IdTiendaOrigen, @IdTiendaDestino, @NumeroSerie, @Referencia",
                    parameters);

                return Ok(new { mensaje = "Movimiento registrado" });
            }
            catch (SqlException ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // Ajuste en C# (opcional)...
        [HttpPost("ajustar")]
        public async Task<IActionResult> AjustarStock([FromBody] MovimientoInventarioDto dto)
        {
            if (dto == null) return BadRequest("Cuerpo vacío.");

            using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                if (dto.IdProducto == null && string.IsNullOrEmpty(dto.NumeroSerie))
                    return BadRequest("Debe indicar id_producto o numero_serie.");

                if (!string.IsNullOrEmpty(dto.NumeroSerie))
                {
                    var equipo = await _context.EquiposFisicos
                        .FirstOrDefaultAsync(e => e.NumeroSerie == dto.NumeroSerie);
                    if (equipo == null) return BadRequest("Número de serie no encontrado.");

                    if (dto.Tipo.Equals("Traslado", StringComparison.OrdinalIgnoreCase) && dto.IdTiendaDestino.HasValue)
                    {
                        equipo.IdTienda = dto.IdTiendaDestino.Value;
                        _context.EquiposFisicos.Update(equipo);
                    }

                    dto.Cantidad = 1;
                    dto.IdProducto = equipo.IdProducto;
                }

                Inventario? inventarioOrigen = null;
                Inventario? inventarioDestino = null;

                if (dto.IdTiendaOrigen.HasValue)
                {
                    inventarioOrigen = await _context.Inventarios
                        .FirstOrDefaultAsync(i => i.IdProducto == dto.IdProducto && i.IdTienda == dto.IdTiendaOrigen.Value);
                }

                if (dto.IdTiendaDestino.HasValue)
                {
                    inventarioDestino = await _context.Inventarios
                        .FirstOrDefaultAsync(i => i.IdProducto == dto.IdProducto && i.IdTienda == dto.IdTiendaDestino.Value);
                }

                if (dto.Tipo.Equals("Entrada", StringComparison.OrdinalIgnoreCase))
                {
                    if (inventarioDestino == null)
                    {
                        inventarioDestino = new Inventario
                        {
                            IdProducto = dto.IdProducto!.Value,
                            IdTienda = dto.IdTiendaDestino!.Value,
                            Cantidad = dto.Cantidad,
                            StockMinimo = 0
                        };
                        _context.Inventarios.Add(inventarioDestino);
                    }
                    else
                    {
                        inventarioDestino.Cantidad += dto.Cantidad;
                        _context.Inventarios.Update(inventarioDestino);
                    }
                }
                else if (dto.Tipo.Equals("Salida", StringComparison.OrdinalIgnoreCase))
                {
                    if (inventarioOrigen == null) return BadRequest("No hay inventario en la tienda origen.");
                    if (inventarioOrigen.Cantidad < dto.Cantidad) return BadRequest("Stock insuficiente.");
                    inventarioOrigen.Cantidad -= dto.Cantidad;
                    _context.Inventarios.Update(inventarioOrigen);
                }
                else if (dto.Tipo.Equals("Traslado", StringComparison.OrdinalIgnoreCase))
                {
                    if (inventarioOrigen == null) return BadRequest("No hay inventario en la tienda origen.");
                    if (inventarioOrigen.Cantidad < dto.Cantidad) return BadRequest("Stock insuficiente en origen.");

                    inventarioOrigen.Cantidad -= dto.Cantidad;
                    _context.Inventarios.Update(inventarioOrigen);

                    if (inventarioDestino == null)
                    {
                        inventarioDestino = new Inventario
                        {
                            IdProducto = dto.IdProducto!.Value,
                            IdTienda = dto.IdTiendaDestino!.Value,
                            Cantidad = dto.Cantidad,
                            StockMinimo = 0
                        };
                        _context.Inventarios.Add(inventarioDestino);
                    }
                    else
                    {
                        inventarioDestino.Cantidad += dto.Cantidad;
                        _context.Inventarios.Update(inventarioDestino);
                    }
                }
                else
                {
                    return BadRequest("Tipo de movimiento no soportado.");
                }

                var movimiento = new MovimientoInventario
                {
                    Tipo = dto.Tipo,
                    Fecha = dto.Fecha == default ? DateTime.UtcNow : dto.Fecha,
                    Referencia = dto.Referencia,
                    NumeroSerie = dto.NumeroSerie,
                    IdProducto = dto.IdProducto,
                    Cantidad = dto.Cantidad,
                    IdTiendaOrigen = dto.IdTiendaOrigen,
                    IdTiendaDestino = dto.IdTiendaDestino
                };
                _context.MovimientosInventario.Add(movimiento);

                await _context.SaveChangesAsync();
                await tx.CommitAsync();

                return Ok(new { mensaje = "Movimiento registrado", movimientoId = movimiento.IdMovimiento });
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/inventario/equipos
        [HttpGet("equipos")]
        public async Task<IActionResult> GetEquiposFisicos()
        {
            var equipos = await _context.EquiposFisicos
                .Include(e => e.Producto)
                .Include(e => e.Tienda)
                .Select(e => new {
                    e.NumeroSerie,
                    Producto = e.Producto!.nombre_modelo,
                    e.Estado,
                    e.Color,
                    Tienda = e.Tienda!.Nombre
                })
                .ToListAsync();

            return Ok(equipos);
        }

        // GET: api/inventario/movimientos
        [HttpGet("movimientos")]
        public async Task<IActionResult> GetMovimientos([FromQuery] int? idProducto = null, [FromQuery] int? idTienda = null)
        {
            var q = _context.MovimientosInventario
                .Include(m => m.Producto)
                .Include(m => m.TiendaOrigen)
                .Include(m => m.TiendaDestino)
                .AsQueryable();

            if (idProducto.HasValue) q = q.Where(m => m.IdProducto == idProducto.Value);
            if (idTienda.HasValue) q = q.Where(m => m.IdTiendaOrigen == idTienda.Value || m.IdTiendaDestino == idTienda.Value);

            var movimientos = await q
                .OrderByDescending(m => m.Fecha)
                .Select(m => new {
                    m.IdMovimiento,
                    Fecha = m.Fecha,
                    Tipo = m.Tipo,
                    Producto = m.Producto != null ? m.Producto.nombre_modelo : null,
                    NumeroSerie = m.NumeroSerie,
                    Cantidad = m.Cantidad,
                    Referencia = m.Referencia,
                    IdTiendaOrigen = m.IdTiendaOrigen,
                    TiendaOrigen = m.TiendaOrigen != null ? m.TiendaOrigen.Nombre : null,
                    IdTiendaDestino = m.IdTiendaDestino,
                    TiendaDestino = m.TiendaDestino != null ? m.TiendaDestino.Nombre : null
                })
                .ToListAsync();

            return Ok(movimientos);
        }

    }
}
