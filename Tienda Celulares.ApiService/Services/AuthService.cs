using CRUD.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Tienda_Celulares.ApiService.Data;



namespace Tienda_Celulares.ApiService.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        /*
        public async Task<Usuario?> LoginAsync(string username, string password)
        {
            var user = await _context.Usuarios
                .Include(u => u.UsuarioRoles)
                .ThenInclude(ur => ur.Rol)
                .FirstOrDefaultAsync(u => u.Username == username && u.Estado == "activo");

            if (user == null) return null;

            // Validar hash
            var hash = ComputeSha256Hash(password);
            //if (user.PasswordHash != hash) return null;
            if (user.PasswordHash.Trim().ToLower() != hash.Trim().ToLower()) return null;

            user.UltimoAcceso = DateTime.Now;
            await _context.SaveChangesAsync();

            return user;
        } */

        public async Task<Usuario?> LoginAsync(string username, string password)
        {
            try
            {
                var user = await _context.Usuarios
                    .Include(u => u.UsuarioRoles)
                    .ThenInclude(ur => ur.Rol)
                    .FirstOrDefaultAsync(u => u.Username == username && u.Estado == "activo");

                if (user == null) return null;

                var hash = ComputeSha256Hash(password);
                if (user.PasswordHash.Trim().ToLower() != hash.Trim().ToLower()) return null;

                user.UltimoAcceso = DateTime.Now;
                await _context.SaveChangesAsync();

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("Error en LoginAsync: " + ex.Message, ex);
            }
        }


        private string ComputeSha256Hash(string rawData)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }
}
