using Microsoft.EntityFrameworkCore;
using Tienda_Celulares.ApiService.Models;
using Tienda_Celulares.ApiService.Models.ViewModel;

using static Microsoft.IO.RecyclableMemoryStreamManager;

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
            // Producto
            modelBuilder.Entity<Producto>(entity =>
            {
                entity.ToTable("producto");
                entity.HasKey(p => p.id_producto);

                entity.Property(p => p.id_producto).HasColumnName("id_producto");
                entity.Property(p => p.nombre_modelo).HasColumnName("nombre_modelo");
                entity.Property(p => p.descripcion).HasColumnName("descripcion");
                entity.Property(p => p.precio_actual).HasColumnName("precio_actual");
                entity.Property(p => p.tipo_producto).HasColumnName("tipo_producto");
                entity.Property(p => p.id_marca).HasColumnName("id_marca");
                entity.Property(p => p.id_categoria).HasColumnName("id_categoria");

                entity.HasOne(p => p.Marca)
                      .WithMany()
                      .HasForeignKey(p => p.id_marca)
                      .HasConstraintName("fk_producto_marca");

                entity.HasOne(p => p.Categoria)
                      .WithMany()
                      .HasForeignKey(p => p.id_categoria)
                      .HasConstraintName("fk_producto_categoria");

                // Si existe trigger en la tabla
                entity.HasAnnotation("Relational:HasTrigger", "trg_historial_precio");
            });

            // Marca
            modelBuilder.Entity<Marca>(entity =>
            {
                entity.ToTable("marca");
                entity.HasKey(m => m.IdMarca);
                entity.Property(m => m.IdMarca).HasColumnName("id_marca");
                entity.Property(m => m.Nombre).HasColumnName("nombre_marca");
            });

            // Categoria
            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.ToTable("categoria");
                entity.HasKey(c => c.IdCategoria);
                entity.Property(c => c.IdCategoria).HasColumnName("id_categoria");
                entity.Property(c => c.Nombre).HasColumnName("nombre_categoria");
            });

            // Venta
            modelBuilder.Entity<Venta>(entity =>
            {
                entity.ToTable("venta");
                entity.HasKey(v => v.id_venta);
                entity.Property(v => v.id_venta).HasColumnName("id_venta");
                entity.Property(v => v.fecha_hora).HasColumnName("fecha_hora");
                entity.Property(v => v.total_venta).HasColumnName("total_venta");
                entity.Property(v => v.estado).HasColumnName("estado");
                entity.Property(v => v.id_cliente).HasColumnName("id_cliente");
                entity.Property(v => v.id_empleado).HasColumnName("id_empleado");
                entity.Property(v => v.id_metodo).HasColumnName("id_metodo");
                entity.Property(v => v.id_tienda).HasColumnName("id_tienda");
            });

            // DetalleVenta (PK simple id_detalle)
            modelBuilder.Entity<DetalleVenta>(entity =>
            {
                entity.ToTable("detalle_venta");
                entity.HasKey(d => d.id_detalle);

                entity.Property(d => d.id_detalle).HasColumnName("id_detalle");
                entity.Property(d => d.id_venta).HasColumnName("id_venta");
                entity.Property(d => d.id_producto).HasColumnName("id_producto");
                entity.Property(d => d.numero_serie).HasColumnName("numero_serie");
                entity.Property(d => d.cantidad).HasColumnName("cantidad");
                entity.Property(d => d.precio_unitario).HasColumnName("precio_unitario");

                entity.HasOne(d => d.Venta)
                      .WithMany(v => v.DetalleVentas)
                      .HasForeignKey(d => d.id_venta)
                      .HasConstraintName("fk_detalle_venta");

                entity.HasOne(d => d.Producto)
                      .WithMany(p => p.DetalleVentas)
                      .HasForeignKey(d => d.id_producto)
                      .HasConstraintName("fk_detalle_producto");
            });

            // Persona, Cliente, Empleado, Usuario (una sola definición cada uno)
            modelBuilder.Entity<Persona>().ToTable("persona").HasKey(p => p.IdPersona);

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.ToTable("cliente");
                entity.HasKey(c => c.IdPersona);
                entity.HasOne(c => c.Persona).WithOne(p => p.Cliente).HasForeignKey<Cliente>(c => c.IdPersona);
            });

            modelBuilder.Entity<Empleado>(entity =>
            {
                entity.ToTable("empleado");
                entity.HasKey(e => e.IdPersona);
                entity.HasOne(e => e.Persona).WithOne(p => p.Empleado).HasForeignKey<Empleado>(e => e.IdPersona);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("usuario");
                entity.HasKey(u => u.IdUsuario);
                entity.HasOne(u => u.Empleado).WithOne(e => e.Usuario).HasForeignKey<Usuario>(u => u.IdEmpleado);
            });

            base.OnModelCreating(modelBuilder);
        }

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

        // =========================
        // VENTAS
        // =========================

        public DbSet<Venta> Ventas { get; set; }

        public DbSet<DetalleVenta> DetalleVentas { get; set; }

        // =========================
        // PRODUCTOS
        // =========================

       

        // =========================
        // OTROS
        // =========================

        //public DbSet<Tienda> Tiendas { get; set; }

        public DbSet<MetodoPago> MetodoPagos { get; set; }
    }


}
