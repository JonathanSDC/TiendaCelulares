using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD.Shared.Models
{
    [Table("usuario")]
    public class Usuario
    {
        [Key]
        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [Required]
        [Column("username")]
        [StringLength(50)]
        public string Username { get; set; } = null!;

        [Required]
        [Column("password_hash")]
        [StringLength(255)]
        public string PasswordHash { get; set; } = null!;

        [Column("ultimo_acceso")]
        public DateTime? UltimoAcceso { get; set; }

        [Column("estado")]
        [StringLength(30)]
        public string? Estado { get; set; }

        // FK a empleado.id_persona (no es PK de usuario)
        [Column("id_empleado")]
        public int IdEmpleado { get; set; }

        [ForeignKey(nameof(IdEmpleado))]
        public Empleado? Empleado { get; set; }

        public ICollection<UsuarioRol> UsuarioRoles { get; set; } = new List<UsuarioRol>();
    }
}
