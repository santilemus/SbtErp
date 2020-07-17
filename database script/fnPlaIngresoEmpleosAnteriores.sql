use Sbt_Erp
go
IF EXISTS(SELECT * FROM Information_schema.Routines 
           WHERE Specific_schema = 'dbo' AND SPECIFIC_NAME = 'fnPlaIngresoEmpleosAnteriores' AND Routine_Type = 'FUNCTION')
begin
  drop function fnPlaIngresoEmpleosAnteriores;
end
go

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/***************************************************************************************
Creado Por     : Santiago Enrirque Lemus
Fecha Creación : 16/07/2020
Propósito      : Retornar el acumulado de los ingresos gravados en los empleos previos
Parámetros
  @OidEmpleado  : Código del empleado a quien corresponden los ingresos gravados
  @FechaFin     : Fecha Fin del periodo al cual se quiere obtener los ingresos de empleos previos
Retorna         : Retornar el acumulado de los ingresos gravados en los empleos previos
Comentarios     : 
****************************************************************************************/
create function fnPlaIngresoEmpleosAnteriores(
  @OidEmpleado  int,
  @FechaFin     datetime)
returns numeric(14,2)
as
begin
  declare @monto numeric(14,2)
  select @monto = sum(coalesce(IngresoGravado, 0)) from RentaEmpleoAnterior 
   where Empleado = @OidEmpleado 
     and FechaHasta between DateFromParts(year(@FechaFin), 1, 1) and dbo.fnEndDay('mcs', @FechaFin)
  return coalesce(@monto, 0);  
end
go
grant execute on fnPlaIngresoEmpleosAnteriores to public
go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Retornar el acumulado de los ingresos gravados en los empleos previos' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'fnPlaIngresoEmpleosAnteriores'
GO