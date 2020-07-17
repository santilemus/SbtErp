use Sbt_Erp
go
IF EXISTS(SELECT * FROM Information_schema.Routines 
           WHERE Specific_schema = 'dbo' AND SPECIFIC_NAME = 'fnPlaOperacionAcumulada2' AND Routine_Type = 'FUNCTION')
begin
  drop function fnPlaOperacionAcumulada2;
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
                 y empleado del par�metro, excluyendo la operaci�n de las planillas de 
                 bonificacion, aguinaldo, dietas
Par�metros
  @OidEmpleado : C�digo del empleado
  @FechaInicio : Fecha de inicio en la cual se obtendr� el acumulado de la operaci�n
  @FechaFin    : Fecha de finalizaci�n en la cual se obrendr� el acumulado de la operaci�n
  @OidOperacion: C�digo de la operaci�n
Retorna        : El total acumulado de la operaci�n que cumple con los par�metros de entrada
****************************************************************************************/
CREATE function [dbo].[fnPlaOperacionAcumulada2](
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
      on d.Planilla  = e.Oid 
   inner join TipoPlanilla t
      on e.Tipo = t.Oid          
   where d.Empleado       = @OidEmpleado
     and e.FechaPago between cast(@FechaInicio as date) and dbo.fnEndDay('mcs', @FechaFin)
     and dop.Operacion    = @OidOperacion
     and t.Clase not in (2, 3, 8)   -- bonificacion, aguinaldo, dietas
  return coalesce(@monto, 0);
end

go
grant execute on dbo.fnPlaOperacionAcumulada2 to public
go

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Retornar el total acumulado de una operaci�n de planillas para el per�odo y empleado 
del par�metro, excluyendo la operaci�n de las planillas de gratificaci�n, aguinaldo, 
bono, dieta; porque son excepcionales' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'fnPlaOperacionAcumulada2'
GO


