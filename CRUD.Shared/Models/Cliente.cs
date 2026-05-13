using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD.Shared.Models
{
    [Table("cliente")]
    public class Cliente
    {
        // PK que también es FK a persona.id_persona
        [Key]
        [Column("id_persona")]
        public int IdPersona { get; set; }

        [Required]
        [Column("fecha_registro")]
        public DateTime FechaRegistro { get; set; }

        [Column("tipo_cliente")]
        [StringLength(50)]
        public string? TipoCliente { get; set; }

        // Navegación a Persona (1:1)
        [ForeignKey(nameof(IdPersona))]
        public Persona? Persona { get; set; }
    }
}
