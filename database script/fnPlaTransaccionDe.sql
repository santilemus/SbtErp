use Sbt_Erp
go
IF EXISTS(SELECT * FROM Information_schema.Routines 
           WHERE Specific_schema = 'dbo' AND SPECIFIC_NAME = 'fnPlaTransaccionDe' AND Routine_Type = 'FUNCTION')
begin
  drop function fnPlaTransaccionDe;
end
go

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/***************************************************************************************
Creado Por     : Santiago Enrirque Lemus
Fecha Creación : 16/07/2020
Propósito      : Retornar el acumulado de transacciones por clasificacion a descontar en 
                 la planilla
Parámetros
  @OidEmpleado  : Código del empleado a quien corresponden las transacciones,
  @FechaFin     : Fecha fin de la quincena o periodo de calculo de la planilla. Sirve
                 para validar que solo las transacciones con fecha inicio antes al fin de la
				 quincena se consideren por la funcion
  @Clasificacion: La clasificacion de las transacciones a incluir en el calculo de la funcion
  @NoQuincena   : No de quincena en la cual se descuentan las transacciones. 3 es ambas quincenas
Retorna         : El monto total de las transacciones por clasificacion a descontar en la planilla
Comentarios     : 
****************************************************************************************/
create function fnPlaTransaccionDe(
   @OidEmpleado   int, 
   @FechaFin      datetime2,
   @Clasificacion varchar(12),
   @NoQuincena    smallint) 
returns numeric(14,2)
as
begin
  declare @monto numeric(14,2);
  -- parametro quincena es 3, cuando la transaccion se aplica en ambas quincenas. El 2do
  -- sum --> coalesce(sum(... es para ajustar cuando la cuota / 2 no es exacta, el ajuste
  -- se aplica en 2da quincena, aumentando el centavo para aplicar la cuota exacta del mes
  select @monto = coalesce(sum(iif(FormaAplicar = 3, coalesce(MontoCuota, 0)/2, coalesce(MontoCuota, 0))), 0) +
                  coalesce(sum(iif(FormaAplicar = 3 and day(@FechaFin) >= 16 and 
                  (round(coalesce(MontoCuota, 0) / 2, 2) * 2) < coalesce(MontoCuota, 0), 0.01, 0)), 0) 
    from PlaTransaccion 
   where Empleado = @OidEmpleado 
     and Cancelado = 0
     and clasificacion = @Clasificacion and (FormaAplicar = @NoQuincena or FormaAplicar = 3)
     and FechaInicio is not null
     and FechaInicio <= @FechaFin
  return coalesce(@monto, 0) 
end   
GO
grant execute on fnPlaTransaccionDe to public
go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Retornar el monto total de las transacciones por clasificacion a descontar en la planilla' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'fnPlaTransaccionDe'
GO