use Sbt_Erp
go
IF EXISTS(SELECT * FROM Information_schema.Routines 
           WHERE Specific_schema = 'dbo' AND SPECIFIC_NAME = 'fnPlaOperacionAcumuladaEx' AND Routine_Type = 'FUNCTION')
begin
  drop function fnPlaOperacionAcumuladaEx;
end
go

/****** Object:  UserDefinedFunction [dbo].[plaOperacionAcumuladaEx]    Script Date: 15/7/2020 00:00:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/***************************************************************************************
Creado Por     : Santiago Enrirque Lemus
Fecha Creacion : 15/07/2020
Prop�sito      : Retornar el total acumulado de una operaci�n de planillas para el per�odo
                 y empleado del par�metro. Difiere de las otras versiones porque esta recupera
				 el monto de la operacion para un tipo de planilla especifico
Par�metros
  @OidEmpleado : C�digo del empleado
  @FechaInicio : Fecha de inicio en la cual se obtendr� el acumulado de la operaci�n
  @FechaFin    : Fecha de finalizaci�n en la cual se obrendr� el acumulado de la operaci�n
  @ClasePlanilla: La clase de planilla para la cual se recupera la operacion
  @OidOperacion: C�digo de la operaci�n
Retorna        : El total acumulado de la operaci�n que cumple con los par�metros de entrada
****************************************************************************************/
create function [dbo].[fnPlaOperacionAcumuladaEx](
     @OidEmpleado   int,
     @FechaInicio   datetime2,
     @FechaFin      datetime2,
	 @ClasePlanilla int,
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
      on d.Planilla    = e.Oid 
   inner join TipoPlanilla t
      on e.Tipo = t.Oid 
   where d.Empleado = @OidEmpleado
     and e.FechaPago between cast(@FechaInicio as date) and dbo.fnEndDay('mcs', @FechaFin)
	 and t.Clase       =   @ClasePlanilla
     and dop.Operacion = @OidOperacion
  return coalesce(@monto, 0);
end
GO
grant execute on dbo.fnPlaOperacionAcumuladaEx to public
go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Retornar el total acumulado de una operaci�n de planillas para el per�odo
 y empleado del par�metro. Difiere de las otras versiones porque esta recupera el monto de la operacion para un tipo de planilla especifico' , 
 @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'fnPlaOperacionAcumuladaEx'
GO


