--- Sentencias que modifican bd por cambios relacionados con generacion de JSON de DTE

-- 1. borrar tabla que no tiene uso
drop table GloCategoriaProducto
go

-- 2. Crear la tabla con los catálogos del sistema de transmisión
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificar los diferentes catálogos del sistema de transmisión' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DteCatalogo', @level2type=N'COLUMN',@level2name=N'Tipo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Código del registro correspondiente al catálogo identificado de IdCatalogo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DteCatalogo', @level2type=N'COLUMN',@level2name=N'Codigo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Concepto o etiqueta' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DteCatalogo', @level2type=N'COLUMN',@level2name=N'Concepto'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Valor correspondiente en el ERP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DteCatalogo', @level2type=N'COLUMN',@level2name=N'Equivalente'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica cuando el registro está activo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DteCatalogo', @level2type=N'COLUMN',@level2name=N'Activo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabla que corresponde a los catálogos de apoyo del sistema de transmisión de Dte, excepto Actividad Economica, Pais, Departamento, Municipio, Tributos, Regimen' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DteCatalogo'
GO

-- 3. insertar constantes (revisar si mejor se llevan al archivo de configuración

	insert into Constante
		   (Codigo, Nombre, TipoConstante, Valor, UsuarioCrea, FechaCrea)
	values ('DTE_AMBIENTE', 'DTE Ambiente Destino - Pruebas', 2, '00', 'Admin', '2024-09-10 23:41:17.760'),
	       ('DTE_MODELO_FACTURACION', 'DTE Modelo Facturación - Previo', 1, '1', 'Admin', '2024-09-10 23:42:25.253'),
		   ('DTE_TIPO_TRANSMISION', 'DTE Tipo Transmisión - Normal', 1, '1', 'Admin', '2024-09-10 23:44:19.657')
    go

-- 4. Evaluar Modificar la tabla ProClasificación para incorporar la columna TipoItem del cuerpo del Dte
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

-- 8. Agregar UnidadMedida a Producto y la referencia
alter table Producto
  add UnidadMedida int null
go 
alter table Producto
  add constraint FK_Producto_UnidadMedida foreign key (UnidadMedida) references UnidadMedida(Oid)
go

alter table Venta
  add NumeroControl varchar(32),
      CodigoGeneracion uniqueidentifier,
	  SelloRecibido varchar(50)
go

-- 9. Modificamos Listas para agregar ahi codigoalterno para el codigo de los documentos de los dte y noos
--    evitamos complicaciones innecesarias
alter table Listas
  add CodigoAlterno varchar(6) null
go

-- 10.1 Agregamos columna NoCaja al usuario
alter table PermissionPolicyUser
  add Caja int null
go

-- 10.2 Actualizamos todos los documentos de venta emitidos a la caja 1, asumiendo que ya se creo el registro en caja
update Venta 
   set Caja = 1
 where Empresa = 1
go
alter table VentaDetalle
  add constraint FK_VentaDetalle_UnidadMedida foreign key(UnidadMedida) references UnidadMedida(Oid)
go
-- repetir para las otras empresas (previa creacion del registro de la caja)

-- 11. atualizar la unidad de medida en productos
update Producto 
   set UnidadMedida = 55
go

--12 . Borrar tabla que ya no se utiliza
drop table Usuario 
go

--13. Agregar UnidadMedida OrdenCompraDetalle
alter table OrdenCompraDetalle
  add UnidadMedida int null
go

--14. Agregar UnidadMedida CompraFacturaDetalle
alter table CompraFacturaDetalle
  add UnidadMedida int null
go