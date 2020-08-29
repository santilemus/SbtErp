use Sbt_Erp
go
IF EXISTS(SELECT * FROM Information_schema.Routines 
           WHERE Specific_schema = 'dbo' AND SPECIFIC_NAME = 'fnPlaIngresoGravado' AND Routine_Type = 'FUNCTION')
begin
  drop function fnPlaIngresoGravado;
end
go

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/***************************************************************************************
Creado Por     : Santiago Enrirque Lemus
Fecha Creación : 22/agosto/2020
Propósito      : Retornar el ingreso acumulado sujeto de calculo de renta, restando las 
                 cotizaciones del seguro social y pensiones
Parámetros
  @OidEmpleado : Id del empleado
  @FechaInicio : Fecha de inicio en la cual se obtendrá el acumulado de la operación
  @FechaFin    : Fecha de finalización en la cual se obrendrá el acumulado de la operación
Retorna        : El total acumulado de la operación que cumple con los parámetros de entrada
****************************************************************************************/
CREATE function [dbo].[fnPlaIngresoGravado](
     @OidEmpleado   int,
     @FechaInicio   datetime,
     @FechaFin      datetime,
	 @Moneda        varchar(3)
)
returns numeric(14, 2)
as
begin
  declare @monto numeric(14,2) = 0
  declare @cotizaciones numeric(14, 2)
  declare @ingresoAnterior numeric(14,2)
  declare @ValorMoneda numeric(14,2)
  
  select @ValorMoneda = FactorCambio from Moneda
   where Codigo = @Moneda
  select @monto = sum(coalesce(dop.valor, 0) / e.ValorMoneda * @ValorMoneda) 
    from PlanillaDetalleOperacion dop
   inner join PlanillaDetalle d
      on dop.PlanillaDetalle = d.Oid
   inner join Planilla e
      on d.Planilla  = e.Oid
   where d.Empleado  = @OidEmpleado
     and e.FechaPago between cast(@FechaInicio as date) and dbo.fnEndDay('mcs', @FechaFin)
	 and dop.Operacion in (6, 12, 17) -- Suma: 6 = Aguinaldo, 12 = IngresoGravadoRenta, 17 = IngresoBruto   
  -- ahora calculamos las cotizaciones
  select @cotizaciones = sum(coalesce(dop.valor, 0)/ e.ValorMoneda * @ValorMoneda) 
    from PlanillaDetalleOperacion dop
   inner join PlanillaDetalle d
      on dop.PlanillaDetalle = d.Oid
   inner join Planilla e
      on d.Planilla = e.Oid
   inner join Empresa emp
      on e.Empresa = emp.Oid 
   where d.Empleado = @OidEmpleado
     and e.FechaPago between cast(@FechaInicio as date) and dbo.fnEndDay('mcs', @FechaFin)
	 and dop.Operacion in (18, 19)    -- Suma: 18 = cotizacion seguro social, 12 = cotizacion pension
	 and emp.Pais = 'SLV'             -- para restar las cotizaciones solo en el caso de ES
  -- ahora obtenemos los ingresos gravados de empleos anteriores, solo cuando se calcula el acumulado del anio
  if (Month(@FechaInicio) = 1 And Day(@FechaInicio) = 1)
    select @ingresoAnterior = sum(coalesce(r.IngresoGravado, 0)/ r.ValorMoneda * @ValorMoneda)
      from RentaEmpleoAnterior r
     where r.Empleado = @OidEmpleado
       and r.FechaHasta between cast(@FechaInicio as date) and dbo.fnEndDay('mcs', @FechaFin)
  return coalesce(@monto, 0) + coalesce(@ingresoAnterior, 0) - coalesce(@cotizaciones, 0);
end
GO
grant execute on fnPlaOperacionAcumulada to public
go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Retornar el ingreso acumulado sujeto de calculo de renta. Se restan las 
cotizaciones del seguro social y pensiones' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'fnPlaIngresoGravado'
GO