USE [Sbt_Erp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  -- Autor         : Santiago Enrique Lemus
  -- Fecha Creacion: 13/03/2020
  -- Usuario Creo  : Santiago Lemus
  -- Descripción   :
	  -- Retorna el saldo al inicio del mes para una cuenta por empresa y período
	  -- @cod_emp1 = la primer empresa a considerar 
	  -- @cod_emp2 = la segunda empresa sí el saldo es consolidado. 
	  --             Sí se emite para una sola empresa es igual al parámetro @cod_emp1
	  -- @cod_periodo = Período contable
	  -- @mes = mes al que se obtiene el saldo
	  -- @cod_cuenta  = Código de la cuenta contable para la cual se obtiene el saldo inicial
create or alter function [dbo].[fnConSaldoInicioMes](
     @Empresa1     smallint,
     @Empresa2     smallint,
     @Periodo      smallint,
     @Mes          smallint,
     @Cuenta   varchar(20))
returns money
as
begin 
  declare @saldo money
  select @saldo = sum(coalesce(SaldoInicio, 0)) from ConSaldoMes
   where Empresa  between @Empresa1 and @Empresa2
     and Periodo = @Periodo
     and Mes     = @mes
     and Cuenta  = @Cuenta  
  return coalesce(@saldo, 0)
end
go
grant execute on dbo.fnConSaldoInicioMes to public
go
