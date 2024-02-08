select * from Empresa

-- 1. Clientes facturas < 200 
-- no hay
select x.* from LibroVentaContribuyente x
 inner join Venta v
    on x.Venta = v.Oid 
 where v.Empresa = 2
   and x.GravadaLocal > 0
   and x.GravadaLocal <= 200
   and v.Estado != 2
   and x.Fecha between '20230701' and '20231231'

-- 2. Operaciones de Clientes mayores de 200.00
select Format(x.Fecha, 'MM') as Mes, 
	   iif(x.Nit is not null, 1, 3) as TipoDocumento,
	   coalesce(x.Nit, coalesce(x.Dui, '')) as NumeroDocumento,
	   c.Nombre as Nombre, 
	   Format(x.Fecha, 'ddMMyyyy') as Fecha,
	   3 as TipoFactura, -- credito fiscal
	   v.NoFactura as NumeroFactura,
	   x.GravadaLocal as Valor,
	   x.DebitoFiscal as Iva,
	   Format(x.Fecha, 'yyyy') as Anio,
	   4 as NumeroAnexo
  from LibroVentaContribuyente x
 inner join Venta v
    on x.Venta = v.Oid 
 inner join Tercero c
    on v.Cliente = c.Oid 
 where v.Empresa = 2
   and x.GravadaLocal >= 200
  -- and v.Estado != 2
   and cast(x.Fecha as Date) between '20230701' and '20231231'
   and v.GCRecord is null
union 
select Format(v.Fecha, 'MM') as Mes, 
	   iif(td.Tipo = 'NIT', 1, 3) as TipoDocumento,
	   coalesce(td.Numero, '') as NumeroDocumento,
	   c.Nombre as Nombre, 
	   Format(v.Fecha, 'ddMMyyyy') as Fecha,
	   1 as TipoFactura, -- factura consumidor
	   v.NoFactura as NumeroFactura,
	   coalesce(v.Exenta, 0.00) + coalesce(v.Gravada, 0.00)  as Valor,
	   coalesce(v.Iva, 0.00) as Iva,
	   Format(v.Fecha, 'yyyy') as Anio,
	   4 as NumeroAnexo
  from Venta v
 inner join Tercero c
    on v.Cliente = c.Oid 
  left join 
 (select Tercero, Tipo, Numero 
    from TerceroDocumento
   where Tipo in ('DUI', 'NIT')) as td
    on c.Oid = td.Tercero
 where v.Empresa = 2
   and (coalesce(v.Exenta, 0.00) + coalesce(v.Gravada, 0.00) + coalesce(v.Iva, 0.00)) >= 200
--   and v.Estado != 2
   and cast(v.Fecha as Date) between '20230701' and '20231231'
   and v.TipoFactura = 'COVE02'
   and v.GCRecord is null

-- 3. Proveedores
select Format(x.Fecha, 'MM') as Mes, 
       coalesce(x.Nit, coalesce(x.Dui, '')) as DocumentoProveedor,
	   t.Nombre, Format(x.Fecha, 'ddMMyyyy') as Fecha, x.ClaseDocumento,
	   cast(x.TipoDocumento as int) as TipoDocumento, coalesce(x.Serie, '') as Serie,
	   x.Numero, 0 as NumeroCtrl, x.InternaGravada, x.CreditoFiscal, 
	   Format(x.Fecha, 'yyyy') as Fecha, 1 as Anexo
  from LibroCompra x
 inner join CompraFactura c
    on x.CompraFactura = c.Oid
 inner join Tercero t
    on c.Proveedor = t.Oid 
 where c.Empresa = 2
   and cast(x.Fecha as Date) between '20230701' and '20231231'
   and c.GCRecord is null