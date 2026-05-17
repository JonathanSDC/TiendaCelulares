using CRUD.Shared.Dto;
using CRUD.Shared.Models;
using CRUD.Shared.Models.ViewModel;
using System.Buffers.Text;
using System.Net.Http.Json;
using System.Text.Json;

namespace Tienda_Celulares.Web.Services
{
    public class ProductoService
    {
        private readonly HttpClient _http;
        private readonly string url = "https://localhost:7473/api/productos";
        private readonly string apiBase = "https://localhost:7473/api";

        public ProductoService(HttpClient http)
        {
            _http = http;
        }

        // LISTAR
        public async Task<List<ProductoViewModel>> ObtenerProductos()
        {
            return await _http.GetFromJsonAsync<List<ProductoViewModel>>(url)
                ?? new List<ProductoViewModel>();
        }

        // CREAR
        public async Task CrearProducto(ProductoViewModel producto)
        {
            Console.WriteLine($"TipoProducto enviado: {producto.TipoProducto}");

            var response = await _http.PostAsJsonAsync(url, producto);
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine(content);
            response.EnsureSuccessStatusCode();
        }

        // EDITAR
        public async Task EditarProducto(ProductoViewModel producto)
        {
            var response = await _http.PutAsJsonAsync($"{url}/{producto.IdProducto}", producto);
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine(content);
            response.EnsureSuccessStatusCode();
        }

        // ELIMINAR
        public async Task EliminarProducto(int id)
        {
            await _http.DeleteAsync($"{url}/{id}");
        }

        // OBTENER POR ID
        public async Task<ProductoViewModel> ObtenerProductoPorId(int id)
        {
            return await _http.GetFromJsonAsync<ProductoViewModel>($"{url}/{id}")
                   ?? new ProductoViewModel();
        }

        public async Task<List<Marca>> ObtenerMarcas()
            => await _http.GetFromJsonAsync<List<Marca>>($"{apiBase}/marcas") ?? new();

        public async Task<List<Categoria>> ObtenerCategorias()
            => await _http.GetFromJsonAsync<List<Categoria>>($"{apiBase}/categorias") ?? new();
    }
}
