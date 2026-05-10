namespace Tienda_de_Celulares.Web.Models
{
    public class Persona
    {
        public int Id_Persona { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Apellido { get; set; } = string.Empty;

        public string? Telefono { get; set; }

        public string? Email { get; set; }
    }
}