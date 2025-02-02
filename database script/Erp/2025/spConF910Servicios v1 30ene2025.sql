USE [SbtErp]
GO
/****** Object:  StoredProcedure [dbo].[spGenerarAnexoPagoCuenta]    Script Date: 30/1/2025 16:15:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- ================================================================================================
-- Author:		Santiago Enrique Lemus
-- Create date: 30/01/2024
-- Description:	Retornar el consolidado de ingresos gravados y las retenciones de renta, realizados
--              a proveedores de servicios, para efectos de cuadre y verificacion del informe F910
-- Modificaciones
-- ================================================================================================
create or alter procedure spConF910Servicios
   @Empresa int,
   @Moneda  varchar(3),
   @Anio    int
as
begin
  set nocount on;
  declare @FactorCambio numeric(12,4) = 1.0
  declare @Plural varchar(25) = ''
  select @FactorCambio = FactorCambio, @Plural = Plural from Moneda
   where Codigo = @Moneda 
  select c.Proveedor, t.Nombre, sum(coalesce(Gravada, 0.0) / coalesce(c.ValorMoneda, 1.0)) * @FactorCambio as MontoGravado, 
         sum(coalesce(renta, 0.0) / coalesce(c.ValorMoneda, 1.0)) * @FactorCambio as Renta,
         replace(trim(d.Numero), '-', '') as Dui, replace(trim(n.Numero), '-', '') as Nit,
	     11 as CodigoIngreso, @Plural as Plural
    from CompraFactura c
   inner join Tercero t
      on c.Proveedor = t.Oid 
    left join TerceroDocumento d
      on d.Tercero = t.Oid 
     and d.Tipo = 'DUI'
    left join TerceroDocumento n
      on n.Tercero = t.Oid
     and n.Tipo = 'NIT'
   where c.Empresa = @Empresa
     and Year(c.Fecha) = @Anio
     and c.Renta > 0.0
     and c.Estado < 2
     and c.GCRecord is null
     and t.TipoPersona = 1
   group by c.Proveedor, t.Nombre, d.Numero, n.Numero
end
go
grant execute on spConF910Servicios to public
go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Retornar el consolidado de ingresos gravados y las retenciones de renta, realizados
 a proveedores de servicios, para efectos de cuadre y verificacion del informe F910' , 
 @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'spConF910Servicios'
GO


select * from Moneda
 where Activa = 1