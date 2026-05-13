using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD.Shared.Models
{
    [Table("persona")]
    public class Persona
    {
        [Key]
        [Column("id_persona")]
        public int IdPersona { get; set; }

        [Required]
        [Column("nombre")]
        [StringLength(100)]
        public string Nombre { get; set; } = null!;

        [Required]
        [Column("apellido")]
        [StringLength(100)]
        public string Apellido { get; set; } = null!;

        [Column("telefono")]
        [StringLength(20)]
        public string? Telefono { get; set; }

        [Column("email")]
        [StringLength(150)]
        public string? Email { get; set; }

        [Column("id_direccion")]
        public int? IdDireccion { get; set; }

        // Navegaciones opcionales
        //public Direccion? Direccion { get; set; }
        public Cliente? Cliente { get; set; }
        public Empleado? Empleado { get; set; }
    }
}
