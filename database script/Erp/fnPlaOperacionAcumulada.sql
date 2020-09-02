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
Fecha Creaci�n : 14/julio/2020
Prop�sito      : Retornar el total acumulado de una operaci�n de planillas para el per�odo
                 y empleado del par�metro
Par�metros
  @OidEmpleado : Id del empleado
  @FechaInicio : Fecha de inicio en la cual se obtendr� el acumulado de la operaci�n
  @FechaFin    : Fecha de finalizaci�n en la cual se obrendr� el acumulado de la operaci�n
  @IdOPeracion : C�digo de la operaci�n
Retorna        : El total acumulado de la operaci�n que cumple con los par�metros de entrada
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
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Retornar el total acumulado de una operaci�n de planillas para el per�odo y empleado 
del par�metro' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'fnPlaOperacionAcumulada'
GO


