USE [SbtErp]
GO
/****** Object:  StoredProcedure [dbo].[spVentaResumenMes]    Script Date: 19/8/2024 21:20:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- ================================================================================================
-- Author:		Santiago Enrique Lemus
-- Create date: 31/05/2023
-- Description:	Generar Resumen de Ventas por Mes para cuadre de IVA
-- Modificaciones
-- ================================================================================================
ALTER   procedure [dbo].[spVentaResumenMes] 
(
	@Empresa int,
	@Moneda varchar(3),
	@FechaDesde DateTime,
	@FechaHasta DateTime
)
AS
begin
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
		on m.Codigo = @Moneda
	 where v.Empresa = @Empresa
	   and v.Estado <> 2 -- anulado 
	   and cast(v.Fecha as Date) between @FechaDesde and @FechaHasta
	   and v.GCRecord is null
	 group by Format(v.Fecha, 'MMMM-yyyy'), v.TipoFactura, l.Nombre, m.Plural
end
