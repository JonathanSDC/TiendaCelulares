using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tienda_Celulares.ApiService.Models
{
    [Table("empleado")]
    public class Empleado
    {
        // PK que también es FK a persona.id_persona
        [Key]
        [Column("id_persona")]
        public int IdPersona { get; set; }

        [Required]
        [Column("cargo")]
        [StringLength(100)]
        public string Cargo { get; set; } = null!;

        [Column("fecha_contratacion")]
        public DateTime? FechaContratacion { get; set; }

        [Column("estado")]
        [StringLength(50)]
        public string? Estado { get; set; }

        // Navegación a Persona (1:1)
        [ForeignKey(nameof(IdPersona))]
        public Persona? Persona { get; set; }

        // Navegación inversa a Usuario (si existe)
        public Usuario? Usuario { get; set; }
    }
}
