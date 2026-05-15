using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.Shared.Models
{
    [Table("rol")]
    public class Rol
    {
        [Key]
        [Column("id_rol")]
        public int IdRol { get; set; }
        
        [Column("nombre_rol")]
        public string NombreRol { get; set; } = string.Empty;
        [Column("descripcion")]
        public string? Descripcion { get; set; }

        public ICollection<UsuarioRol> UsuarioRoles { get; set; } = new List<UsuarioRol>();
    }
}
