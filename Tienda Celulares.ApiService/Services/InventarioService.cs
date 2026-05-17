using CRUD.Shared.Dto;

namespace Tienda_Celulares.ApiService.Services
{
    public class InventarioService
    {
        private readonly HttpClient _http;
        public InventarioService(HttpClient http) => _http = http;

        public async Task<HttpResponseMessage> AjustarConSpAsync(MovimientoInventarioDto dto)
        {
            return await _http.PostAsJsonAsync("api/inventario/ajustar-sp", dto);
        }
    }
}
