--- Sentencias que modifican bd por cambios relacionados con generacion de JSON de DTE

-- 1. borrar tabla que no tiene uso
drop table GloCategoriaProducto
go

-- 2. Crear la tabla con los cat�logos del sistema de transmisi�n
USE [SbtErp2]
GO

/****** Object:  Table [dbo].[DteCatalogo]    Script Date: 11/9/2024 00:43:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DteCatalogo](
	[Oid] [int] IDENTITY(1,1) NOT NULL,
	[Tipo] [smallint] NOT NULL,
	[Codigo] [varchar](4) NOT NULL,
	[Concepto] [varchar](60) NOT NULL,
	[Equivalente] [varchar](6) NULL,
	[Activo] [bit] NOT NULL,
	[OptimisticLockField] [int] NULL,
	[GCRecord] [int] NULL,
 CONSTRAINT [PKDteCatalogo] PRIMARY KEY CLUSTERED 
(
	[Oid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[DteCatalogo] ADD  CONSTRAINT [DF__DteCatalo__Activ__69927322]  DEFAULT ((1)) FOR [Activo]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Llave primaria' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DteCatalogo', @level2type=N'COLUMN',@level2name=N'Oid'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificar los diferentes cat�logos del sistema de transmisi�n' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DteCatalogo', @level2type=N'COLUMN',@level2name=N'Tipo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'C�digo del registro correspondiente al cat�logo identificado de IdCatalogo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DteCatalogo', @level2type=N'COLUMN',@level2name=N'Codigo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Concepto o etiqueta' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DteCatalogo', @level2type=N'COLUMN',@level2name=N'Concepto'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Valor correspondiente en el ERP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DteCatalogo', @level2type=N'COLUMN',@level2name=N'Equivalente'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica cuando el registro est� activo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DteCatalogo', @level2type=N'COLUMN',@level2name=N'Activo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabla que corresponde a los cat�logos de apoyo del sistema de transmisi�n de Dte, excepto Actividad Economica, Pais, Departamento, Municipio, Tributos, Regimen' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DteCatalogo'
GO

-- 3. insertar constantes (revisar si mejor se llevan al archivo de configuraci�n

	insert into Constante
		   (Codigo, Nombre, TipoConstante, Valor, UsuarioCrea, FechaCrea)
	values ('DTE_AMBIENTE', 'DTE Ambiente Destino - Pruebas', 2, '00', 'Admin', '2024-09-10 23:41:17.760'),
	       ('DTE_MODELO_FACTURACION', 'DTE Modelo Facturaci�n - Previo', 1, '1', 'Admin', '2024-09-10 23:42:25.253'),
		   ('DTE_TIPO_TRANSMISION', 'DTE Tipo Transmisi�n - Normal', 1, '1', 'Admin', '2024-09-10 23:44:19.657')
    go

-- 4. Evaluar Modificar la tabla ProClasificaci�n para incorporar la columna TipoItem del cuerpo del Dte
alter table ProClasificacion
  add TipoItem smallint default null
go

select * from UnidadMedida

-- 5. Agregar UnidadMedida al producto o al detalle de la venta porque es requerido por el dte

-- 6. borrar la tabla ProPresentacion ya no se utiliza
drop table ProdPresentacion

-- 7. modificar la tabla ProPresentacion y otros cambios 

drop index iPresentacion_Producto on Producto
go
alter table Producto 
  drop constraint FK_Producto_Presentacion
go
alter table Producto
  drop column Presentacion
go
delete from ProductoPresentacion
go
dbcc checkident('ProductoPresentacion', reseed, 0)
go
alter table ProductoPresentacion
  add Producto int not null
go
alter table ProductoPresentacion
  add constraint FK_ProductoPresentacion_Producto foreign key(Producto) references Producto(Oid)
go
alter table ProductoPresentacion
  drop column Codigo 
go