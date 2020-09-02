use Sbt_Erp
go
IF EXISTS(SELECT * FROM Information_schema.Routines 
           WHERE Specific_schema = 'dbo' AND SPECIFIC_NAME = 'fnPlaFechaUltimaVacacion' AND Routine_Type = 'FUNCTION')
begin
  drop function fnPlaFechaUltimaVacacion;
end
go


/****** Object:  UserDefinedFunction [dbo].[plaFechaUltimaVacacion]    Script Date: 13/7/2020 00:36:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/***************************************************************************************
Creado Por     : Santiago Enrirque Lemus
Fecha Creaci�n : 13/julio/2020
Prop�sito      : Retornar la fecha de la �ltima vacacion de un empleado que ser� liquidado
Par�metros
  @OidEmpleado : C�digo del empleado que se quiere conocer su acumulado de acciones,
  @TipoAccion  : Tipo de la acci�n que se quiere obtener su acumulado de d�as
Retorna        : La fecha de la �ltima vacaci�n que se pago al empleado a liquidaro
Comentarios   

****************************************************************************************/
CREATE function [dbo].[fnPlaFechaUltimaVacacion](
     @OidEmpleado int,
     @FechaFin    datetime
)
returns datetime
as
begin
  declare @fecha     datetime
  declare @fechaCumple datetime
  select @fechaCumple = dateadd(year, year(@FechaFin) - year(p.FechaIngreso), p.FechaIngreso)
    from Empleado e
   inner join Persona p
      on e.Oid = p.Oid 
   where e.Oid = @OidEmpleado 
     
  select @fecha = max(e.FechaInicio)
    from PlanillaDetalleOperacion dop
   inner join PlanillaDetalle d
      on dop.PlanillaDetalle = d.Oid
   inner join Planilla e
      on d.Planilla = e.Oid 
   inner join TipoPlanilla t
      on e.Tipo     = t.Oid
   where d.Empleado = @OidEmpleado
     and t.Clase    =  4 -- vacaci�n
     and e.FechaFin <= dbo.fnEndDay('mcs', @FechaFin)
  if @fecha is null
    set @fecha = @fechaCumple -- dateadd(year, -1, @fechaCumple)
  else
    set @fecha = dateadd(day, day(@fechaCumple) - 1, @fecha) 
  return @fecha
end

GO
grant execute on dbo.fnPlaFechaUltimaVacacion to public
go

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Retornar la fecha de la �ltima vacacion de un empleado que ser� liquidado' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'fnPlaFechaUltimaVacacion'
GO


