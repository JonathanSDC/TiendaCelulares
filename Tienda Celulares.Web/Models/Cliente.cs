namespace Tienda_Celulares.Web.Models
{
    public class Cliente
    {
        public int Id_Persona { get; set; }

        public DateTime Fecha_Registro { get; set; }

        public string? Tipo_Cliente { get; set; }

        public Persona? Persona { get; set; }
    }
}