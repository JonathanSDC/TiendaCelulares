using CRUD.Shared.Dto;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class InventarioService
{
    private readonly HttpClient _http;
    public InventarioService(HttpClient http) => _http = http;

    public async Task<HttpResponseMessage> AjustarConSpAsync(MovimientoInventarioDto dto)
    {
        return await _http.PostAsJsonAsync("api/inventario/ajustar-sp", dto);
    }

    public async Task<List<EquipoDto>> GetEquiposAsync()
    {
        return await _http.GetFromJsonAsync<List<EquipoDto>>("api/inventario/equipos")
               ?? new List<EquipoDto>();
    }

    public async Task<List<MovimientoDto>> GetMovimientosAsync()
    {
        return await _http.GetFromJsonAsync<List<MovimientoDto>>("api/inventario/movimientos")
               ?? new List<MovimientoDto>();
    }



}

public class EquipoDto
{
    public string NumeroSerie { get; set; } = string.Empty;
    public string Producto { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string Tienda { get; set; } = string.Empty;
}

public class MovimientoDto
{
    public int IdMovimiento { get; set; }
    public DateTime Fecha { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string Producto { get; set; } = string.Empty;
    public string? NumeroSerie { get; set; }
    public int Cantidad { get; set; }
    public string? Referencia { get; set; }
    public int? IdTiendaOrigen { get; set; }
    public int? IdTiendaDestino { get; set; }
}

