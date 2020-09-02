use Sbt_Erp
go
IF EXISTS(SELECT * FROM Information_schema.Routines 
           WHERE Specific_schema = 'dbo' AND SPECIFIC_NAME = 'fnPlaOperacionAcumulada' AND Routine_Type = 'FUNCTION')
begin
  drop function fnPlaOperacionAcumulada;
end
go

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/***************************************************************************************
Creado Por     : Santiago Enrirque Lemus
Fecha Creación : 14/julio/2020
Propósito      : Retornar el total acumulado de una operación de planillas para el período
                 y empleado del parámetro
Parámetros
  @OidEmpleado : Id del empleado
  @FechaInicio : Fecha de inicio en la cual se obtendrá el acumulado de la operación
  @FechaFin    : Fecha de finalización en la cual se obrendrá el acumulado de la operación
  @IdOPeracion : Código de la operación
Retorna        : El total acumulado de la operación que cumple con los parámetros de entrada
****************************************************************************************/
CREATE function [dbo].[fnPlaOperacionAcumulada](
     @OidEmpleado   int,
     @FechaInicio   datetime,
     @FechaFin      datetime,
     @OidOperacion  int
)
returns money
as
begin
  declare @monto money = 0
  select @monto = sum(dop.valor) 
    from PlanillaDetalleOperacion dop
   inner join PlanillaDetalle d
      on dop.PlanillaDetalle = d.Oid
   inner join Planilla e
      on d.Planilla       = e.Oid
   where d.Empleado       = @OidEmpleado
     and e.FechaPago between cast(@FechaInicio as date) and dbo.fnEndDay('mcs', @FechaFin)
     and dop.Operacion    = @OidOperacion
  return coalesce(@monto, 0);
end
GO
grant execute on fnPlaOperacionAcumulada to public
go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Retornar el total acumulado de una operación de planillas para el período y empleado 
del parámetro' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'fnPlaOperacionAcumulada'
GO


