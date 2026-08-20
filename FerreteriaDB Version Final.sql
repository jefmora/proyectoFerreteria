USE [master]
GO

IF DB_ID('DB_FERRETERIA') IS NULL
BEGIN
    CREATE DATABASE [DB_FERRETERIA];
END
GO

ALTER DATABASE [DB_FERRETERIA] SET COMPATIBILITY_LEVEL = 150
GO

USE [DB_FERRETERIA]
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
BEGIN
    EXEC [DB_FERRETERIA].[dbo].[sp_fulltext_database] @action = 'enable'
END
GO
ALTER DATABASE [DB_FERRETERIA] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [DB_FERRETERIA] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [DB_FERRETERIA] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [DB_FERRETERIA] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [DB_FERRETERIA] SET ARITHABORT OFF 
GO
ALTER DATABASE [DB_FERRETERIA] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [DB_FERRETERIA] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [DB_FERRETERIA] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [DB_FERRETERIA] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [DB_FERRETERIA] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [DB_FERRETERIA] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [DB_FERRETERIA] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [DB_FERRETERIA] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [DB_FERRETERIA] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [DB_FERRETERIA] SET  ENABLE_BROKER 
GO
ALTER DATABASE [DB_FERRETERIA] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [DB_FERRETERIA] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [DB_FERRETERIA] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [DB_FERRETERIA] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [DB_FERRETERIA] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [DB_FERRETERIA] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [DB_FERRETERIA] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [DB_FERRETERIA] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [DB_FERRETERIA] SET  MULTI_USER 
GO
ALTER DATABASE [DB_FERRETERIA] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [DB_FERRETERIA] SET DB_CHAINING OFF 
GO
ALTER DATABASE [DB_FERRETERIA] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [DB_FERRETERIA] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [DB_FERRETERIA] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [DB_FERRETERIA] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [DB_FERRETERIA] SET QUERY_STORE = OFF
GO
USE [DB_FERRETERIA]
GO
/****** Objeto: Table [dbo].[AuditoriaProducto] Fecha de script: 21/7/2026 17:45:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuditoriaProducto](
	[IdAuditoria] [int] IDENTITY(1,1) NOT NULL,
	[FechaCambio] [datetime] NOT NULL,
	[CampoModificado] [varchar](100) NOT NULL,
	[ValorAnterior] [varchar](250) NULL,
	[ValorNuevo] [varchar](250) NULL,
	[IdProducto] [int] NOT NULL,
	[IdUsuario] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdAuditoria] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Categoria] Fecha de script: 21/7/2026 17:45:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categoria](
	[IdCategoria] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[Descripcion] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdCategoria] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Cliente] Fecha de script: 21/7/2026 17:45:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cliente](
	[IdCliente] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[Telefono] [varchar](20) NULL,
	[Correo] [varchar](100) NULL,
	[Direccion] [varchar](250) NULL,
	[IdUsuario] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdCliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Compra] Fecha de script: 21/7/2026 17:45:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Compra](
	[IdCompra] [int] IDENTITY(1,1) NOT NULL,
	[FechaCompra] [datetime] NOT NULL,
	[Estado] [varchar](30) NOT NULL,
	[FechaRecepcion] [datetime] NULL,
	[IdProveedor] [int] NOT NULL,
	[IdUsuario] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdCompra] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[DetalleCompra] Fecha de script: 21/7/2026 17:45:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DetalleCompra](
	[IdDetalleCompra] [int] IDENTITY(1,1) NOT NULL,
	[Cantidad] [int] NOT NULL,
	[CostoUnitario] [decimal](10, 2) NOT NULL,
	[Subtotal] [decimal](12, 2) NOT NULL,
	[IdCompra] [int] NOT NULL,
	[IdProducto] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdDetalleCompra] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[DetalleVenta] Fecha de script: 21/7/2026 17:45:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DetalleVenta](
	[IdDetalleVenta] [int] IDENTITY(1,1) NOT NULL,
	[Cantidad] [int] NOT NULL,
	[PrecioUnitario] [decimal](10, 2) NOT NULL,
	[Subtotal] [decimal](12, 2) NOT NULL,
	[IdVenta] [int] NOT NULL,
	[IdProducto] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdDetalleVenta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[ErrorSistema] Fecha de script: 21/7/2026 17:45:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ErrorSistema](
	[IdError] [int] IDENTITY(1,1) NOT NULL,
	[FechaError] [datetime] NOT NULL,
	[Ruta] [varchar](250) NULL,
	[MensajeError] [varchar](max) NULL,
	[StackTrace] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdError] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[MovimientoInventario] Fecha de script: 21/7/2026 17:45:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MovimientoInventario](
	[IdMovimiento] [int] IDENTITY(1,1) NOT NULL,
	[TipoMovimiento] [varchar](20) NOT NULL,
	[Cantidad] [int] NOT NULL,
	[FechaMovimiento] [datetime] NOT NULL,
	[IdProducto] [int] NOT NULL,
	[IdUsuario] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdMovimiento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Producto] Fecha de script: 21/7/2026 17:45:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Producto](
	[IdProducto] [int] IDENTITY(1,1) NOT NULL,
	[SKU] [varchar](50) NOT NULL,
	[Nombre] [varchar](150) NOT NULL,
	[Precio] [decimal](10, 2) NOT NULL,
	[StockActual] [int] NOT NULL,
	[StockMinimo] [int] NOT NULL,
	[Estado] [bit] NOT NULL,
	[IdCategoria] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdProducto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[ProductoProveedor] Fecha de script: 21/7/2026 17:45:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductoProveedor](
	[IdProductoProveedor] [int] IDENTITY(1,1) NOT NULL,
	[IdProducto] [int] NOT NULL,
	[IdProveedor] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdProductoProveedor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Proveedor] Fecha de script: 21/7/2026 17:45:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Proveedor](
	[IdProveedor] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[Telefono] [varchar](20) NULL,
	[Correo] [varchar](100) NULL,
	[Direccion] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdProveedor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Rol] Fecha de script: 21/7/2026 17:45:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rol](
	[IdRol] [int] IDENTITY(1,1) NOT NULL,
	[NombreRol] [varchar](50) NOT NULL,
	[Descripcion] [varchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdRol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Usuario] Fecha de script: 21/7/2026 17:45:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuario](
	[IdUsuario] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[Correo] [varchar](100) NOT NULL,
	[PasswordHash] [varchar](255) NOT NULL,
	[Estado] [bit] NOT NULL,
	[FechaRegistro] [datetime] NOT NULL,
	[IdRol] [int] NOT NULL,
	[Identificacion] [varchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Venta] Fecha de script: 21/7/2026 17:45:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Venta](
	[IdVenta] [int] IDENTITY(1,1) NOT NULL,
	[FechaVenta] [datetime] NOT NULL,
	[Total] [decimal](12, 2) NOT NULL,
	[Estado] [varchar](20) NOT NULL,
	[MotivoAnulacion] [varchar](250) NULL,
	[IdCliente] [int] NOT NULL,
	[IdUsuario] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdVenta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Categoria] ON 

INSERT [dbo].[Categoria] ([IdCategoria], [Nombre], [Descripcion]) VALUES (1, N'Herramientas', N'Herramientas manuales y eléctricas')
SET IDENTITY_INSERT [dbo].[Categoria] OFF
GO
SET IDENTITY_INSERT [dbo].[Cliente] ON 

INSERT [dbo].[Cliente] ([IdCliente], [Nombre], [Telefono], [Correo], [Direccion], [IdUsuario]) VALUES (2, N'Administrador', N'88888888', N'admin@ferreteria.com', N'Cartago', 1)
INSERT [dbo].[Cliente] ([IdCliente], [Nombre], [Telefono], [Correo], [Direccion], [IdUsuario]) VALUES (3, N'Andrey', NULL, N'jefersonmora333@gmail.com', NULL, 2)
SET IDENTITY_INSERT [dbo].[Cliente] OFF
GO
SET IDENTITY_INSERT [dbo].[Producto] ON 

INSERT [dbo].[Producto] ([IdProducto], [SKU], [Nombre], [Precio], [StockActual], [StockMinimo], [Estado], [IdCategoria]) VALUES (2, N'MAR001', N'Martillo', CAST(8500.00 AS Decimal(10, 2)), 20, 5, 1, 1)
INSERT [dbo].[Producto] ([IdProducto], [SKU], [Nombre], [Precio], [StockActual], [StockMinimo], [Estado], [IdCategoria]) VALUES (3, N'DES001', N'Destornillador', CAST(2500.00 AS Decimal(10, 2)), 50, 10, 1, 1)
INSERT [dbo].[Producto] ([IdProducto], [SKU], [Nombre], [Precio], [StockActual], [StockMinimo], [Estado], [IdCategoria]) VALUES (4, N'TAL001', N'Taladro', CAST(55000.00 AS Decimal(10, 2)), 8, 2, 1, 1)
SET IDENTITY_INSERT [dbo].[Producto] OFF
GO
SET IDENTITY_INSERT [dbo].[Rol] ON 

INSERT [dbo].[Rol] ([IdRol], [NombreRol], [Descripcion]) VALUES (1, N'Administrador', N'Control total del sistema')
INSERT [dbo].[Rol] ([IdRol], [NombreRol], [Descripcion]) VALUES (2, N'Cajero', N'Registro de ventas')
INSERT [dbo].[Rol] ([IdRol], [NombreRol], [Descripcion]) VALUES (3, N'Bodeguero', N'Gestión de inventario')
SET IDENTITY_INSERT [dbo].[Rol] OFF
GO
SET IDENTITY_INSERT [dbo].[Usuario] ON 

INSERT [dbo].[Usuario] ([IdUsuario], [Nombre], [Correo], [PasswordHash], [Estado], [FechaRegistro], [IdRol], [Identificacion]) VALUES (1, N'Administrador', N'admin@ferreteria.com', N'<<HASH_BCRYPT>>', 1, CAST(N'2026-07-21T14:01:53.010' AS DateTime), 1, N'')
INSERT [dbo].[Usuario] ([IdUsuario], [Nombre], [Correo], [PasswordHash], [Estado], [FechaRegistro], [IdRol], [Identificacion]) VALUES (2, N'Andrey', N'jefersonmora333@gmail.com', N'$2a$11$OE5AfCo56KMzIqLKqSThQee7ekvgC.15G4N91x/DZ4ZqcClqkHx1m', 1, CAST(N'2026-07-21T17:20:19.337' AS DateTime), 2, N'305310417')
SET IDENTITY_INSERT [dbo].[Usuario] OFF
GO
SET ANSI_PADDING ON
GO
/****** Objeto: Index [UQ__Producto__CA1ECF0DC2BCEE54] Fecha de script: 21/7/2026 17:45:55 ******/
ALTER TABLE [dbo].[Producto] ADD UNIQUE NONCLUSTERED 
(
	[SKU] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Objeto: Index [UQ__Usuario__60695A19634510BE] Fecha de script: 21/7/2026 17:45:55 ******/
ALTER TABLE [dbo].[Usuario] ADD UNIQUE NONCLUSTERED 
(
	[Correo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AuditoriaProducto] ADD  DEFAULT (getdate()) FOR [FechaCambio]
GO
ALTER TABLE [dbo].[Compra] ADD  DEFAULT (getdate()) FOR [FechaCompra]
GO
ALTER TABLE [dbo].[ErrorSistema] ADD  DEFAULT (getdate()) FOR [FechaError]
GO
ALTER TABLE [dbo].[MovimientoInventario] ADD  DEFAULT (getdate()) FOR [FechaMovimiento]
GO
ALTER TABLE [dbo].[Producto] ADD  DEFAULT ((0)) FOR [StockActual]
GO
ALTER TABLE [dbo].[Producto] ADD  DEFAULT ((1)) FOR [Estado]
GO
ALTER TABLE [dbo].[Usuario] ADD  DEFAULT ((1)) FOR [Estado]
GO
ALTER TABLE [dbo].[Usuario] ADD  DEFAULT (getdate()) FOR [FechaRegistro]
GO
ALTER TABLE [dbo].[Usuario] ADD  CONSTRAINT [DF_Usuario_Identificacion]  DEFAULT ('') FOR [Identificacion]
GO
ALTER TABLE [dbo].[Venta] ADD  DEFAULT (getdate()) FOR [FechaVenta]
GO
ALTER TABLE [dbo].[Venta] ADD  DEFAULT ('ACTIVA') FOR [Estado]
GO
ALTER TABLE [dbo].[AuditoriaProducto]  WITH CHECK ADD  CONSTRAINT [FK_AP_Producto] FOREIGN KEY([IdProducto])
REFERENCES [dbo].[Producto] ([IdProducto])
GO
ALTER TABLE [dbo].[AuditoriaProducto] CHECK CONSTRAINT [FK_AP_Producto]
GO
ALTER TABLE [dbo].[AuditoriaProducto]  WITH CHECK ADD  CONSTRAINT [FK_AP_Usuario] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[AuditoriaProducto] CHECK CONSTRAINT [FK_AP_Usuario]
GO
ALTER TABLE [dbo].[Cliente]  WITH CHECK ADD  CONSTRAINT [FK_Cliente_Usuario] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[Cliente] CHECK CONSTRAINT [FK_Cliente_Usuario]
GO
ALTER TABLE [dbo].[Compra]  WITH CHECK ADD  CONSTRAINT [FK_Compra_Proveedor] FOREIGN KEY([IdProveedor])
REFERENCES [dbo].[Proveedor] ([IdProveedor])
GO
ALTER TABLE [dbo].[Compra] CHECK CONSTRAINT [FK_Compra_Proveedor]
GO
ALTER TABLE [dbo].[Compra]  WITH CHECK ADD  CONSTRAINT [FK_Compra_Usuario] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[Compra] CHECK CONSTRAINT [FK_Compra_Usuario]
GO
ALTER TABLE [dbo].[DetalleCompra]  WITH CHECK ADD  CONSTRAINT [FK_DC_Compra] FOREIGN KEY([IdCompra])
REFERENCES [dbo].[Compra] ([IdCompra])
GO
ALTER TABLE [dbo].[DetalleCompra] CHECK CONSTRAINT [FK_DC_Compra]
GO
ALTER TABLE [dbo].[DetalleCompra]  WITH CHECK ADD  CONSTRAINT [FK_DC_Producto] FOREIGN KEY([IdProducto])
REFERENCES [dbo].[Producto] ([IdProducto])
GO
ALTER TABLE [dbo].[DetalleCompra] CHECK CONSTRAINT [FK_DC_Producto]
GO
ALTER TABLE [dbo].[DetalleVenta]  WITH CHECK ADD  CONSTRAINT [FK_DV_Producto] FOREIGN KEY([IdProducto])
REFERENCES [dbo].[Producto] ([IdProducto])
GO
ALTER TABLE [dbo].[DetalleVenta] CHECK CONSTRAINT [FK_DV_Producto]
GO
ALTER TABLE [dbo].[DetalleVenta]  WITH CHECK ADD  CONSTRAINT [FK_DV_Venta] FOREIGN KEY([IdVenta])
REFERENCES [dbo].[Venta] ([IdVenta])
GO
ALTER TABLE [dbo].[DetalleVenta] CHECK CONSTRAINT [FK_DV_Venta]
GO
ALTER TABLE [dbo].[MovimientoInventario]  WITH CHECK ADD  CONSTRAINT [FK_MI_Producto] FOREIGN KEY([IdProducto])
REFERENCES [dbo].[Producto] ([IdProducto])
GO
ALTER TABLE [dbo].[MovimientoInventario] CHECK CONSTRAINT [FK_MI_Producto]
GO
ALTER TABLE [dbo].[MovimientoInventario]  WITH CHECK ADD  CONSTRAINT [FK_MI_Usuario] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[MovimientoInventario] CHECK CONSTRAINT [FK_MI_Usuario]
GO
ALTER TABLE [dbo].[Producto]  WITH CHECK ADD  CONSTRAINT [FK_Producto_Categoria] FOREIGN KEY([IdCategoria])
REFERENCES [dbo].[Categoria] ([IdCategoria])
GO
ALTER TABLE [dbo].[Producto] CHECK CONSTRAINT [FK_Producto_Categoria]
GO
ALTER TABLE [dbo].[ProductoProveedor]  WITH CHECK ADD  CONSTRAINT [FK_PP_Producto] FOREIGN KEY([IdProducto])
REFERENCES [dbo].[Producto] ([IdProducto])
GO
ALTER TABLE [dbo].[ProductoProveedor] CHECK CONSTRAINT [FK_PP_Producto]
GO
ALTER TABLE [dbo].[ProductoProveedor]  WITH CHECK ADD  CONSTRAINT [FK_PP_Proveedor] FOREIGN KEY([IdProveedor])
REFERENCES [dbo].[Proveedor] ([IdProveedor])
GO
ALTER TABLE [dbo].[ProductoProveedor] CHECK CONSTRAINT [FK_PP_Proveedor]
GO
ALTER TABLE [dbo].[Usuario]  WITH CHECK ADD  CONSTRAINT [FK_Usuario_Rol] FOREIGN KEY([IdRol])
REFERENCES [dbo].[Rol] ([IdRol])
GO
ALTER TABLE [dbo].[Usuario] CHECK CONSTRAINT [FK_Usuario_Rol]
GO
ALTER TABLE [dbo].[Venta]  WITH CHECK ADD  CONSTRAINT [FK_Venta_Cliente] FOREIGN KEY([IdCliente])
REFERENCES [dbo].[Cliente] ([IdCliente])
GO
ALTER TABLE [dbo].[Venta] CHECK CONSTRAINT [FK_Venta_Cliente]
GO
ALTER TABLE [dbo].[Venta]  WITH CHECK ADD  CONSTRAINT [FK_Venta_Usuario] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[Venta] CHECK CONSTRAINT [FK_Venta_Usuario]
GO
/****** Objeto: StoredProcedure [dbo].[spActualizarContrasenna] Fecha de script: 21/7/2026 17:45:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spActualizarContrasenna]
    @IdUsuario INT,
    @Contrasenna VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Usuario
    SET PasswordHash = @Contrasenna
    WHERE IdUsuario = @IdUsuario;
END
GO
/****** Objeto: StoredProcedure [dbo].[spActualizarPerfil] Fecha de script: 21/7/2026 17:45:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spActualizarPerfil]
    @IdUsuario INT,
    @Identificacion VARCHAR(20),
    @Nombre VARCHAR(100),
    @CorreoElectronico VARCHAR(100),
    @Telefono VARCHAR(20),
    @Direccion VARCHAR(250)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Usuario
    SET
        Identificacion = @Identificacion,
        Nombre = @Nombre,
        Correo = @CorreoElectronico
    WHERE IdUsuario = @IdUsuario;

    UPDATE Cliente
    SET
        Nombre = @Nombre,
        Correo = @CorreoElectronico,
        Telefono = @Telefono,
        Direccion = @Direccion
    WHERE IdUsuario = @IdUsuario;
END
GO
/****** Objeto: StoredProcedure [dbo].[spConsultarProductos] Fecha de script: 21/7/2026 17:45:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spConsultarProductos]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        IdProducto,
        Nombre,
        Precio,
        StockActual
    FROM Producto
    WHERE Estado = 1;
END
GO
/****** Objeto: StoredProcedure [dbo].[spConsultarUsuario] Fecha de script: 21/7/2026 17:45:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spConsultarUsuario]
    @IdUsuario INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        U.IdUsuario,
        U.Identificacion,
        U.Nombre,
        U.Correo,
        U.IdRol,
        C.Telefono,
        C.Direccion
    FROM Usuario U
    LEFT JOIN Cliente C
        ON U.IdUsuario = C.IdUsuario
    WHERE U.IdUsuario = @IdUsuario;
END
GO
/****** Objeto: StoredProcedure [dbo].[spIniciarSesionUsuario] Fecha de script: 21/7/2026 17:45:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spIniciarSesionUsuario]
    @CorreoElectronico VARCHAR(100),
    @Contrasenna VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        U.IdUsuario AS Consecutivo,
        U.Identificacion,
        U.Nombre,
        U.Correo AS CorreoElectronico,
        U.PasswordHash AS Contrasenna,
        U.Estado,
        CAST(0 AS BIT) AS UsaContrasennaTemp,
        U.IdRol AS ConsecutivoRol,
        R.NombreRol
    FROM Usuario U
    INNER JOIN Rol R
        ON U.IdRol = R.IdRol
    WHERE U.Correo = @CorreoElectronico
      AND U.Estado = 1;
END
GO
/****** Objeto: StoredProcedure [dbo].[spRegistrarError] Fecha de script: 21/7/2026 17:45:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spRegistrarError]
(
    @Ruta VARCHAR(250),
    @MensajeError VARCHAR(MAX),
    @StackTrace VARCHAR(MAX)
)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO ErrorSistema
    (
        FechaError,
        Ruta,
        MensajeError,
        StackTrace
    )
    VALUES
    (
        GETDATE(),
        @Ruta,
        @MensajeError,
        @StackTrace
    );
END
GO
/****** Objeto: StoredProcedure [dbo].[spRegistrarUsuario] Fecha de script: 21/7/2026 17:45:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spRegistrarUsuario]
    @Identificacion VARCHAR(20),
    @Nombre VARCHAR(100),
    @CorreoElectronico VARCHAR(100),
    @Contrasenna VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS(SELECT 1 FROM Usuario WHERE Correo = @CorreoElectronico)
    BEGIN
        RAISERROR('El correo ya se encuentra registrado.',16,1);
        RETURN;
    END

    INSERT INTO Usuario
    (
        Identificacion,
        Nombre,
        Correo,
        PasswordHash,
        Estado,
        FechaRegistro,
        IdRol
    )
    VALUES
    (
        @Identificacion,
        @Nombre,
        @CorreoElectronico,
        @Contrasenna,
        1,
        GETDATE(),
        2
    );

    DECLARE @IdUsuario INT = SCOPE_IDENTITY();

    INSERT INTO Cliente
    (
        Nombre,
        Telefono,
        Correo,
        Direccion,
        IdUsuario
    )
    VALUES
    (
        @Nombre,
        NULL,
        @CorreoElectronico,
        NULL,
        @IdUsuario
    );

END
GO
/****** Objeto: StoredProcedure [dbo].[spValidarCorreo] Fecha de script: 21/7/2026 17:45:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spValidarCorreo]
    @CorreoElectronico VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        U.IdUsuario AS Consecutivo,
        U.Identificacion,
        U.Nombre,
        U.Correo AS CorreoElectronico,
        U.PasswordHash AS Contrasenna,
        U.Estado,
        CAST(0 AS BIT) AS UsaContrasennaTemp,
        U.IdRol AS ConsecutivoRol,
        R.NombreRol
    FROM Usuario U
    INNER JOIN Rol R
        ON U.IdRol = R.IdRol
    WHERE U.Correo = @CorreoElectronico
      AND U.Estado = 1;
END
GO


/*==============================================================
  MODULO DE CONTACTO Y SOPORTE (COPIADO VERBATIM DE FerreteriaDB (2).sql)
==============================================================*/

/*==============================================================
  MODULO DE CONTACTO Y SOPORTE
==============================================================*/

--=========================
-- TABLA CONTACTO
--=========================

IF OBJECT_ID('Contacto', 'U') IS NULL
BEGIN

CREATE TABLE Contacto
(
    IdContacto INT IDENTITY(1,1) PRIMARY KEY,

    Nombre NVARCHAR(150) NOT NULL,

    Correo NVARCHAR(150) NOT NULL,

    Telefono NVARCHAR(20) NULL,

    Asunto NVARCHAR(200) NOT NULL,

    Mensaje NVARCHAR(MAX) NOT NULL,

    Estado VARCHAR(20) NOT NULL DEFAULT('Pendiente'),

    FechaCreacion DATETIME NOT NULL DEFAULT(GETDATE())
);

END
GO

--=========================
-- TABLA RESPUESTAS
--=========================

IF OBJECT_ID('RespuestaContacto', 'U') IS NULL
BEGIN

CREATE TABLE RespuestaContacto
(
    IdRespuesta INT IDENTITY(1,1) PRIMARY KEY,

    IdContacto INT NOT NULL,

    Respuesta NVARCHAR(MAX) NOT NULL,

    RespondidoPor NVARCHAR(100) NOT NULL,

    FechaRespuesta DATETIME NOT NULL DEFAULT(GETDATE()),

    CONSTRAINT FK_Respuesta_Contacto
        FOREIGN KEY(IdContacto)
        REFERENCES Contacto(IdContacto)
);

END
GO

/*==============================================================
                PROCEDIMIENTOS ALMACENADOS
==============================================================*/

--=========================
-- REGISTRAR CONTACTO
--=========================

CREATE OR ALTER PROCEDURE SP_RegistrarContacto

    @Nombre NVARCHAR(150),
    @Correo NVARCHAR(150),
    @Telefono NVARCHAR(20),
    @Asunto NVARCHAR(200),
    @Mensaje NVARCHAR(MAX)

AS
BEGIN

SET NOCOUNT ON;

INSERT INTO Contacto
(
    Nombre,
    Correo,
    Telefono,
    Asunto,
    Mensaje
)

VALUES
(
    @Nombre,
    @Correo,
    @Telefono,
    @Asunto,
    @Mensaje
);

END
GO

--=========================
-- LISTAR CONTACTOS
--=========================

CREATE OR ALTER PROCEDURE SP_ListarContactos

AS
BEGIN

SET NOCOUNT ON;

SELECT
    IdContacto,
    Nombre,
    Correo,
    Telefono,
    Asunto,
    Estado,
    FechaCreacion

FROM Contacto

ORDER BY FechaCreacion DESC;

END
GO

--=========================
-- OBTENER CONTACTO
--=========================

CREATE OR ALTER PROCEDURE SP_ObtenerContacto

@IdContacto INT

AS
BEGIN

SET NOCOUNT ON;

SELECT *

FROM Contacto

WHERE IdContacto=@IdContacto;

END
GO

--=========================
-- RESPUESTAS DEL CONTACTO
--=========================

CREATE OR ALTER PROCEDURE SP_ObtenerRespuestas

@IdContacto INT

AS
BEGIN

SET NOCOUNT ON;

SELECT *

FROM RespuestaContacto

WHERE IdContacto=@IdContacto

ORDER BY FechaRespuesta;

END
GO

--=========================
-- RESPONDER CONTACTO
--=========================

CREATE OR ALTER PROCEDURE SP_ResponderContacto

    @IdContacto INT,
    @Respuesta NVARCHAR(MAX),
    @RespondidoPor NVARCHAR(100)

AS
BEGIN

SET NOCOUNT ON;

INSERT INTO RespuestaContacto
(
    IdContacto,
    Respuesta,
    RespondidoPor
)

VALUES
(
    @IdContacto,
    @Respuesta,
    @RespondidoPor
);

UPDATE Contacto

SET Estado='Respondido'

WHERE IdContacto=@IdContacto;

END
GO

--=========================
-- CAMBIAR ESTADO
--=========================

CREATE OR ALTER PROCEDURE SP_CambiarEstadoContacto

@IdContacto INT,
@Estado VARCHAR(20)

AS
BEGIN

SET NOCOUNT ON;

UPDATE Contacto

SET Estado=@Estado

WHERE IdContacto=@IdContacto;

END
GO

--=========================
-- ELIMINAR CONTACTO
--=========================

CREATE OR ALTER PROCEDURE SP_EliminarContacto

@IdContacto INT

AS
BEGIN

SET NOCOUNT ON;

DELETE FROM RespuestaContacto

WHERE IdContacto=@IdContacto;

DELETE FROM Contacto

WHERE IdContacto=@IdContacto;

END
GO

/*==============================================================
  MODULO ADMINISTRADOR (COPIADO VERBATIM DE script.sql)
==============================================================*/

CREATE   PROCEDURE [dbo].[spActualizarCategoria]
    @IdCategoria INT,
    @Nombre VARCHAR(100),
    @Descripcion VARCHAR(250) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Categoria
    SET Nombre = @Nombre,
        Descripcion = @Descripcion
    WHERE IdCategoria = @IdCategoria;
END
GO

CREATE   PROCEDURE [dbo].[spActualizarProducto]
    @IdProducto INT,
    @SKU VARCHAR(50),
    @Nombre VARCHAR(150),
    @Precio DECIMAL(10, 2),
    @StockActual INT,
    @StockMinimo INT,
    @IdCategoria INT
AS
BEGIN
    SET NOCOUNT OFF;

    UPDATE Producto
    SET SKU = @SKU,
        Nombre = @Nombre,
        Precio = @Precio,
        StockActual = @StockActual,
        StockMinimo = @StockMinimo,
        IdCategoria = @IdCategoria
    WHERE IdProducto = @IdProducto;
END
GO

CREATE   PROCEDURE [dbo].[spCambiarEstadoCategoria]
    @IdCategoria INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Categoria
    SET Estado = CASE WHEN ISNULL(Estado, 1) = 1 THEN 0 ELSE 1 END
    WHERE IdCategoria = @IdCategoria;
END
GO

CREATE   PROCEDURE [dbo].[spCambiarEstadoProducto]
    @IdProducto INT
AS
BEGIN
    SET NOCOUNT OFF;

    UPDATE Producto
    SET Estado = CASE WHEN Estado = 1 THEN 0 ELSE 1 END
    WHERE IdProducto = @IdProducto;
END
GO

CREATE   PROCEDURE [dbo].[spCambiarEstadoUsuario]
    @Consecutivo INT
AS
BEGIN
    SET NOCOUNT OFF;

    UPDATE Usuario
    SET Estado = CASE WHEN Estado = 1 THEN 0 ELSE 1 END
    WHERE IdUsuario = @Consecutivo;
END
GO

CREATE   PROCEDURE [dbo].[spCambiarRolUsuario]
    @Consecutivo INT,
    @ConsecutivoRol INT
AS
BEGIN
    SET NOCOUNT OFF;

    UPDATE Usuario
    SET IdRol = @ConsecutivoRol
    WHERE IdUsuario = @Consecutivo;
END
GO

CREATE   PROCEDURE [dbo].[spConsultarCategoriaPorId]
    @IdCategoria INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdCategoria, Nombre, Descripcion, ISNULL(Estado, 1) AS Estado
    FROM Categoria
    WHERE IdCategoria = @IdCategoria;
END
GO

CREATE   PROCEDURE [dbo].[spConsultarCategorias]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdCategoria, Nombre, Descripcion, ISNULL(Estado, 1) AS Estado FROM Categoria;
END
GO

CREATE   PROCEDURE [dbo].[spConsultarProductoPorId]
    @IdProducto INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        P.IdProducto,
        P.SKU,
        P.Nombre,
        P.Precio,
        P.StockActual,
        P.StockMinimo,
        P.Estado,
        P.IdCategoria,
        ISNULL(C.Nombre, 'General') AS NombreCategoria
    FROM Producto P
    LEFT JOIN Categoria C ON P.IdCategoria = C.IdCategoria
    WHERE P.IdProducto = @IdProducto;
END
GO

CREATE   PROCEDURE [dbo].[spConsultarProductosAdmin]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        P.IdProducto,
        P.SKU,
        P.Nombre,
        P.Precio,
        P.StockActual,
        P.StockMinimo,
        P.Estado,
        P.IdCategoria,
        ISNULL(C.Nombre, 'General') AS NombreCategoria
    FROM Producto P
    LEFT JOIN Categoria C ON P.IdCategoria = C.IdCategoria;
END
GO

CREATE   PROCEDURE [dbo].[spConsultarRoles]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        IdRol AS ConsecutivoRol,
        NombreRol
    FROM Rol;
END
GO

CREATE   PROCEDURE [dbo].[spConsultarUsuarios]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        U.IdUsuario AS Consecutivo,
        U.Identificacion,
        U.Nombre,
        U.Correo AS CorreoElectronico,
        U.Estado,
        U.IdRol AS ConsecutivoRol,
        R.NombreRol
    FROM Usuario U
    INNER JOIN Rol R ON U.IdRol = R.IdRol;
END
GO

CREATE   PROCEDURE [dbo].[spRegistrarCategoria]
    @Nombre VARCHAR(100),
    @Descripcion VARCHAR(250) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Categoria (Nombre, Descripcion, Estado)
    VALUES (@Nombre, @Descripcion, 1);
END
GO

CREATE   PROCEDURE [dbo].[spRegistrarProducto]
    @SKU VARCHAR(50),
    @Nombre VARCHAR(150),
    @Precio DECIMAL(10, 2),
    @StockActual INT,
    @StockMinimo INT,
    @IdCategoria INT
AS
BEGIN
    SET NOCOUNT OFF;

    INSERT INTO Producto (SKU, Nombre, Precio, StockActual, StockMinimo, Estado, IdCategoria)
    VALUES (@SKU, @Nombre, @Precio, @StockActual, @StockMinimo, 1, @IdCategoria);
END
GO


/*==============================================================
  MODULO DE CONTACTO Y SOPORTE (COPIADO VERBATIM DE FerreteriaDB (2).sql)
==============================================================*/

/*==============================================================
  MODULO DE CONTACTO Y SOPORTE
==============================================================*/

--=========================
-- TABLA CONTACTO
--=========================

IF OBJECT_ID('Contacto', 'U') IS NULL
BEGIN

CREATE TABLE Contacto
(
    IdContacto INT IDENTITY(1,1) PRIMARY KEY,

    Nombre NVARCHAR(150) NOT NULL,

    Correo NVARCHAR(150) NOT NULL,

    Telefono NVARCHAR(20) NULL,

    Asunto NVARCHAR(200) NOT NULL,

    Mensaje NVARCHAR(MAX) NOT NULL,

    Estado VARCHAR(20) NOT NULL DEFAULT('Pendiente'),

    FechaCreacion DATETIME NOT NULL DEFAULT(GETDATE())
);

END
GO

--=========================
-- TABLA RESPUESTAS
--=========================

IF OBJECT_ID('RespuestaContacto', 'U') IS NULL
BEGIN

CREATE TABLE RespuestaContacto
(
    IdRespuesta INT IDENTITY(1,1) PRIMARY KEY,

    IdContacto INT NOT NULL,

    Respuesta NVARCHAR(MAX) NOT NULL,

    RespondidoPor NVARCHAR(100) NOT NULL,

    FechaRespuesta DATETIME NOT NULL DEFAULT(GETDATE()),

    CONSTRAINT FK_Respuesta_Contacto
        FOREIGN KEY(IdContacto)
        REFERENCES Contacto(IdContacto)
);

END
GO

/*==============================================================
                PROCEDIMIENTOS ALMACENADOS
==============================================================*/

--=========================
-- REGISTRAR CONTACTO
--=========================

CREATE OR ALTER PROCEDURE SP_RegistrarContacto

    @Nombre NVARCHAR(150),
    @Correo NVARCHAR(150),
    @Telefono NVARCHAR(20),
    @Asunto NVARCHAR(200),
    @Mensaje NVARCHAR(MAX)

AS
BEGIN

SET NOCOUNT ON;

INSERT INTO Contacto
(
    Nombre,
    Correo,
    Telefono,
    Asunto,
    Mensaje
)

VALUES
(
    @Nombre,
    @Correo,
    @Telefono,
    @Asunto,
    @Mensaje
);

END
GO

--=========================
-- LISTAR CONTACTOS
--=========================

CREATE OR ALTER PROCEDURE SP_ListarContactos

AS
BEGIN

SET NOCOUNT ON;

SELECT
    IdContacto,
    Nombre,
    Correo,
    Telefono,
    Asunto,
    Estado,
    FechaCreacion

FROM Contacto

ORDER BY FechaCreacion DESC;

END
GO

--=========================
-- OBTENER CONTACTO
--=========================

CREATE OR ALTER PROCEDURE SP_ObtenerContacto

@IdContacto INT

AS
BEGIN

SET NOCOUNT ON;

SELECT *

FROM Contacto

WHERE IdContacto=@IdContacto;

END
GO

--=========================
-- RESPUESTAS DEL CONTACTO
--=========================

CREATE OR ALTER PROCEDURE SP_ObtenerRespuestas

@IdContacto INT

AS
BEGIN

SET NOCOUNT ON;

SELECT *

FROM RespuestaContacto

WHERE IdContacto=@IdContacto

ORDER BY FechaRespuesta;

END
GO

--=========================
-- RESPONDER CONTACTO
--=========================

CREATE OR ALTER PROCEDURE SP_ResponderContacto

    @IdContacto INT,
    @Respuesta NVARCHAR(MAX),
    @RespondidoPor NVARCHAR(100)

AS
BEGIN

SET NOCOUNT ON;

INSERT INTO RespuestaContacto
(
    IdContacto,
    Respuesta,
    RespondidoPor
)

VALUES
(
    @IdContacto,
    @Respuesta,
    @RespondidoPor
);

UPDATE Contacto

SET Estado='Respondido'

WHERE IdContacto=@IdContacto;

END
GO

--=========================
-- CAMBIAR ESTADO
--=========================

CREATE OR ALTER PROCEDURE SP_CambiarEstadoContacto

@IdContacto INT,
@Estado VARCHAR(20)

AS
BEGIN

SET NOCOUNT ON;

UPDATE Contacto

SET Estado=@Estado

WHERE IdContacto=@IdContacto;

END
GO

--=========================
-- ELIMINAR CONTACTO
--=========================

CREATE OR ALTER PROCEDURE SP_EliminarContacto

@IdContacto INT

AS
BEGIN

SET NOCOUNT ON;

DELETE FROM RespuestaContacto

WHERE IdContacto=@IdContacto;

DELETE FROM Contacto

WHERE IdContacto=@IdContacto;

END
GO

/*==============================================================
  MODULO ADMINISTRADOR (COPIADO VERBATIM DE script.sql)
==============================================================*/


GO


USE [master]
GO
ALTER DATABASE [DB_FERRETERIA] SET READ_WRITE 
GO


/*==============================================================
  spConsultarProductosDestacados
==============================================================*/

CREATE PROCEDURE [dbo].[spConsultarProductosDestacados]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 5
        P.IdProducto,
        P.Nombre,
        P.Precio,
        P.StockActual
    FROM Producto P
    WHERE P.Estado = 1
    ORDER BY P.Precio DESC;
END
GO
