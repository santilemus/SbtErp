USE [SbtErp]
GO
/****** Object:  StoredProcedure [dbo].[spGeneraLibroCompra]    Script Date: 13/6/2023 16:03:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- ================================================================================================
-- Author:		Santiago Enrique Lemus
-- Create date: 31/05/2023
-- Description:	Generar Libro de Compras - IVA
-- Modificaciones
-- 13/junio/2023 por SELM
-- Se agrego la columa correlativo para cumplir con la dispuesto en el instructivo de aplicacion del
-- codigo tributario
-- se quito la parte de del begin try .. begin tran t1
-- Se agregan las columnas Fovial, Cotrans, IvaRetenido, IvaPercibido
-- ================================================================================================
create or alter procedure [dbo].[spGeneraLibroCompra](
   @Empresa     int,
   @Moneda      varchar(3),
   @Mes         int,
   @Anio        int)
as
begin
  set nocount on
  declare @corrInicio int = 0
  declare @filas int
  
  declare @factorCambio numeric(12,2);
  select @factorCambio = m.FactorCambio from Moneda m
   where m.Codigo = @Moneda

	select @corrInicio = coalesce(max(Correlativo), 0) from LibroCompra x
		inner join CompraFactura c
		   on x.CompraFactura = c.Oid
		where c.Empresa = @Empresa
		and Year(x.Fecha) = @Anio

	select @filas = count(*) from LibroCompra x
		inner join CompraFactura y
		on x.CompraFactura = y.Oid
		where y.Empresa = @Empresa 
		and Month(x.Fecha) = @Mes
		and Year(x.Fecha) = @Anio

	if (@filas = 0)
	begin
		-- pendiente de resolver el tema de las compras de bienes y servicios en el territorio nacional
		-- de usuarios de zonas francas o dpa (TipoFactura = 'COVE03'). Este caso aplicara cuando la empresa
		-- de la sesion sea un usuario de zona franca o una dpa
		insert into LibroCompra
				(Correlativo, Fecha, ClaseDocumento, TipoDocumento, Numero, Nit, Proveedor, InternaExenta, InternacionExenta,
				ImportacionExenta, InternaGravada, InternacionGravadaBien, ImportacionGravadaBien, 
				ImportacionGravadaServicio, CreditoFiscal, CompraExcluido, CompraFactura, Fovial, Cotrans, IvaPercibido, IvaRetenido)
		select @CorrInicio + row_number() over(partition by x.Empresa, Year(x.Fecha) order by x.Fecha),
		        x.Fecha, x.Clase, iif(x.TipoFactura = 'COVE12', '12', iif(x.TipoFactura = 'COVE13', '13', '03')), 
				format(cast(ltrim(rtrim(x.NumeroFactura)) as int), '00000000'), td.Numero, x.Proveedor, 
				iif(x.TipoFactura = 'COVE01', x.Exenta, 0.0), 
				iif(x.TipoFactura in ('COVE12', 'COVE13') and d.Pais in ('CRI', 'GTM', 'HND', 'NIC'), x.Exenta, 0.0), -- internacion exenta
				iif(x.TipoFactura in ('COVE12', 'COVE13'), x.Exenta, 0.0), -- import exenta 
				iif(x.TipoFactura = 'COVE01', x.Gravada, 0.0), 
				-- Internacion Gravada bien 
				iif(x.TipoFactura in ('COVE12', 'COVE13') and x.Tipo <> 0 and d.Pais in ('CRI', 'GTM', 'HND', 'NIC'), x.Gravada, 0.0),  
				iif(x.TipoFactura in ('COVE12', 'COVE13') and x.Tipo <> 0, x.Gravada, 0.0), -- importacion gravada bien 
				iif(x.TipoFactura in ('COVE12', 'COVE13') and x.Tipo = 0, x.Gravada, 0.0), -- importacion gravada servicio
				x.Iva, iif(x.TipoFactura = 'COVE06', x.Gravada, 0.0), -- compra excluido
				x.Oid, x.Fovial, 0.0 /* falta cotrans*/, x.IvaPercibido, x.IvaRetenido
		from CompraFactura x
		inner join Tercero t
			on x.Proveedor = t.Oid
		left join TerceroDocumento td
			on td.Tercero = x.Proveedor
			and td.Tipo = 'NIT'
		left join TerceroDireccion d
			on t.DireccionPrincipal = d.Oid
		where x.Empresa = @Empresa 
			and Month(x.Fecha) = @Mes 
			and Year(x.Fecha) = @Anio 
			and x.TipoFactura in ('COVE01', 'COVE03', 'COVE06', 'COVE12', 'COVE13')
	end
	else
	begin
		merge LibroCompra as destino
		using
			(select @CorrInicio + row_number() over(partition by x.Empresa, Year(x.Fecha) order by x.Fecha), x.Fecha, x.Clase, 
						iif(x.TipoFactura = 'COVE01', '03', iif(x.TipoFactura = 'COVE12', '12', iif(x.TipoFactura = 'COVE13', '13', null))), 
						format(cast(ltrim(rtrim(x.NumeroFactura)) as int), '00000000'), td.Numero, x.Proveedor, 
						iif(x.TipoFactura = 'COVE01', x.Exenta, 0.0), 
						iif(x.TipoFactura in ('COVE12', 'COVE13') and d.Pais in ('CRI', 'GTM', 'HND', 'NIC'), x.Exenta, 0.0), -- internacion exenta
						iif(x.TipoFactura in ('COVE12', 'COVE13'), x.Exenta, 0.0), -- import exenta 
						iif(x.TipoFactura = 'COVE01', x.Gravada, 0.0), 
						-- Internacion Gravada bien
						iif(x.TipoFactura in ('COVE12', 'COVE13') and x.Tipo <> 0 and d.Pais in ('CRI', 'GTM', 'HND', 'NIC'), x.Gravada, 0.0),  
						iif(x.TipoFactura in ('COVE12', 'COVE13') and x.Tipo <> 0, x.Gravada, 0.0), -- importacion gravada bien 
						iif(x.TipoFactura in ('COVE12', 'COVE13') and x.Tipo = 0, x.Gravada, 0.0), -- importacion gravada servicio
						x.Iva, iif(x.TipoFactura = 'COVE06', x.Gravada, 0.0), -- compra excluido
						x.Oid, x.Fovial, 0.0 /* falta cotrans*/, x.IvaPercibido, x.IvaRetenido
				from CompraFactura x
				inner join Tercero t
					on x.Proveedor = t.Oid
				left join TerceroDocumento td
					on td.Tercero = x.Proveedor
					and td.Tipo = 'NIT'
				left join TerceroDireccion d
					on t.DireccionPrincipal = d.Oid
				where x.Empresa = @Empresa 
					and Month(x.Fecha) = @Mes 
					and Year(x.Fecha) = @Anio 
					and x.TipoFactura in ('COVE01', 'COVE03', 'COVE06', 'COVE12', 'COVE13')
			) as origen
			(Correlat, Fecha, Clase, TipoDoc, NumeroFactura, Nit, Proveedor,  InternaExenta, InternacionExenta, ImportExenta, Gravada,
				InternacionGravadaBien, ImportacionGravadaBien, ImportacionGravadaServicio, Iva, CompraExcluido, FacturaOid,
				Fovial, Cotrans, IvaPercibido, IvaRetenido)
			on (destino.CompraFactura = Origen.FacturaOid
		and  destino.TipoDocumento = origen.TipoDoc)
		when matched then
			update set
			    Correlativo = origen.Correlat,
				Fecha = origen.Fecha,
				ClaseDocumento = origen.Clase,
				TipoDocumento = origen.TipoDoc,
				Numero = origen.NumeroFactura,
				Nit = origen.Nit,
				Proveedor = origen.Proveedor,
				InternaExenta = origen.InternaExenta,
				InternacionExenta = origen.InternacionExenta,
				ImportacionExenta = origen.ImportExenta,
				InternaGravada = origen.Gravada,
				InternacionGravadaBien = origen.InternacionGravadaBien,
				ImportacionGravadaBien = origen.ImportacionGravadaBien,
				ImportacionGravadaServicio = origen.ImportacionGravadaServicio,
				CreditoFiscal = origen.Iva,
				CompraExcluido = origen.CompraExcluido,
				Fovial = origen.Fovial,
				-- falta Cotrans
				IvaPercibido = origen.IvaPercibido,
				IvaRetenido = origen.IvaRetenido
		when not matched by target then
		insert (Correlativo, Fecha, ClaseDocumento, TipoDocumento, Numero, Nit, Proveedor, InternaExenta, InternacionExenta, ImportacionExenta,
				InternaGravada, InternacionGravadaBien, ImportacionGravadaBien, ImportacionGravadaServicio, CreditoFiscal, 
				CompraExcluido, CompraFactura, Fovial, Cotrans, IvaPercibido, IvaRetenido)
		values (Correlat, origen.Fecha, origen.Clase, origen.TipoDoc, origen.NumeroFactura, origen.Nit, origen.Proveedor, origen.InternaExenta,
				origen.InternacionExenta, origen.ImportExenta, origen.Gravada, origen.InternacionGravadaBien, 
				origen.ImportacionGravadaBien, origen.ImportacionGravadaServicio, origen.Iva, origen.CompraExcluido, origen.FacturaOid,
				origen.Fovial, 0.0 /* falta cotrans */, origen.IvaPercibido, origen.IvaRetenido)
		--when not matched by source and destino.TipoDocumento in ('03', '11', '12', '13') 
		--                           and Year(destino.Fecha) = @Anio and Month(destino.Fecha) = @Mes then
		--  delete    -- se comenta porque borra los registros de compras que no son del mes y posiblemente los de otras empresas
		;
		-- lo anterior se resuelve de esta manera
		delete l
		from LibroCompra l		  
		where not exists (select Oid from CompraFactura c where c.Oid = l.CompraFactura)
		                  
	end

	-- se busca actualizar la columna DUI cuando los NIT son nulos
	update l
		set l.DUI = t.Numero
		from LibroCompra l
		inner join CompraFactura f
		on l.CompraFactura = f.Oid
		inner join TerceroDocumento t
		on t.Tercero = l.Proveedor
		and t.Tipo = 'DUI'
		where f.Empresa = @Empresa 
		and month(f.Fecha) = @Mes 
		and year(f.Fecha) = @Anio 
		and l.Nit is null
	-- faltan las notas de debito y credito
	--select count(*) from CxPTransaccion

end