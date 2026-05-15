using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.Shared.Models
{
    [Table("usuario_rol")]
    public class UsuarioRol
    {
        [Key]

        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [Column("id_rol")]
        public int IdRol { get; set; }
        //public Rol Rol { get; set; } = null!;
        
        public Usuario Usuario { get; set; } = default!;
        public Rol Rol { get; set; } = default!;
    }
}
