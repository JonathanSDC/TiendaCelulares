using Microsoft.EntityFrameworkCore;
using Tienda_Celulares.ApiService.Data;
using CRUD.Shared.Models.ViewModel;
using CRUD.Shared.Models;


namespace Tienda_Celulares.ApiService.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Linq;

    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly AppDbContext _db;

        public ProductosController(AppDbContext db)
        {
            _db = db ?? throw new System.ArgumentNullException(nameof(db));
        }

        // LISTAR
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoViewModel>>> GetAll()
        {
            var productos = await _db.Productos
                .Include(p => p.Marca)
                .Include(p => p.Categoria)
                .Select(p => new ProductoViewModel
                {
                    IdProducto = p.id_producto,
                    NombreModelo = p.nombre_modelo,
                    Descripcion = p.descripcion,
                    PrecioActual = p.precio_actual,
                    TipoProducto = p.tipo_producto,
                    IdMarca = p.id_marca,
                    MarcaNombre = p.Marca != null ? p.Marca.Nombre : string.Empty,
                    IdCategoria = p.id_categoria,
                    CategoriaNombre = p.Categoria != null ? p.Categoria.Nombre : string.Empty
                })
                .ToListAsync();

            return Ok(productos);
        }

        // OBTENER POR ID
        [HttpGet("{id}", Name = "GetProductoById")]
        public async Task<ActionResult<ProductoViewModel>> GetById(int id)
        {
            var producto = await _db.Productos
                .Include(p => p.Marca)
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.id_producto == id);

            if (producto == null) return NotFound();

            var vm = new ProductoViewModel
            {
                IdProducto = producto.id_producto,
                NombreModelo = producto.nombre_modelo,
                Descripcion = producto.descripcion,
                PrecioActual = producto.precio_actual,
                TipoProducto = producto.tipo_producto,
                IdMarca = producto.id_marca,
                MarcaNombre = producto.Marca?.Nombre ?? string.Empty,
                IdCategoria = producto.id_categoria,
                CategoriaNombre = producto.Categoria?.Nombre ?? string.Empty
            };

            return Ok(vm);
        }

        // CREAR
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProductoViewModel model)
        {
            if (model == null) return BadRequest();

            // Validación sincronizada con el CHECK de SQL
            var allowed = new[] { "telefono", "tablet", "accesorio", "repuesto" };
            if (!allowed.Contains(model.TipoProducto))
                return BadRequest($"tipo_producto inválido. Valores permitidos: {string.Join(", ", allowed)}");

            var producto = new Producto
            {
                nombre_modelo = model.NombreModelo,
                descripcion = model.Descripcion,
                precio_actual = model.PrecioActual,
                tipo_producto = model.TipoProducto,
                id_marca = model.IdMarca,
                id_categoria = model.IdCategoria
            };

            _db.Productos.Add(producto);
            await _db.SaveChangesAsync();

            var vm = new ProductoViewModel
            {
                IdProducto = producto.id_producto,
                NombreModelo = producto.nombre_modelo,
                Descripcion = producto.descripcion,
                PrecioActual = producto.precio_actual,
                TipoProducto = producto.tipo_producto,
                IdMarca = producto.id_marca,
                MarcaNombre = (await _db.Marcas.FindAsync(producto.id_marca))?.Nombre ?? string.Empty,
                IdCategoria = producto.id_categoria,
                CategoriaNombre = (await _db.Categorias.FindAsync(producto.id_categoria))?.Nombre ?? string.Empty
            };

            return CreatedAtRoute("GetProductoById", new { id = producto.id_producto }, vm);
        }

        // EDITAR
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ProductoViewModel model)
        {
            if (model == null) return BadRequest();

            var producto = await _db.Productos.FindAsync(id);
            if (producto == null) return NotFound();

            producto.nombre_modelo = model.NombreModelo;
            producto.descripcion = model.Descripcion;
            producto.precio_actual = model.PrecioActual;
            producto.tipo_producto = model.TipoProducto;
            producto.id_marca = model.IdMarca;
            producto.id_categoria = model.IdCategoria;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        // ELIMINAR
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var producto = await _db.Productos.FindAsync(id);
            if (producto == null) return NotFound();

            _db.Productos.Remove(producto);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
