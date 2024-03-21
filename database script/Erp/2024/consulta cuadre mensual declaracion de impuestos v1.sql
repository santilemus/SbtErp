declare @CodigoMoneda varchar(3) = 'USD'
declare @OidEmpresa int = 1
declare @FechaDesde datetime = '20240101'
declare @FechaHasta datetime = '20240131'

-- Venta
/*
select Format(v.Fecha, 'MMMM-yyyy') as Mes, Concat(v.TipoFactura, ' ', l.Nombre) as Tipo, m.Plural,
       count(*) as Cantidad, sum(coalesce(v.Exenta, 0) / v.ValorMoneda * m.FactorCambio)  as Exenta,
       sum(coalesce(v.Gravada, 0) / v.ValorMoneda * m.FactorCambio) as Gravada, 
	   sum(coalesce(v.IVA, 0) / v.ValorMoneda * m.FactorCambio) as Iva,
	   sum(coalesce(v.IvaRetenido, 0) / v.ValorMoneda * m.FactorCambio) as IvaRetenido,
	   sum(coalesce(v.IvaPercibido, 0) / v.ValorMoneda * m.FactorCambio) as IvaPercibido, 
	   sum((coalesce(v.Exenta, 0) + coalesce(v.Gravada, 0) + coalesce(v.Iva, 0) - coalesce(v.IvaRetenido, 0) + 
		   coalesce(v.IvaPercibido, 0) + coalesce(v.noSujeta, 0)) / v.ValorMoneda * m.FactorCambio) as Total
  from dbo.Venta v
 inner join dbo.Listas l
    on v.TipoFactura = l.Codigo
 inner join dbo.Moneda m
    on m.Codigo = @CodigoMoneda
 where v.Empresa = @OidEmpresa
   and v.Estado <> 2 -- anulado 
   and cast(v.Fecha as Date) between @FechaDesde and @FechaHasta
 group by Format(v.Fecha, 'MMMM-yyyy'), v.TipoFactura, l.Nombre, m.Plural
 */
  select *
   from vwVentaMes v
  where v.Emp

 -- Libro Venta Contribuyente
select Format(c.Fecha, 'MMMM-yyyy') as Mes, c.TipoDocumento,
       count(*) as Cantidad, sum(coalesce(c.Exenta, 0)) as Exenta,
       sum(coalesce(c.GravadaLocal, 0)) as Gravada, sum(coalesce(c.DebitoFiscal, 0)) as Iva,
	   sum(coalesce(c.IvaRetenido, 0)) as IvaRetenido,
	   sum(coalesce(c.IvaPercibido, 0)) as IvaPercibido, 
	   sum(coalesce(c.Exenta, 0) + coalesce(c.GravadaLocal, 0) + coalesce(c.DebitoFiscal, 0) - coalesce(c.IvaRetenido, 0) + 
		   coalesce(c.IvaPercibido, 0) + coalesce(c.noSujeta, 0)) as Total, 'Venta Contribuyente' as Grupo
  from LibroVentaContribuyente c
 inner join Venta v
    on c.Venta = v.Oid 
 inner join Moneda m
 where v.Empresa = 1
   and v.Estado <> 2 -- anulado 
   and Month(c.fecha) = 01
   and Year(c.fecha) = 2024
 group by Format(c.Fecha, 'MMMM-yyyy'), c.TipoDocumento

 -- Libro Venta Consumidor
 select Format(c.Fecha, 'MMMM-yyyy') as Mes, 
        case c.TipoDocumento 
		     when '01' then 'Factura de Consumidor Final'
		     when '02' then 'Factura de Venta Simplificada'
			 when '10' then 'Ticket de Maquina Registradora'
			 when '11' then 'Factura de Exportación'
			 else '*** NO IDENTIFICADO ***'
	    end as TipoDocumento,
       count(*) as Cantidad, sum(coalesce(c.Exenta, 0)) as Exenta,
	   sum(coalesce(c.InternaExenta, 0)) as InternaExenta,
       sum(coalesce(c.GravadaLocal, 0))  as GravadaLocal,
	   sum(coalesce(ExportacionCA, 0)) as ExportacionCA,
	   sum(coalesce(ExportacionFueraCA, 0)) as ExportacionFueraCA,
	   sum(coalesce(ExportacionServicio, 0)) as ExportacionServicio,
	   sum(coalesce(VentaZonaFranca, 0)) as VentaZonaFranca,
	   sum(coalesce(VentaTercero, 0)) as VentaTercero,
	   sum(coalesce(NoSujeta, 0)) as NoSujeta,
	   sum(coalesce(c.InternaExenta, 0) + coalesce(c.GravadaLocal, 0) + coalesce(ExportacionCA, 0) +
	       coalesce(c.ExportacionFueraCA, 0) + coalesce(c.ExportacionServicio, 0) + 
		   coalesce(c.VentaZonaFranca, 0) + coalesce(VentaTercero, 0) + coalesce(NoSujeta, 0)) as Total
  from LibroVentaConsumidor c
 where c.Empresa = 1
   and Month(c.fecha) = 01
   and Year(c.fecha) = 2024
 group by Format(c.Fecha, 'MMMM-yyyy'), c.TipoDocumento

-- documentos de venta anulados
select v.Fecha, v.NoFactura, a.Resolucion, a.Serie, 
       choose(a.Clase, 'Impreso por Imprenta', 'Formulario Único', 'Otro', 'Dte') as Clase,
       v.FechaAnula, v.UsuarioAnulo, Concat(v.TipoFactura, ' ', l.Nombre) as Tipo,
       coalesce(Exenta, 0) as Exenta, coalesce(Gravada, 0) as Gravada, coalesce(IVA, 0) as Iva, 
	   coalesce(v.IvaRetenido, 0) as IvaRetenido, coalesce(v.IvaPercibido, 0) as IvaPercibido, 
	   coalesce(v.Exenta, 0) + coalesce(v.Gravada, 0) + coalesce(v.Iva, 0) - coalesce(v.IvaRetenido, 0) + 
	   coalesce(v.IvaPercibido, 0) + coalesce(v.noSujeta, 0) as Total
  from Venta v
 inner join Listas l
    on v.TipoFactura = l.Codigo
 inner join FacAutorizacionDoc a
    on v.AutorizacionDocumento = a.Oid
 where v.Empresa = 1
   and v.Estado = 2 -- anulado 
   and Month(v.fecha) = 01
   and Year(v.fecha) = 2024

 -- Compra
 select Format(c.Fecha, 'MMMM-yyyy') as Mes,  Concat(c.TipoFactura, ' ', l.Nombre) as TipoFactura,
        m.Plural, count(*) as Cantidad, 
		sum(coalesce(c.Exenta, 0)/ coalesce(v.ValorMoneda, 1.00) * m.FactorCambio) as Exenta, 
		sum(coalesce(c.Gravada, 0)/ coalesce(v.ValorMoneda, 1.00) * m.FactorCambio) as Gravada,
		sum(coalesce(c.Iva, 0)/coalesce(v.ValorMoneda, 1.00)) as Iva, 
		sum(coalesce(c.IvaRetenido, 0)/coalesce(v.ValorMoneda, 1.00)) as IvaRetenido, 
		sum(coalesce(c.IvaPercibido, 0)/coalesce(v.ValorMoneda, 1.00)) as IvaPercibido, 
		sum(coalesce(c.NoSujeta, 0)/coalesce(v.ValorMoneda, 1.00)) as NoSujeta,
		sum(coalesce(c.Fovial, 0)/coalesce(v.ValorMoneda, 1.00)) as Fovial, 
		sum(coalesce(c.Renta, 0) / coalesce(v.ValorMoneda, 1.00)) as Renta,
		sum((coalesce(c.Exenta, 0) + coalesce(c.Gravada, 0) + coalesce(c.Iva, 0) + coalesce(c.IvaPercibido, 0) -
		    coalesce(c.IvaRetenido, 0) + coalesce(c.NoSujeta, 0) + coalesce(c.Fovial, 0)) / coalesce(v.ValorMoneda, 1.00)) as Total
   from CompraFactura c
  inner join Listas l
     on c.TipoFactura = l.Codigo 
  inner join dbo.Moneda m
	 on m.Codigo = @Moneda
  where c.Empresa = @EmpresaOid
    and cast(c.Fecha as Date) between @FechaDesde and @FechaHasta
	and c.Estado != 2 -- Anulado
	and c.TipoFactura in ('COVE01', 'COVE03', 'COVE06', 'COVE12', 'COVE13')
  group by Format(c.Fecha, 'MMMM-yyyy'), c.TipoFactura, l.Nombre

select Format(c.Fecha, 'MMMM-yyyy') as Mes, 
       case c.TipoDocumento 
	      when '03' then 'Comprobante Crédito Fiscal'
		  when '05' then 'Nota de Crédito'
		  when '06' then 'Nota de Débito'
		  when '11' then 'Factura Exportación'
		  when '12' then 'Declaración de Mercancías'
		  when '13' then 'Mandamiento de Ingreso'
		  else '** INDEFINIDO **'
	   end as TipoDocumento,
	   sum(coalesce(c.InternaExenta,0)) as InternaExenta, sum(coalesce(c.InternacionExenta, 0)) as InternacionExenta,
	   sum(coalesce(c.ImportacionExenta, 0)) as ImportacionExenta, sum(coalesce(c.InternaGravada, 0)) as InternaGravada,
	   sum(coalesce(c.InternacionGravadaBien, 0)) as InternacionGravadaBien,
	   sum(coalesce(c.ImportacionGravadaBien, 0)) as ImportacionGravadaBien,
	   sum(coalesce(c.ImportacionGravadaServicio, 0)) as ImportacionGravadaServicio,
	   sum(coalesce(c.CreditoFiscal, 0)) as CreditoFiscal, sum(coalesce(c.CompraExcluido, 0)) as CompraExcluido,
	   sum(coalesce(c.Fovial, 0)) as Fovial, sum(coalesce(c.IvaPercibido, 0)), sum(Coalesce(c.IvaRetenido, 0)),
	   sum(coalesce(c.InternaExenta, 0) + coalesce(c.InternacionExenta, 0) + coalesce(c.ImportacionExenta, 0) +
	       coalesce(c.InternaGravada, 0) + coalesce(c.InternacionGravadaBien, 0) + coalesce(c.ImportacionGravadaBien, 0) +
		   coalesce(c.ImportacionGravadaServicio, 0) + coalesce(c.CreditoFiscal, 0) + coalesce(c.CompraExcluido, 0) +
		   coalesce(c.Fovial, 0) + coalesce(c.IvaPercibido, 0) - Coalesce(c.IvaRetenido, 0)) as Total
  from LibroCompra c
 inner join CompraFactura f
    on c.CompraFactura = f.Oid 
 where f.Empresa = 1
   and month(c.Fecha) = 01
   and year(c.Fecha) = 2024
   and f.Estado != 2 -- anulado
 group by Format(c.Fecha, 'MMMM-yyyy'), c.TipoDocumento

 select Codigo, Plural, Nombre 
  from Moneda
 where Activa = 1
   and Plural is not null


