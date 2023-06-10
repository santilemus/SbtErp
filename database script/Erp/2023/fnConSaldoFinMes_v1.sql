USE [SbtErp]
GO

/****** Object:  UserDefinedFunction [dbo].[fnConSaldoInicioMes]    Script Date: 6/6/2023 14:53:19 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

  -- Autor         : Santiago Enrique Lemus
  -- Fecha Creacion: 06/06/2023
  -- Usuario Creo  : Santiago Lemus
  -- Descripción   :
	  -- Retorna el saldo final del mes para una cuenta por empresa y período
	  -- @cod_emp1 = la primer empresa a considerar 
	  -- @cod_emp2 = la segunda empresa sí el saldo es consolidado. 
	  --             Sí se emite para una sola empresa es igual al parámetro @cod_emp1
	  -- @cod_periodo = Período contable
	  -- @mes = mes al que se obtiene el saldo
	  -- @cod_cuenta  = Código de la cuenta contable para la cual se obtiene el saldo inicial
CREATE   function [dbo].[fnConSaldoFinMes](
     @Empresa1     smallint,
     @Empresa2     smallint,
     @Periodo      smallint,
     @Mes          smallint,
     @Cuenta   varchar(20))
returns money
as
begin 
  declare @saldo money
  select @saldo = sum(coalesce(s.SaldoFin, 0)) from ConSaldoMes s
   inner join ConCatalogo c
      on s.Cuenta = c.Oid
   where c.Empresa  between @Empresa1 and @Empresa2
     and s.Periodo = @Periodo
     and s.Mes     = @mes
     and c.CodigoCuenta  = @Cuenta  
  return coalesce(@saldo, 0)
end
GO
grant execute on dbo.fnConSaldoFinalMes to public
go

