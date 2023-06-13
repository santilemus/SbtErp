use SbtErp
go
select * from Tercero
 where Nombre like '%Mar%'
select * from TerceroDocumento
 where Tercero = 39


alter table CompraFactura 
  add Fovial numeric(14, 2) default 0.0 null
go

declare @Empresa smallint = 1
/*
insert into CompraFactura
       (Empresa, Numero, Fecha, Tipo, TipoFactura, NumeroFactura, Proveedor, CondicionPago, DiasCredito, Moneda, ValorMoneda,
	    OrdenCompra, Origen, Concepto, Exenta, NoSujeta, Gravada, Iva, IvaRetenido, IvaPercibido, Fovial, Renta, Saldo, Estado,
		Clase, Comentario, UsuarioCrea, FechaCrea)
*/
select @Empresa, row_number() over(partition by Year(d.Fecha) order by  d.NIT, d.NoFactura) as Numero, d.Fecha, 0 /* servicio */,
       'COVE01' /* credito fiscal */, d.NoFactura, td1.Tercero, 0 /* condicion pago */, 0 /* dias credito */,
	   'USD', 1.0, null /*orden compra */, 0 /* local */, null, 0.0, 0.0, d.InternaGravada, d.CreditoFiscal, 0.0, 0.0,  0.0, 0.0,
	   0.0, 1 /*estado pagada */, d.Clase, null, 'Admin', current_timestamp
 from declaraciones_iva d
 left join TerceroDocumento td1
   on d.Nit = td1.Numero
  and 'NIT' = td1.Tipo

select * from TerceroDocumento 
 where Numero in ('05110606161016', '06141407830018', '06140107740062')


select * from declaraciones_iva
 where Empresa = 1


select * from Moneda
 where Codigo = 'USD'


 select Numero, Fecha, Format(Cast(NumeroFactura as int), '00000000'), Gravada, Iva, Saldo, Estado
  from CompraFactura


sp_help LibroCompra

alter table LibroCompra
drop column IvaPercibido, RetencionTercero
go


select * from Listas where Categoria = 15

select * from Tributo

Iif([This.Proveedor.Origen] = 3, [This.Empresa.Renta25], [This.Proveedor.Origen] = 2 And [Tipo] In (0, 3), [This.Empresa.Renta20], [This.Proveedor.TipoPersona] = 2 And [Tipo] = 3, [This.Empresa.Renta5], [This.Proveedor.TipoPersona] = 1 And [Tipo] In (0, 3), [This.Empresa.Renta10], 0.0) * ([Gravada] + [NoSujeta])

alter table LibroCompra
  add PreCalculo bit default 0
go


select * from Moneda
select * from Constante


select l.Fecha, l.ClaseDocumento, l.TipoDocumento, l.Numero, l.Nit, t.Nombre as NombreProveedor, l.InternaExenta, 
       l.InternacionExenta, l.ImportacionExenta, l.InternaGravada, l.InternacionGravadaBien, l.ImportacionGravadaBien, 
	   l.ImportacionGravadaServicio, l.CreditoFiscal, 
	   coalesce(l.InternaExenta, 0) + coalesce(l.InternacionExenta, 0) + coalesce(l.ImportacionExenta, 0) +
	   coalesce(l.InternaGravada, 0) + coalesce(l.InternacionGravadaBien, 0) + coalesce(l.ImportacionGravadaBien, 0) +
	   coalesce(l.IMportacionGravadaServicio, 0) + coalesce(l.CreditoFiscal, 0) + coalesce(CompraExcluido, 0) as total, 
	   l.DUI, '03' as Anexo
  from dbo.LibroCompra l 
 inner join CompraFactura f
    on l.CompraFactura = f.Oid
 inner join Tercero t
    on l.Proveedor = t.Oid
 where f.Empresa = @Empresa
   and l.Fecha between @FechaInicio and @FechaFin

