USE [Sbt_Erp]
GO

/****** Object:  UserDefinedFunction [dbo].[conSaldoCuenta]    Script Date: 25/2/2020 15:38:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- ================================================================================================
-- Author:		Santiago Enrique Lemus
-- Create date: 25/02/2020
-- Description:	Retorna el saldo contable de una cuenta a una fecha específica
-- Parametros :
--   @Empresa     = Código de la empresa
--   @FechaHasta  = Fecha hasta la cual se calcula el saldo (de aquí se obtiene el período)
--   @Oid         = Oid de la cuenta para la cual se necesita el saldo
-- NOTA       : Migrado desde el ERP SIAF
-- ================================================================================================
CREATE function [dbo].[fnConSaldoCuentaOid](
     @Empresa    int,
     @FechaHasta datetime,
     @Oid        int)
returns money
as
begin
  declare @TipoSaldoDia smallint
  declare @FechaTmp datetime
  set @TipoSaldoDia = 1    -- operaciones del ejercicio
  select @FechaTmp = Fecha from ConSaldoDiario
   where Empresa = @Empresa and Periodo = year(@FechaHasta) and TipoSaldoDia = 0 
  if @FechaHasta <= @FechaTmp
  begin
    set @TipoSaldoDia = 0
    set @FechaTmp = dateadd(day, 1, @FechaTmp)
  end  
  else
    set @FechaTmp = @FechaHasta
  
  declare @saldo money
  select @saldo = sum(iif(c.TipoSaldoCta = 0, coalesce(s.Debe, 0) - coalesce(s.Haber, 0),  coalesce(Haber, 0) - coalesce(Debe, 0)))
    from ConSaldoDiario s
   inner join ConCatalogo c
	  on s.Cuenta = c.OID
   where s.Empresa = @Empresa
	 and s.Cuenta  = @Oid
	 and s.Periodo = year(@FechaHasta)
	 and s.Fecha   < @FechaTmp
	 and s.TipoSaldoDia <= @TipoSaldoDia
  return coalesce(@saldo, 0)
end
GO
grant execute on [dbo].[fnConSaldoCuentaOid] to public
go

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Retorna el saldo de una cuenta contable a una fecha específica' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'fnConSaldoCuentaOid'
GO


