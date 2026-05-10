-- =========================================
-- TIENDA DE CELULARES
-- =========================================

CREATE DATABASE tienda_celulares;
USE tienda_celulares;

-- =========================================
-- UBICACIÓN Y PERSONAS
-- =========================================

CREATE TABLE direccion (
    id_direccion INT IDENTITY(1,1) PRIMARY KEY,
    calle VARCHAR(150) NOT NULL,
    ciudad VARCHAR(100) NOT NULL,
    departamento VARCHAR(100) NOT NULL,
    pais VARCHAR(100) NOT NULL
);

CREATE TABLE persona (
    id_persona INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    apellido VARCHAR(100) NOT NULL,
    telefono VARCHAR(20),
    email VARCHAR(150),
    id_direccion INT,
    
    CONSTRAINT fk_persona_direccion
    FOREIGN KEY (id_direccion)
    REFERENCES direccion(id_direccion)
);

CREATE TABLE cliente (
    id_persona INT PRIMARY KEY,
    fecha_registro DATE NOT NULL,
    tipo_cliente VARCHAR(50),

    CONSTRAINT fk_cliente_persona
        FOREIGN KEY (id_persona)
        REFERENCES persona(id_persona)
);

CREATE TABLE empleado (
    id_persona INT PRIMARY KEY,
    cargo VARCHAR(100) NOT NULL,
    fecha_contratacion DATE,
    estado VARCHAR(50),

    CONSTRAINT fk_empleado_persona
        FOREIGN KEY (id_persona)
        REFERENCES persona(id_persona)
);

CREATE TABLE tienda (
    id_tienda INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    id_direccion INT,

    CONSTRAINT fk_tienda_direccion
        FOREIGN KEY (id_direccion)
        REFERENCES direccion(id_direccion)
);

CREATE TABLE usuario (
    id_usuario INT IDENTITY(1,1) PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    ultimo_acceso DATETIME,
    estado VARCHAR(30) DEFAULT 'activo',
    id_empleado INT NOT NULL,

    CONSTRAINT fk_usuario_empleado
        FOREIGN KEY (id_empleado)
        REFERENCES empleado(id_persona)
);

-- ========================================
-- ROLES Y PERMISOS
-- ========================================
CREATE TABLE rol (
    id_rol INT IDENTITY(1,1) PRIMARY KEY,
    nombre_rol VARCHAR(50) NOT NULL,
    descripcion VARCHAR(200)
);

CREATE TABLE permiso (
    id_permiso INT IDENTITY(1,1) PRIMARY KEY,
    nombre_permiso VARCHAR(100) NOT NULL,
    descripcion VARCHAR(200)
);

CREATE TABLE rol_permiso (
    id_rol INT,
    id_permiso INT,

    PRIMARY KEY (id_rol, id_permiso),

    CONSTRAINT fk_rolpermiso_rol
        FOREIGN KEY (id_rol)
        REFERENCES rol(id_rol),

    CONSTRAINT fk_rolpermiso_permiso
        FOREIGN KEY (id_permiso)
        REFERENCES permiso(id_permiso)
);

CREATE TABLE usuario_rol (
    id_usuario INT,
    id_rol INT,

    PRIMARY KEY (id_usuario, id_rol),

    CONSTRAINT fk_usuario_rol_usuario
        FOREIGN KEY (id_usuario)
        REFERENCES usuario(id_usuario),

    CONSTRAINT fk_usuario_rol_rol
        FOREIGN KEY (id_rol)
        REFERENCES rol(id_rol)
);
-- =========================================
-- PRODUCTOS
-- =========================================

CREATE TABLE marca (
    id_marca INT IDENTITY(1,1) PRIMARY KEY,
    nombre_marca VARCHAR(100) NOT NULL
);

CREATE TABLE categoria (
    id_categoria INT IDENTITY(1,1) PRIMARY KEY,
    nombre_categoria VARCHAR(100) NOT NULL
);

CREATE TABLE producto (
    id_producto INT IDENTITY(1,1) PRIMARY KEY,
    nombre_modelo VARCHAR(150) NOT NULL,
    descripcion TEXT,
    precio_actual DECIMAL(10,2) NOT NULL,
    tipo_producto VARCHAR(50) NOT NULL,
    id_marca INT,
    id_categoria INT,

    CONSTRAINT fk_producto_marca
        FOREIGN KEY (id_marca)
        REFERENCES marca(id_marca),

    CONSTRAINT fk_producto_categoria
        FOREIGN KEY (id_categoria)
        REFERENCES categoria(id_categoria)
);

CREATE TABLE historial_precio (
    id_historial INT IDENTITY(1,1) PRIMARY KEY,
    precio DECIMAL(10,2) NOT NULL,
    fecha_inicio DATE NOT NULL,
    fecha_fin DATE,
    id_producto INT,

    CONSTRAINT fk_historial_producto
        FOREIGN KEY (id_producto)
        REFERENCES producto(id_producto)
);

CREATE TABLE promocion (
    id_promocion INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    descripcion VARCHAR(250),
    porcentaje_descuento DECIMAL(5,2),
    fecha_inicio DATE,
    fecha_fin DATE,
    estado VARCHAR(30) DEFAULT 'activa'
);

CREATE TABLE promocion_producto (
    id_promocion INT,
    id_producto INT,

    PRIMARY KEY (id_promocion, id_producto),

    CONSTRAINT fk_promocion_producto_promocion
        FOREIGN KEY (id_promocion)
        REFERENCES promocion(id_promocion),

    CONSTRAINT fk_promocion_producto_producto
        FOREIGN KEY (id_producto)
        REFERENCES producto(id_producto)
);


-- =========================================
-- INVENTARIO
-- =========================================

CREATE TABLE equipo_fisico (
    numero_serie VARCHAR(100) PRIMARY KEY,
    estado VARCHAR(50),
    color VARCHAR(50),
    id_producto INT,
    id_tienda INT,

    CONSTRAINT fk_equipo_producto
        FOREIGN KEY (id_producto)
        REFERENCES producto(id_producto),

    CONSTRAINT fk_equipo_tienda
        FOREIGN KEY (id_tienda)
        REFERENCES tienda(id_tienda)
);

CREATE TABLE inventario (
    id_inventario INT IDENTITY(1,1) PRIMARY KEY,
    id_producto INT,
    id_tienda INT,
    cantidad INT NOT NULL DEFAULT 0,
    stock_minimo INT NOT NULL DEFAULT 0,

    CONSTRAINT uq_producto_tienda
        UNIQUE (id_producto, id_tienda),

    CONSTRAINT fk_inventario_producto
        FOREIGN KEY (id_producto)
        REFERENCES producto(id_producto),

    CONSTRAINT fk_inventario_tienda
        FOREIGN KEY (id_tienda)
        REFERENCES tienda(id_tienda)
);

CREATE TABLE movimiento_inventario (
    id_movimiento INT IDENTITY(1,1) PRIMARY KEY,
    tipo VARCHAR(50) NOT NULL,
    fecha DATETIME NOT NULL,
    referencia VARCHAR(150),
    numero_serie VARCHAR(100),
    id_producto INT,
    cantidad INT NOT NULL,

    CONSTRAINT fk_movimiento_equipo
        FOREIGN KEY (numero_serie)
        REFERENCES equipo_fisico(numero_serie),

    CONSTRAINT fk_movimiento_producto
        FOREIGN KEY (id_producto)
        REFERENCES producto(id_producto)
);

-- =========================================
-- VENTAS
-- =========================================

CREATE TABLE metodo_pago (
    id_metodo INT IDENTITY(1,1) PRIMARY KEY,
    tipo_metodo VARCHAR(50) NOT NULL
);

CREATE TABLE venta (
    id_venta INT IDENTITY(1,1) PRIMARY KEY,
    fecha_hora DATETIME NOT NULL,
    total_venta DECIMAL(10,2) NOT NULL,
    estado VARCHAR(50),
    id_cliente INT,
    id_empleado INT,
    id_metodo INT,
    id_tienda INT,

    CONSTRAINT fk_venta_cliente
        FOREIGN KEY (id_cliente)
        REFERENCES cliente(id_persona),

    CONSTRAINT fk_venta_empleado
        FOREIGN KEY (id_empleado)
        REFERENCES empleado(id_persona),

    CONSTRAINT fk_venta_metodo
        FOREIGN KEY (id_metodo)
        REFERENCES metodo_pago(id_metodo),

    CONSTRAINT fk_venta_tienda
        FOREIGN KEY (id_tienda)
        REFERENCES tienda(id_tienda)
);

CREATE TABLE detalle_venta (
    id_detalle INT IDENTITY(1,1) PRIMARY KEY,
    id_venta INT,
    id_producto INT,
    numero_serie VARCHAR(100),
    cantidad INT NOT NULL,
    precio_unitario DECIMAL(10,2) NOT NULL,

    CONSTRAINT fk_detalle_venta
        FOREIGN KEY (id_venta)
        REFERENCES venta(id_venta),

    CONSTRAINT fk_detalle_producto
        FOREIGN KEY (id_producto)
        REFERENCES producto(id_producto),

    CONSTRAINT fk_detalle_equipo
        FOREIGN KEY (numero_serie)
        REFERENCES equipo_fisico(numero_serie)
);

CREATE TABLE garantia (
    id_garantia INT IDENTITY(1,1) PRIMARY KEY,
    meses_cobertura INT,
    fecha_inicio DATE,
    fecha_fin DATE,
    condiciones TEXT,
    id_detalle INT,

    CONSTRAINT fk_garantia_detalle
        FOREIGN KEY (id_detalle)
        REFERENCES detalle_venta(id_detalle)
);

-- ========================================
-- DEVOLUCION
-- ========================================
CREATE TABLE devolucion (
    id_devolucion INT IDENTITY(1,1) PRIMARY KEY,
    fecha DATETIME NOT NULL DEFAULT GETDATE(),
    motivo VARCHAR(250),
    estado VARCHAR(50),
    id_venta INT,
    id_cliente INT,

    CONSTRAINT fk_devolucion_venta
        FOREIGN KEY (id_venta)
        REFERENCES venta(id_venta),

    CONSTRAINT fk_devolucion_cliente
        FOREIGN KEY (id_cliente)
        REFERENCES cliente(id_persona)
);

CREATE TABLE detalle_devolucion (
    id_detalle_devolucion INT IDENTITY(1,1) PRIMARY KEY,
    id_devolucion INT,
    id_producto INT,
    numero_serie VARCHAR(100),
    cantidad INT,
    motivo VARCHAR(250),

    CONSTRAINT fk_detalle_devolucion
        FOREIGN KEY (id_devolucion)
        REFERENCES devolucion(id_devolucion),

    CONSTRAINT fk_detalle_devolucion_producto
        FOREIGN KEY (id_producto)
        REFERENCES producto(id_producto),

    CONSTRAINT fk_detalle_devolucion_equipo
        FOREIGN KEY (numero_serie)
        REFERENCES equipo_fisico(numero_serie)
);
-- =========================================
-- PROVEEDORES / COMPRAS
-- =========================================

CREATE TABLE proveedor (
    id_proveedor INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(150) NOT NULL,
    telefono VARCHAR(20),
    email VARCHAR(150)
);

CREATE TABLE compra_proveedor (
    id_compra INT IDENTITY(1,1) PRIMARY KEY,
    fecha DATE NOT NULL,
    estado VARCHAR(50),
    id_proveedor INT,
    id_tienda INT,

    CONSTRAINT fk_compra_proveedor
        FOREIGN KEY (id_proveedor)
        REFERENCES proveedor(id_proveedor),

    CONSTRAINT fk_compra_tienda
        FOREIGN KEY (id_tienda)
        REFERENCES tienda(id_tienda)
);

CREATE TABLE detalle_compra (
    id_detalle_compra INT IDENTITY(1,1) PRIMARY KEY,
    id_compra INT,
    id_producto INT,
    cantidad INT NOT NULL,
    precio_compra DECIMAL(10,2) NOT NULL,

    CONSTRAINT fk_detalle_compra_compra
        FOREIGN KEY (id_compra)
        REFERENCES compra_proveedor(id_compra),

    CONSTRAINT fk_detalle_compra_producto
        FOREIGN KEY (id_producto)
        REFERENCES producto(id_producto)
);

-- =========================================
-- SERVICIO TÉCNICO
-- =========================================

CREATE TABLE orden_servicio (
    id_orden INT IDENTITY(1,1) PRIMARY KEY,
    fecha_ingreso DATETIME NOT NULL,
    estado VARCHAR(50),
    tipo_servicio VARCHAR(100),
    descripcion_problema TEXT,
    id_cliente INT,
    id_empleado INT,

    CONSTRAINT fk_orden_cliente
        FOREIGN KEY (id_cliente)
        REFERENCES cliente(id_persona),

    CONSTRAINT fk_orden_empleado
        FOREIGN KEY (id_empleado)
        REFERENCES empleado(id_persona)
);

CREATE TABLE equipo_servicio (
    id_equipo_servicio INT IDENTITY(1,1) PRIMARY KEY,
    marca VARCHAR(100),
    modelo VARCHAR(100),
    numero_serie VARCHAR(100),
    observaciones TEXT,
    id_orden INT,

    CONSTRAINT fk_equipo_servicio_orden
        FOREIGN KEY (id_orden)
        REFERENCES orden_servicio(id_orden)
);

CREATE TABLE diagnostico (
    id_diagnostico INT IDENTITY(1,1) PRIMARY KEY,
    descripcion TEXT,
    costo_estimado DECIMAL(10,2),
    fecha DATETIME,
    id_orden INT,

    CONSTRAINT fk_diagnostico_orden
        FOREIGN KEY (id_orden)
        REFERENCES orden_servicio(id_orden)
);

CREATE TABLE reparacion (
    id_reparacion INT IDENTITY(1,1) PRIMARY KEY,
    descripcion TEXT,
    costo_mano_obra DECIMAL(10,2),
    fecha_inicio DATETIME,
    fecha_fin DATETIME,
    id_orden INT,

    CONSTRAINT fk_reparacion_orden
        FOREIGN KEY (id_orden)
        REFERENCES orden_servicio(id_orden)
);

CREATE TABLE garantia_servicio (
    id_garantia INT IDENTITY(1,1) PRIMARY KEY,
    dias_garantia INT,
    fecha_inicio DATE,
    fecha_fin DATE,
    condiciones TEXT,
    id_reparacion INT,

    CONSTRAINT fk_garantia_servicio
        FOREIGN KEY (id_reparacion)
        REFERENCES reparacion(id_reparacion)
);

CREATE TABLE repuesto_usado (
    id_repuesto INT IDENTITY(1,1) PRIMARY KEY,
    id_reparacion INT,
    numero_serie VARCHAR(100),
    id_producto INT,
    cantidad INT,
    costo DECIMAL(10,2),

    CONSTRAINT fk_repuesto_reparacion
        FOREIGN KEY (id_reparacion)
        REFERENCES reparacion(id_reparacion),

    CONSTRAINT fk_repuesto_equipo
        FOREIGN KEY (numero_serie)
        REFERENCES equipo_fisico(numero_serie),

    CONSTRAINT fk_repuesto_producto
        FOREIGN KEY (id_producto)
        REFERENCES producto(id_producto)
);






-- =========================================
-- Condiciones de no negativo
-- =========================================

ALTER TABLE inventario
ADD CONSTRAINT chk_inventario_cantidad
CHECK (cantidad >= 0);

ALTER TABLE producto
ADD CONSTRAINT chk_producto_precio
CHECK (precio_actual >= 0);

ALTER TABLE detalle_venta
ADD CONSTRAINT chk_detalle_cantidad
CHECK (cantidad > 0);

ALTER TABLE detalle_venta
ADD CONSTRAINT chk_precio_unitario
CHECK (precio_unitario >= 0);

ALTER TABLE promocion
ADD CONSTRAINT chk_descuento
CHECK (
    porcentaje_descuento >= 0
    AND porcentaje_descuento <= 100
);

ALTER TABLE garantia_servicio
ADD CONSTRAINT chk_dias_garantia
CHECK (dias_garantia >= 0);
-- =========================================
-- Verifica la Garantia
-- =========================================

ALTER TABLE garantia
ADD CONSTRAINT chk_garantia_meses
CHECK (meses_cobertura >= 0);

-- =========================================
-- Catalogo
-- =========================================

ALTER TABLE venta
ADD CONSTRAINT chk_estado_venta
CHECK (estado IN ('pendiente', 'completada', 'cancelada'));

ALTER TABLE producto
ADD CONSTRAINT chk_tipo_producto
CHECK (tipo_producto IN (
    'telefono',
    'tablet',
    'accesorio',
    'repuesto'
));

ALTER TABLE equipo_fisico
ADD CONSTRAINT chk_estado_equipo
CHECK (estado IN (
    'disponible',
    'vendido',
    'reparacion',
    'defectuoso'
));

ALTER TABLE orden_servicio
ADD CONSTRAINT chk_estado_orden
CHECK (estado IN (
    'pendiente',
    'diagnostico',
    'reparando',
    'finalizado',
    'entregado'
));
-- =========================================
-- Inicializados
-- =========================================

ALTER TABLE venta
ADD CONSTRAINT df_fecha_venta
DEFAULT GETDATE() FOR fecha_hora;

ALTER TABLE venta
ADD CONSTRAINT df_estado_venta
DEFAULT 'pendiente' FOR estado;

ALTER TABLE persona
ADD CONSTRAINT uq_persona_email
UNIQUE(email);

-- =========================================
-- Disminuir inventario
-- =========================================

GO
CREATE TRIGGER trg_disminuir_inventario
ON detalle_venta
AFTER INSERT
AS
BEGIN

    UPDATE inv
    SET inv.cantidad = inv.cantidad - i.cantidad
    FROM inventario inv
    INNER JOIN inserted i
        ON inv.id_producto = i.id_producto
    INNER JOIN venta v
        ON i.id_venta = v.id_venta
       AND inv.id_tienda = v.id_tienda;

	   
	UPDATE ef
	SET ef.estado = 'vendido'
	FROM equipo_fisico ef
	INNER JOIN inserted i ON ef.numero_serie = i.numero_serie
	WHERE i.numero_serie IS NOT NULL;

END;
GO


-- =========================================
-- Aumento Inventario
-- =========================================
GO
ALTER TRIGGER trg_aumentar_inventario
ON detalle_compra
AFTER INSERT
AS
BEGIN
    UPDATE inv
    SET inv.cantidad = inv.cantidad + i.cantidad
    FROM inventario inv
    INNER JOIN inserted i ON inv.id_producto = i.id_producto
    INNER JOIN compra_proveedor c ON i.id_compra = c.id_compra
    WHERE inv.id_tienda = c.id_tienda;
END;
GO

-- =========================================
-- Historial de Precio
-- =========================================

GO

ALTER TRIGGER trg_historial_precio
ON producto
AFTER UPDATE
AS
BEGIN
    IF UPDATE(precio_actual)
    BEGIN
        -- Cerrar el periodo del precio anterior
        UPDATE historial_precio
        SET fecha_fin = GETDATE()
        FROM historial_precio hp
        INNER JOIN inserted i ON hp.id_producto = i.id_producto
        WHERE hp.fecha_fin IS NULL;

        -- Insertar el nuevo precio
        INSERT INTO historial_precio (precio, fecha_inicio, id_producto)
        SELECT i.precio_actual, GETDATE(), i.id_producto
        FROM inserted i
        INNER JOIN deleted d ON i.id_producto = d.id_producto
        WHERE i.precio_actual <> d.precio_actual;
    END
END;
GO

-- =========================================
-- Stock Validar
-- =========================================

GO

ALTER TRIGGER trg_validar_stock
ON detalle_venta
AFTER INSERT
AS
BEGIN
    IF EXISTS (
        SELECT 1
        FROM inventario inv
        INNER JOIN inserted i ON inv.id_producto = i.id_producto
        INNER JOIN venta v ON i.id_venta = v.id_venta
        WHERE inv.id_tienda = v.id_tienda
          AND inv.cantidad < i.cantidad
    )
    BEGIN
        RAISERROR('Stock insuficiente.',16,1);
        ROLLBACK TRANSACTION;
    END
END;


    -- Si hay stock suficiente, insertar normalmente
 --   INSERT INTO detalle_venta (id_venta, id_producto, numero_serie, cantidad, precio_unitario)
--    SELECT id_venta, id_producto, numero_serie, cantidad, precio_unitario
 --   FROM inserted;
--END;



-- ====================================================
-- DEVOLUCION AUMENTO DEL INVENTARIO Y CAMBIO DE ESTADO
-- =====================================================

GO
CREATE TRIGGER trg_devolucion_inventario
ON detalle_devolucion
AFTER INSERT
AS
BEGIN
    UPDATE inv
    SET inv.cantidad = inv.cantidad + i.cantidad
    FROM inventario inv
    INNER JOIN inserted i
        ON inv.id_producto = i.id_producto
    INNER JOIN devolucion d
        ON i.id_devolucion = d.id_devolucion
    INNER JOIN venta v
        ON d.id_venta = v.id_venta
       AND inv.id_tienda = v.id_tienda;
END;
GO


GO

CREATE TRIGGER trg_devolucion_equipo
ON detalle_devolucion
AFTER INSERT
AS
BEGIN

    UPDATE equipo_fisico
    SET estado = 'disponible'
    FROM equipo_fisico ef
    INNER JOIN inserted i
        ON ef.numero_serie = i.numero_serie;

END;
GO

--==============================
-- PROCEDIMIENTO ALMACENADOS
--==============================



---------------------------------
---OBTENER PRODUCTO POR CATEGORIA
---------------------------------

CREATE OR ALTER PROCEDURE InsertarDatosIniciales
AS
BEGIN
    SET NOCOUNT ON;

    -- Insertar Marcas conocidas sin duplicar
    IF NOT EXISTS (SELECT 1 FROM marca WHERE nombre_marca = 'Samsung')
        INSERT INTO marca (nombre_marca) VALUES ('Samsung');
    IF NOT EXISTS (SELECT 1 FROM marca WHERE nombre_marca = 'Apple')
        INSERT INTO marca (nombre_marca) VALUES ('Apple');
    IF NOT EXISTS (SELECT 1 FROM marca WHERE nombre_marca = 'Xiaomi')
        INSERT INTO marca (nombre_marca) VALUES ('Xiaomi');
    IF NOT EXISTS (SELECT 1 FROM marca WHERE nombre_marca = 'Motorola')
        INSERT INTO marca (nombre_marca) VALUES ('Motorola');
    IF NOT EXISTS (SELECT 1 FROM marca WHERE nombre_marca = 'Huawei')
        INSERT INTO marca (nombre_marca) VALUES ('Huawei');
    IF NOT EXISTS (SELECT 1 FROM marca WHERE nombre_marca = 'Sony')
        INSERT INTO marca (nombre_marca) VALUES ('Sony');

    -- Insertar Categorías sin duplicar

     -- Teléfonos
    IF NOT EXISTS (SELECT 1 FROM categoria WHERE nombre_categoria = 'Gama alta')
        INSERT INTO categoria (nombre_categoria) VALUES ('Gama alta');
    IF NOT EXISTS (SELECT 1 FROM categoria WHERE nombre_categoria = 'Gama media')
        INSERT INTO categoria (nombre_categoria) VALUES ('Gama media');
    IF NOT EXISTS (SELECT 1 FROM categoria WHERE nombre_categoria = 'Gama baja')
        INSERT INTO categoria (nombre_categoria) VALUES ('Gama baja');
    IF NOT EXISTS (SELECT 1 FROM categoria WHERE nombre_categoria = 'Smartphones reacondicionados')
        INSERT INTO categoria (nombre_categoria) VALUES ('Smartphones reacondicionados');

    -- Tablets
    IF NOT EXISTS (SELECT 1 FROM categoria WHERE nombre_categoria = 'Android')
        INSERT INTO categoria (nombre_categoria) VALUES ('Android');
    IF NOT EXISTS (SELECT 1 FROM categoria WHERE nombre_categoria = 'iPad')
        INSERT INTO categoria (nombre_categoria) VALUES ('iPad');
    IF NOT EXISTS (SELECT 1 FROM categoria WHERE nombre_categoria = 'Windows Tablet')
        INSERT INTO categoria (nombre_categoria) VALUES ('Windows Tablet');
    IF NOT EXISTS (SELECT 1 FROM categoria WHERE nombre_categoria = 'Infantil')
        INSERT INTO categoria (nombre_categoria) VALUES ('Infantil');

    -- Repuestos
    IF NOT EXISTS (SELECT 1 FROM categoria WHERE nombre_categoria = 'Pantallas')
        INSERT INTO categoria (nombre_categoria) VALUES ('Pantallas');
    IF NOT EXISTS (SELECT 1 FROM categoria WHERE nombre_categoria = 'Baterías')
        INSERT INTO categoria (nombre_categoria) VALUES ('Baterías');
    IF NOT EXISTS (SELECT 1 FROM categoria WHERE nombre_categoria = 'Placas madre')
        INSERT INTO categoria (nombre_categoria) VALUES ('Placas madre');
    IF NOT EXISTS (SELECT 1 FROM categoria WHERE nombre_categoria = 'Cámaras')
        INSERT INTO categoria (nombre_categoria) VALUES ('Cámaras');

    -- Accesorios
    IF NOT EXISTS (SELECT 1 FROM categoria WHERE nombre_categoria = 'Fundas')
        INSERT INTO categoria (nombre_categoria) VALUES ('Fundas');
    IF NOT EXISTS (SELECT 1 FROM categoria WHERE nombre_categoria = 'Protectores de pantalla')
        INSERT INTO categoria (nombre_categoria) VALUES ('Protectores de pantalla');
    IF NOT EXISTS (SELECT 1 FROM categoria WHERE nombre_categoria = 'Cargadores')
        INSERT INTO categoria (nombre_categoria) VALUES ('Cargadores');
    IF NOT EXISTS (SELECT 1 FROM categoria WHERE nombre_categoria = 'Audífonos')
        INSERT INTO categoria (nombre_categoria) VALUES ('Audífonos');
END;


EXEC InsertarDatosIniciales;




--ALTER DATABASE [tienda_celulares] SET RECURSIVE_TRIGGERS OFF;
SELECT * FROM marca;
SELECT * FROM categoria;
SELECT * FROM producto;

DELETE FROM marca WHERE id_marca > 6;  
DELETE FROM categoria WHERE id_categoria > 16; 
