USE [SbtErp]
GO
/****** Object:  UserDefinedFunction [dbo].[fnConSaldoCuenta]    Script Date: 12/11/2021 16:06:24 ******/
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
--   @Cuenta      = código de la cuenta contable
-- NOTA       : Migrado desde el ERP SIAF
-- ================================================================================================
ALTER function [dbo].[fnConSaldoCuenta](
     @Empresa     smallint,
     @FechaHasta datetime,
     @CodigoCta   varchar(20))
returns money
as
begin
  declare @TipoSaldoDia smallint
  declare @FechaTmp datetime
  set @TipoSaldoDia = 1    -- operaciones del ejercicio
  -- obtener la fecha de la partida de apertura, por eso la condicion TipoSaldoDia = 0
  select @FechaTmp = s.Fecha from ConSaldoDiario s
   inner join ConCatalogo c
      on s.Cuenta = c.Oid
   where c.Empresa = @Empresa and s.Periodo = year(@FechaHasta) and s.TipoSaldoDia = 0   

  if @FechaHasta <= @FechaTmp
  begin
    -- aqui cuando el saldo a retornar es el de la partida de apertura
    set @TipoSaldoDia = 0
    set @FechaTmp = dateadd(day, 1, @FechaTmp)
  end  
  else
    set @FechaTmp = @FechaHasta
  declare @saldo money
  -- cuando la cuenta es deudora: debe - haber; acreedora: haber - debe
  select @saldo = sum(iif(c.TipoSaldoCta = 0, coalesce(s.Debe, 0) - coalesce(s.Haber, 0), coalesce(s.Haber, 0) - coalesce(s.Debe,0)))
    from ConSaldoDiario  s
   inner join ConCatalogo c
	  on s.Cuenta = c.Oid
   where c.Empresa = @Empresa
	 and c.CodigoCuenta  = @CodigoCta
	 and s.Periodo = year(@FechaHasta)
	 and s.Fecha   < @FechaTmp
	 and s.TipoSaldoDia <= @TipoSaldoDia
  return coalesce(@saldo, 0)
end
