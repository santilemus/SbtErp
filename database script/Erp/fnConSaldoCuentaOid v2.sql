USE [SbtErp]
GO
/****** Object:  UserDefinedFunction [dbo].[fnConSaldoCuentaOid]    Script Date: 12/11/2021 16:08:44 ******/
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
ALTER function [dbo].[fnConSaldoCuentaOid](
     @Empresa    int,
     @FechaHasta datetime,
     @Oid        int)
returns money
as
begin
  declare @TipoSaldoDia smallint
  declare @FechaTmp datetime
  set @TipoSaldoDia = 1    -- operaciones del ejercicio
  select @FechaTmp = s.Fecha from ConSaldoDiario s
   inner join ConCatalogo c
      on s.Cuenta = c.Oid
   where c.Empresa = @Empresa and s.Periodo = year(@FechaHasta) and s.TipoSaldoDia = 0 
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
	  on s.Cuenta = c.Oid
   where c.Empresa = @Empresa
	 and s.Cuenta  = @Oid
	 and s.Periodo = year(@FechaHasta)
	 and s.Fecha   < @FechaTmp
	 and s.TipoSaldoDia <= @TipoSaldoDia
  return coalesce(@saldo, 0)
end
