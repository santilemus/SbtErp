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
Fecha Creación : 16/07/2020
Propósito      : Retornar el acumulado de días por Tipo de Acción en un período
Parámetros
  @OidEmpleado : Código del empleado que se quiere conocer su acumulado de acciones,
  @TipoAccion  : Tipo de la acción que se quiere obtener su acumulado de días
  @FechaInicio : Fecha de inicio del período a incluir en el cálculo de los días
  @FechaFin    : Fecha de finalización del período a incluir en el cálculo de días
Retorna        : El acumulado de la columna monto por tipo de acción del parámetro
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
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Retornar el acumulado de días por Tipo de Acción en un período' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'fnPlaTotalCantidadAccion'
GO

