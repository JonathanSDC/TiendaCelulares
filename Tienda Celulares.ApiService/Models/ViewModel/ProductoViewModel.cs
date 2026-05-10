namespace Tienda_Celulares.ApiService.Models.ViewModel
{
    public class ProductoViewModel
    {
        public int IdProducto { get; set; }
        public string NombreModelo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal PrecioActual { get; set; }
        public string TipoProducto { get; set; } = string.Empty;
        public int? IdMarca { get; set; }
        public string MarcaNombre { get; set; } = string.Empty;
        public int? IdCategoria { get; set; }
        public string CategoriaNombre { get; set; } = string.Empty;
    }
}
