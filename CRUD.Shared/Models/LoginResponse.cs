using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.Shared.Models
{
    public class LoginResponse
    {
        public int IdUsuario { get; set; }
        public string Username { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new();
    }
}
