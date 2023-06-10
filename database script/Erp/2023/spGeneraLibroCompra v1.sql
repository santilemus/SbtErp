SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- ================================================================================================
-- Author:		Santiago Enrique Lemus
-- Create date: 31/05/2023
-- Description:	Generar Libro de Compras - IVA
-- ================================================================================================
create or alter procedure [dbo].[spGeneraLibroCompra](
   @Empresa     int,
   @Moneda      varchar(3),
   @Mes         int,
   @Anio        int)
as
begin
  set nocount on
  declare @error_msg nvarchar(4000)
  declare @error_sev int;

  
  declare @factorCambio numeric(12,2);
  select @factorCambio = m.FactorCambio from Moneda m
   where m.Codigo = @Moneda

  --- procesamos, la partida de apertura (si se encuentra en @fecha_desde)
  begin try
    begin tran t1
		select count(*) from LibroCompra x
		 inner join CompraFactura y
			on x.CompraFactura = y.Oid
		 where y.Empresa = @Empresa 
		   and Month(x.Fecha) = @Mes
		   and Year(x.Fecha) = @Anio
		if (@@rowcount = 0)
		begin
		  -- pendiente de resolver el tema de las compras de bienes y servicios en el territorio nacional
		  -- de usuarios de zonas francas o dpa (TipoFactura = 'COVE03'). Este caso aplicara cuando la empresa
		  -- de la sesion sea un usuario de zona franca o una dpa
		  insert into LibroCompra
				 (Fecha, ClaseDocumento, TipoDocumento, Numero, Nit, Proveedor, InternaExenta, InternacionExenta,
				  ImportacionExenta, InternaGravada, InternacionGravadaBien, ImportacionGravadaBien, 
				  ImportacionGravadaServicio, CreditoFiscal, CompraExcluido, CompraFactura)
		  select x.Fecha, x.Clase, iif(x.TipoFactura = 'COVE12', '12', iif(x.TipoFactura = 'COVE13', '13', '03')), 
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
				 x.Oid
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
				(select x.Fecha, x.Clase, 
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
						 x.Oid
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
				(Fecha, Clase, TipoDoc, NumeroFactura, Nit, Proveedor,  InternaExenta, InternacionExenta, ImportExenta, Gravada,
				 InternacionGravadaBien, ImportacionGravadaBien, ImportacionGravadaServicio, Iva, CompraExcluido, FacturaOid)
			 on (destino.CompraFactura = Origen.FacturaOid
			and  destino.TipoDocumento = origen.TipoDoc)
		  when matched then
			 update set
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
				 CompraExcluido = origen.CompraExcluido
		  when not matched by target then
			insert (Fecha, ClaseDocumento, TipoDocumento, Numero, Nit, Proveedor, InternaExenta, InternacionExenta, ImportacionExenta,
					InternaGravada, InternacionGravadaBien, ImportacionGravadaBien, ImportacionGravadaServicio, CreditoFiscal, 
					CompraExcluido, CompraFactura)
			values (origen.Fecha, origen.Clase, origen.TipoDoc, origen.NumeroFactura, origen.Nit, origen.Proveedor, origen.InternaExenta,
					origen.InternacionExenta, origen.ImportExenta, origen.Gravada, origen.InternacionGravadaBien, 
					origen.ImportacionGravadaBien, origen.ImportacionGravadaServicio, origen.Iva, origen.CompraExcluido, origen.FacturaOid)
		  --when not matched by source and destino.TipoDocumento in ('03', '11', '12', '13') 
		  --                           and Year(destino.Fecha) = @Anio and Month(destino.Fecha) = @Mes then
		  --  delete    -- se comenta porque borra los registros de compras que no son del mes y posiblemente los de otras empresas
			;
		  -- lo anterior se resuelve de esta manera
		  delete l
		    from LibroCompra l		  
		   where not exists (select Oid from CompraFactura c where c.Oid = l.CompraFactura)
		                  
		end
		select count(*) from CxPTransaccion
	commit tran t1
  end try
  begin catch
    rollback tran t1
    select @error_msg = error_message(), @error_sev = error_severity()
    raiserror(@error_msg, @error_sev, 1)
  end catch 
end