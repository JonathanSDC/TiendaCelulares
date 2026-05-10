using Microsoft.EntityFrameworkCore;
using Tienda_Celulares.ApiService.Models;

namespace Tienda_Celulares.ApiService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Persona> Personas { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Producto>(entity =>
            {

                entity.ToTable("producto");
                entity.HasKey(p => p.id_producto);

                // Configurar relación con Marca
                entity.HasOne(p => p.Marca)
                      .WithMany()
                      .HasForeignKey(p => p.id_marca); // <--- Indica la columna exacta de tu DB

                // Configurar relación con Categoria
                entity.HasOne(p => p.Categoria)
                      .WithMany()
                      .HasForeignKey(p => p.id_categoria); // <--- Indica la columna exacta de tu DB

                // Esto desactiva el uso de OUTPUT en esta tabla
                entity.ToTable(tb => tb.HasTrigger("trg_historial_precio"));
            });

            // Configuración para Categoria
            modelBuilder.Entity<Categoria>()
                .HasKey(c => c.id_categoria);

            // Configuración para Marca
            modelBuilder.Entity<Marca>()
                .HasKey(m => m.id_marca);

            // Lo que ya tenías para el Trigger de Producto
            modelBuilder.Entity<Producto>(entity =>
            {
                entity.ToTable(tb => tb.HasTrigger("trg_historial_precio"));
            });

            // Mapear a nombre de tabla en singular como está en la DB
            modelBuilder.Entity<Marca>(entity =>
            {
                entity.ToTable("marca"); 
                entity.HasKey(m => m.id_marca);
            });

            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.ToTable("categoria"); // <--- Nombre exacto de tu DB
                entity.HasKey(c => c.id_categoria); // <--- Esto quita el error de la imagen 3ef538
            });

            // Mapear a nombre de tabla en singular como está en la DB

            modelBuilder.Entity<Marca>().ToTable("marca").HasKey(m => m.id_marca);
            modelBuilder.Entity<Categoria>().ToTable("categoria").HasKey(c => c.id_categoria);

            // Mapear a nombre de tabla en singular como está en la DB
            modelBuilder.Entity<Persona>().ToTable("persona");
            modelBuilder.Entity<Cliente>().ToTable("cliente");


            base.OnModelCreating(modelBuilder);


            // Persona
            modelBuilder.Entity<Persona>()
                .ToTable("persona")
                .HasKey(p => p.IdPersona);

            // Cliente: PK = FK a Persona (1:1)
            modelBuilder.Entity<Cliente>()
                .ToTable("cliente")
                .HasKey(c => c.IdPersona);

            modelBuilder.Entity<Cliente>()
                .HasOne(c => c.Persona)
                .WithOne(p => p.Cliente)
                .HasForeignKey<Cliente>(c => c.IdPersona)
                .HasConstraintName("fk_cliente_persona")
                .OnDelete(DeleteBehavior.Cascade);

            // Empleado: PK = FK a Persona (1:1)
            modelBuilder.Entity<Empleado>()
                .ToTable("empleado")
                .HasKey(e => e.IdPersona);

            modelBuilder.Entity<Empleado>()
                .HasOne(e => e.Persona)
                .WithOne(p => p.Empleado)
                .HasForeignKey<Empleado>(e => e.IdPersona)
                .HasConstraintName("fk_empleado_persona")
                .OnDelete(DeleteBehavior.Cascade);

            // Usuario: FK id_empleado -> empleado.id_persona
            modelBuilder.Entity<Usuario>()
                .ToTable("usuario")
                .HasKey(u => u.IdUsuario);

            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Empleado)
                .WithOne(e => e.Usuario)
                .HasForeignKey<Usuario>(u => u.IdEmpleado)
                .HasConstraintName("fk_usuario_empleado")
                .OnDelete(DeleteBehavior.Restrict);

            /* ... 
           // Direccion mapping (si existe)
           modelBuilder.Entity<Direccion>()
               .ToTable("direccion")
               .HasKey(d => d.IdDireccion);

           modelBuilder.Entity<Persona>()
               .HasOne(p => p.Direccion)
               .WithMany() // ajusta si direccion tiene colección
               .HasForeignKey(p => p.IdDireccion)
               .HasConstraintName("fk_persona_direccion")
               .OnDelete(DeleteBehavior.SetNull); 

           */
        }


    }
}