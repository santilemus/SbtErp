USE [Sbt_Erp]
GO
/****** Object:  UserDefinedFunction [dbo].[fnDiasLaborales]    Script Date: 12/7/2020 23:09:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/***************************************************************************************
Creado Por     : Santiago Enrique Lemus
Propósito      : Retornar las cantidad de dias laborales de un período comprendido entre
                 FechaDesde y FechaHasta excluyendo los dias de descanso (normalmente domingo
				 o fin de semana completo)
Parámetros
  @FechaInicio : Fecha de inicio del período a incluir en el cálculo de las horas extras
  @FechaFin    : Fecha de finalización del período a incluir en el cálculo de horas extras
  @Modo        : Modo de exclusion del fin de semana o dia de descanso semanal
                 0 no existe dia de descanso 
                 1 excluye solo un dia de descanso por semana (domingo), 
				 2 excluye dos dias de descanso (fin de semana)
Retorna        : Los días laborales comprendidos entre fecha desde y fecha hasta, excluyendo
                 los dias de descanso y asuetos
Comentarios    : Todos los asuetos deben estar parametrizados en el sistema para que el dato
                 retornado sea exacto
				 El parametro modo es necesario porque algunas empresas y las publicas normalmente no
				 laboran todo el fin de semana, pero la mayor parte de la empresa privada solo hay un
				 dia de descanso a la semana
****************************************************************************************/ 
  
create function [dbo].[fnDiasLaborales](
  @FechaInicio  datetime2,
  @FechaFin  datetime2,
  @Modo smallint = 1) 
returns int
as
begin
  declare @Asuetos int
  declare @DiasDescanso int
  -- Sí los parámetros de entrada están en orden incorrecto, los revierte
  select @Asuetos = count(*) from Asueto
   where Fecha between cast(@FechaInicio as date) and dbo.fnEndDay('ns', @FechaFin)
  if (@Modo > 0)
    set @DiasDescanso = datediff(week, cast(@FechaInicio as date), dbo.fnEndDay('ns', @FechaFin)) * @Modo;
  return datediff(day, cast(@FechaInicio as date), dbo.fnEndDay('ns', @FechaFin)) - coalesce(@DiasDescanso, 0) - @Asuetos;
end
