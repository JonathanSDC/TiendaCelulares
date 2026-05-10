using System.Buffers.Text;
using System.Net.Http.Json;
using Tienda_Celulares.Web.Models;


namespace Tienda_Celulares.Web.Services
{
    public class ProductoService
    {
        private readonly HttpClient _http;

        private readonly string url =
            "https://localhost:7473/api/productos";

        // 1. Define la URL base de la API General solo para corregir CAMBIAR DESPUES
        private readonly string apiBase = "https://localhost:7473/api";

        public ProductoService(HttpClient http)
        {
            _http = http;
        }

        // LISTAR
        public async Task<List<Producto>> ObtenerProductos()
        {
            return await _http.GetFromJsonAsync<List<Producto>>(url)
                ?? new List<Producto>();
        }

        // CREAR
        public async Task CrearProducto(Producto producto)
        {
            var response = await _http.PostAsJsonAsync(url, producto);
            // Esto lanzará una excepción si la API devuelve error (ej. 400 Bad Request)
            response.EnsureSuccessStatusCode();
        }

        // EDITAR
        public async Task EditarProducto(Producto producto)
        {
            var response = await _http.PutAsJsonAsync($"{url}/{producto.id_producto}", producto);
            response.EnsureSuccessStatusCode();
        }

        // ELIMINAR
        public async Task EliminarProducto(int id)
        {
            await _http.DeleteAsync($"{url}/{id}");
          
        }

        public async Task<List<Marca>> ObtenerMarcas()
        => await _http.GetFromJsonAsync<List<Marca>>($"{apiBase}/marcas") ?? new();

        public async Task<List<Categoria>> ObtenerCategorias()
            => await _http.GetFromJsonAsync<List<Categoria>>($"{apiBase}/categorias") ?? new();
    }
}