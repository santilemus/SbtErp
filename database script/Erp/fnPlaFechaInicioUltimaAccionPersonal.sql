use Sbt_Erp
go
IF EXISTS(SELECT * FROM Information_schema.Routines 
           WHERE Specific_schema = 'dbo' AND SPECIFIC_NAME = 'fnPlaFechaInicioUltimaAccionPersonal' AND Routine_Type = 'FUNCTION')
begin
  drop function fnPlaFechaInicioUltimaAccionPersonal;
end
go

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/***************************************************************************************
Creado Por     : Santiago Enrirque Lemus
Fecha Creación : 13/07/2020
Propósito      : Retornar la fecha inicial de la última accion de personal del tipo 
                 especificado en el parámetro
Parámetros
  @OidEmpleado : Código del empleado que se quiere conocer su acumulado de acciones,
  @TipoAccion  : Tipo de la acción que se quiere obtener su acumulado de días
Retorna        : La fecha inicial de la última accion de personal, del tipo especi-
                 ficado en el parámetro
Comentarios   
****************************************************************************************/
CREATE function [dbo].[fnPlaFechaInicioUltimaAccionPersonal](
     @OidEmpleado int,
     @FechaInicio datetime,
     @TipoAccion  smallint
)
returns datetime
as
begin
  declare @fecha datetime
  select @fecha = max(FechaInicio) 
    from AccionPersonal
   where Empleado = @OidEmpleado
     and Tipo     = @TipoAccion
     and FechaFin >= cast(@FechaInicio as Date)
     and estado = 1 -- Aprobada
  return @fecha
end
GO
grant execute on dbo.fnPlaFechaInicioUltimaAccionPersonal to public
go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Retornar la fecha inicial de la última accion de personal del tipo  especificado 
en el parámetro' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'fnPlaFechaInicioUltimaAccionPersonal'
GO


