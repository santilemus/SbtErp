use Sbt_Erp
go
IF EXISTS(SELECT * FROM Information_schema.Routines 
           WHERE Specific_schema = 'dbo' AND SPECIFIC_NAME = 'fnPlaAcumuladoDiasAccion' AND Routine_Type = 'FUNCTION')
begin
  drop function fnPlaAcumuladoDiasAccion;
end
go

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/***************************************************************************************
Creado Por     : Santiago Enrirque Lemus
Prop�sito      : Retornar el acumulado de d�as por Tipo de Acci�n en un per�odo
Par�metros
  @OidEmpleado : Oid del empleado que se quiere conocer su acumulado de acciones,
  @TipoAccion  : Tipo de la acci�n que se quiere obtener su acumulado de d�as
  @FechaInicio : Fecha de inicio del per�odo a incluir en el c�lculo de los d�as
  @FechaFin    : Fecha de finalizaci�n del per�odo a incluir en el c�lculo de d�as
Retorna        : El acumulado de los d�as para un tipo de acci�n particular
****************************************************************************************/
CREATE function [dbo].[fnPlaAcumuladoDiasAccion](
     @OidEmpleado int,
     @TipoAccion  smallint,
     @FechaInicio datetime,
     @FechaFin    datetime
) 
returns integer
as
begin
  declare @tramo1 int
  declare @tramo2 int
  declare @tramo3 int
  declare @tramo4 int
  -- 1ero: Obtenemos la cantidad de d�as para acciones que se encuentran completamente dentro del per�odo
  select @tramo1 = sum(coalesce(datediff(day, cast(FechaInicio as date), dbo.fnEndDay('ns', FechaFin)), 0)) 
    from AccionPersonal
   where Empleado = @OidEmpleado
     and Tipo     = @TipoAccion
     and FechaInicio >= cast(@FechaInicio as date)
	 and FechaFin is not null
     and FechaFin    <= dbo.fnEndDay('ns', @FechaFin)
     and Estado      = 1  -- aprobada
  -- 2do: obtenemos la cantidad de d�as para acciones que comenzaron antes de @FechaInicio y que terminan
  --      antes de @FechaFin, por lo tanto solo se consideran parcialmente los dias que corresponden
  -- se asume que solo hay una accion porque no pueden haber dos simultaneas
  select @tramo2 = sum(datediff(day, cast(@FechaInicio as DATE), dbo.fnEndDay('ns', FechaFin)))
    from AccionPersonal
   where Empleado = @OidEmpleado
     and Tipo     = @TipoAccion
     and FechaInicio < @FechaInicio
     and FechaFin between cast(@FechaInicio as date) and dbo.fnEndDay('ns', @FechaFin) 
     and estado = 1 -- aprobada  
  -- 3ro: obtenemos la cantidad de d�as para acciones que comenzaron al menos en @FechaInicio y que
  --      terminan despues de @FechaFin, por lo tanto solo se consideran parcialmente los dias que corresonde
  -- se asume que solo hay una porque no pueden haber dos simultaneas
  select @tramo3 = sum(datediff(day, cast(FechaInicio as date), dbo.fnEndDay('ns', @FechaFin)))
    from AccionPersonal
   where Empleado = @OidEmpleado
     and Tipo     = @TipoAccion
     and FechaInicio between cast(@FechaInicio as date) and dbo.fnEndDay('ns', @FechaFin)
     and FechaFin > dbo.fnEndDay('ns', @FechaFin)
     and estado = 1 -- aprobada
     
  -- el caso cuando la acci�n trasciende el per�odo ingresado, es decir comienza antes de fecha inicio
  -- y finaliza despues de fecha fin
  select @tramo4 = sum(datediff(day, cast(@FechaInicio as date), dbo.fnEndDay('ns', @FechaFin))) 
    from AccionPersonal
   where Empleado = @OidEmpleado
     and Tipo     = @TipoAccion
     and FechaInicio < @FechaInicio
     and FechaFin > dbo.fnEndDay('ns', @FechaFin) 
     and estado = 1 -- aprobada 
  -- es probable que nos falte un tramo adicional, que corresponde a los que estan con permiso
  -- sin goce de sueldo por un per�odo largo que trasciende @fecha_inicio and @fecha_fin
  return coalesce(@tramo1, 0) + coalesce(@tramo2, 0) + coalesce(@tramo3, 0) + coalesce(@tramo4, 0)
end
GO
grant execute on dbo.fnPlaAcumuladoDiasAccion to public
go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Retornar el acumulado de d�as por Tipo de Acci�n en un per�odo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'fnPlaAcumuladoDiasAccion'
GO

