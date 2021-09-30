drop index idx_TributoNombreCorto on Tributo
go
alter table Tributo
   drop column NombreAbreviado
go
alter table Tributo
   alter column Nombre varchar(50)
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
       (2, 'Percepcion Iva', 0, null, 1,'SBT.Apps.Facturacion.Module.BusinessObjects\Venta'),
	   (3, 'Iva - Compra', 0, null, 1, 'SBT.Apps.Compra.Module.BusinessObjects\CompraFactura'),
	   (4, 'Retención Iva', 0, null, 1, 'SBT.Apps.Compra.Module.BusinessObjects\CompraFactura'),
       (5, 'ISR', 0, null, 1, 'SBT.Apps.Compra.Module.BusinessObjects\CompraFactura')
go
set identity_insert Tributo off
go

select * from Tributo
