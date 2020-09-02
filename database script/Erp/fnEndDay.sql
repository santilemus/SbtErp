 use Sbt_Erp
 go
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/***************************************************************************************
Creado Por     : Santiago Enrique Lemus
Propósito      : Retornar el final del dia de una fecha dada
Parámetros
  @Fecha       : Fecha a la cual se le va a calcular el final del dia
Retorna        : La hora final del dia
****************************************************************************************/ 
create function fnEndDay(@datepart varchar(3), @Fecha datetime2)
returns datetime2
as
begin
  declare @res datetime2
  if (Lower(@datepart) = 'mcs')
    set @res = dateadd(mcs, -1, cast(dateadd(day, 1, dateadd(day, datediff(day, 0, @Fecha), 0)) as datetime2))
  else
    set @res = dateadd(ns, -1, cast(dateadd(day, 1, dateadd(day, datediff(day, 0, @Fecha), 0)) as datetime2))
  return @res;
end
go
grant execute on dbo.fnEndDay to public
go



