use Sbt_Erp
go
IF EXISTS(SELECT * FROM Information_schema.Routines 
           WHERE Specific_schema = 'dbo' AND SPECIFIC_NAME = 'fnPlaOperacionPlanilla' AND Routine_Type = 'FUNCTION')
begin
  drop function fnPlaOperacionPlanilla;
end
go
/****** Object:  UserDefinedFunction [dbo].[plaOperacionPlanilla]    Script Date: 15/7/2020 00:13:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/***************************************************************************************
Creado Por     : Santiago Enrirque Lemus
Fecha Creacion : 15/07/2020
Prop�sito      : Retornar el total acumulado de una operaci�n de planillas para el per�odo
                 y empleado del par�metro
Par�metros
  @OidEmpleado : C�digo del empleado
  @OidPlanilla : Numero de planilla
  @OidOperacion: C�digo de la operaci�n
Retorna        : El total acumulado de la operaci�n que cumple con los par�metros de entrada
****************************************************************************************/
CREATE function [dbo].[fnPlaOperacionPlanilla](
     @OidEmpleado  int,
	 @OidPlanilla  int,
     @OidOperacion int
)
returns money
as
begin
  declare @monto money = 0
  select @monto = coalesce(dop.valor, 0) 
    from PlanillaDetalleOperacion dop
   inner join PlanillaDetalle d
      on dop.PlanillaDetalle = d.Oid
   where d.Empleado    = @OidEmpleado
     and d.Planilla    = @OidPlanilla
     and dop.Operacion = @OidOperacion
  return coalesce(@monto, 0);
end
GO
grant execute on dbo.fnPlaOperacionPlanilla to public
go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Retornar el total acumulado de una operaci�n de planillas para el per�odo
 y empleado del par�metro' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'fnPlaOperacionPlanilla'
GO


