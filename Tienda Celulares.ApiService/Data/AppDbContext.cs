using Microsoft.EntityFrameworkCore;
using CRUD.Shared.Models;
using static Microsoft.IO.RecyclableMemoryStreamManager;

namespace Tienda_Celulares.ApiService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // =========================
        // PRODUCTOS
        // =========================
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        // =========================
        // PERSONAS
        // =========================
        public DbSet<Persona> Personas { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        // =========================
        // VENTAS
        // =========================
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetalleVentas { get; set; }

        // =========================
        // TIENDAS
        // =========================
        public DbSet<Tienda> Tiendas { get; set; }

        // =========================
        // DIRECCIONES
        // =========================
        public DbSet<Direccion> Direcciones { get; set; }

        // =========================
        // OTROS
        // =========================
        public DbSet<MetodoPago> MetodosPago { get; set; }
        // public DbSet<Tienda> Tiendas { get; set; } // pendiente si existe

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // =========================
            // PRODUCTO
            // =========================
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

                entity.HasAnnotation("Relational:HasTrigger", "trg_historial_precio");
            });

            // =========================
            // MARCA
            // =========================
            modelBuilder.Entity<Marca>(entity =>
            {
                entity.ToTable("marca");
                entity.HasKey(m => m.IdMarca);
                entity.Property(m => m.IdMarca).HasColumnName("id_marca");
                entity.Property(m => m.Nombre).HasColumnName("nombre_marca");
            });

            // =========================
            // CATEGORIA
            // =========================
            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.ToTable("categoria");
                entity.HasKey(c => c.IdCategoria);
                entity.Property(c => c.IdCategoria).HasColumnName("id_categoria");
                entity.Property(c => c.Nombre).HasColumnName("nombre_categoria");
            });

            // =========================
            // VENTA
            // =========================
            modelBuilder.Entity<Venta>(entity =>
            {
                entity.ToTable("venta");
                entity.HasKey(v => v.id_venta);

                entity.Property(v => v.fecha_hora).HasColumnName("fecha_hora");
                entity.Property(v => v.total_venta).HasColumnName("total_venta");
                entity.Property(v => v.estado).HasColumnName("estado");

                entity.HasOne(v => v.Cliente)
                      .WithMany()
                      .HasForeignKey(v => v.id_cliente)
                      .HasPrincipalKey(c => c.IdPersona);

                entity.HasOne(v => v.Empleado)
                      .WithMany()
                      .HasForeignKey(v => v.id_empleado)
                      .HasPrincipalKey(e => e.IdPersona);

                entity.HasOne(v => v.MetodoPago)
                      .WithMany()
                      .HasForeignKey(v => v.id_metodo);

                entity.HasMany(v => v.DetalleVentas)
                      .WithOne(d => d.Venta)
                      .HasForeignKey(d => d.id_venta);
            });

            // =========================
            // DETALLE VENTA
            // =========================
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

            // =========================
            // PERSONA / CLIENTE / EMPLEADO / USUARIO
            // =========================
            modelBuilder.Entity<Persona>().ToTable("persona").HasKey(p => p.IdPersona);

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.ToTable("cliente");
                entity.HasKey(c => c.IdPersona);
                entity.HasOne(c => c.Persona)
                      .WithOne(p => p.Cliente)
                      .HasForeignKey<Cliente>(c => c.IdPersona);
            });

            modelBuilder.Entity<Empleado>(entity =>
            {
                entity.ToTable("empleado");
                entity.HasKey(e => e.IdPersona);
                entity.HasOne(e => e.Persona)
                      .WithOne(p => p.Empleado)
                      .HasForeignKey<Empleado>(e => e.IdPersona);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("usuario");
                entity.HasKey(u => u.IdUsuario);
                entity.HasOne(u => u.Empleado)
                      .WithOne(e => e.Usuario)
                      .HasForeignKey<Usuario>(u => u.IdEmpleado);
            });

            // =========================
            // METODO PAGO
            // =========================
            modelBuilder.Entity<MetodoPago>(entity =>
            {
                entity.ToTable("metodo_pago");
                entity.HasKey(m => m.IdMetodo);
                entity.Property(m => m.IdMetodo).HasColumnName("id_metodo");
                entity.Property(m => m.TipoMetodo).HasColumnName("tipo_metodo");
            });

            // =========================
            // TIENDA
            // =========================
            modelBuilder.Entity<Tienda>(entity =>
            {
                entity.ToTable("tienda");
                entity.HasKey(t => t.IdTienda);

                entity.Property(t => t.IdTienda).HasColumnName("id_tienda");
                entity.Property(t => t.Nombre).HasColumnName("nombre");

                entity.Property(t => t.IdDireccion).HasColumnName("id_direccion");

                entity.HasOne(t => t.Direccion)
                      .WithMany(d => d.Tiendas)
                      .HasForeignKey(t => t.IdDireccion)
                      .HasConstraintName("fk_tienda_direccion");
            });


            base.OnModelCreating(modelBuilder);
        }
    }
}
