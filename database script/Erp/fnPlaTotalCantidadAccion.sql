use Sbt_Erp
go
IF EXISTS(SELECT * FROM Information_schema.Routines 
           WHERE Specific_schema = 'dbo' AND SPECIFIC_NAME = 'fnPlaTotalCantidadAccion' AND Routine_Type = 'FUNCTION')
begin
  drop function fnPlaTotalCantidadAccion;
end
go

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/***************************************************************************************
Creado Por     : Santiago Enrirque Lemus
Fecha Creaci�n : 16/07/2020
Prop�sito      : Retornar el acumulado de d�as por Tipo de Acci�n en un per�odo
Par�metros
  @OidEmpleado : C�digo del empleado que se quiere conocer su acumulado de acciones,
  @TipoAccion  : Tipo de la acci�n que se quiere obtener su acumulado de d�as
  @FechaInicio : Fecha de inicio del per�odo a incluir en el c�lculo de los d�as
  @FechaFin    : Fecha de finalizaci�n del per�odo a incluir en el c�lculo de d�as
Retorna        : El acumulado de la columna monto por tipo de acci�n del par�metro
Comentarios

****************************************************************************************/
CREATE function [dbo].[fnPlaTotalCantidadAccion](
     @OidEmpleado int,
     @TipoAccion  smallint,
     @FechaInicio datetime,
     @FechaFin    datetime
)
returns money
as
begin
  declare @total money = 0
  select @total = sum(coalesce(monto, 0)) 
    from AccionPersonal 
   where Empleado = @OidEmpleado
     and FechaAccion between @FechaInicio and @FechaFin
     and Tipo   = @TipoAccion
     and Estado = 1
  return coalesce(@total, 0.0)
end
GO
grant execute on fnPlaTotalCantidadAccion to public
go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Retornar el acumulado de d�as por Tipo de Acci�n en un per�odo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'fnPlaTotalCantidadAccion'
GO

