drop index idx_TributoNombreCorto on Tributo
go
alter table Tributo
   drop column NombreAbreviado
go
alter table Tributo
   alter column Nombre varchar(50)
go
alter table Tributo
   alter column Formula nvarchar(500)
go
delete from Tributo
go
dbcc checkident('Tributo', reseed, 0)
go
set identity_insert Tributo on
go
insert into Tributo
       (Oid, Nombre, Clase, Formula, Activo, TipoBO)
values (1, 'Iva - Venta', 0, null, 1, 'SBT.Apps.Facturacion.Module.BusinessObjects\Venta'),
       (2, 'Retencion Iva - Venta', 0, null, 1,'SBT.Apps.Facturacion.Module.BusinessObjects\Venta'),
       (3, 'Percepcion Iva - Venta', 0, null, 1,'SBT.Apps.Facturacion.Module.BusinessObjects\Venta'),
	   (4, 'Iva - Compra', 0, null, 1, 'SBT.Apps.Compra.Module.BusinessObjects\CompraFactura'),
	   (5, 'Retención Iva - Compra', 0, null, 1, 'SBT.Apps.Compra.Module.BusinessObjects\CompraFactura'),
	   (6, 'Percepción Iva - Compra', 0, null, 1, 'SBT.Apps.Compra.Module.BusinessObjects\CompraFactura'),
       (7, 'ISR', 0, null, 1, 'SBT.Apps.Compra.Module.BusinessObjects\CompraFactura')
go
set identity_insert Tributo off
go

insert into Listas
       (Codigo, Nombre, Categoria, Comentario, Activo)
values ('COVE12', 'Declaración de Mercancias', 15, null, 1),
       ('COVE13', 'Mandamiento de Ingreso - Import', 15, null, 1)

alter table Tercero
   add Origen smallint null
go
update Tercero 
   set Origen = 0

