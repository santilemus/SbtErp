USE [SbtErp]
GO
/****** Object:  StoredProcedure [dbo].[spConAbrirDias]    Script Date: 13/11/2021 13:06:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ================================================================================================
-- Author:		Santiago Enrique Lemus
-- Create date: 05/05/2020
-- Description:	Ejecutar el proceso de apertura de dias, siempre y cuando no se halla ejecutado aun
--              el cierre mensual
-- NOTA       : Migrado desde el ERP SIAF
-- ================================================================================================
ALTER procedure [dbo].[spConAbrirDias](
   @Empresa     int,
   @FechaDesde  datetime,
   @FechaHasta  datetime,
   @Usuario     varchar(25))
as
begin
  -- SET NOCOUNT ON added to prevent extra result sets from
  -- interfering with SELECT statements.
  set nocount on;
  declare @nfilas int = 0;
  select @nfilas = count(*) from ConCierre
   where Empresa = @Empresa 
     and FechaCierre between @FechaDesde and @FechaHasta
	 and (MesCerrado = 1 or FechaCierreAudit is not null)
  if @nfilas > 0 
    return; -- se sale porque el mes ya esta cerrado o uno o mas dias de auditoria ya fueron cerrados
  
  -- procedemos a abrir los dias
  update ConCierre 
     set DiaCerrado = 0
   where Empresa = @Empresa 
     and FechaCierre between @FechaDesde and @FechaHasta
  -- quitamos el atributo de mayorizadas a las partidas
  update ConPartida
     set Mayorizada = 0
   where Empresa = @Empresa 
     and Fecha between @FechaDesde and @FechaHasta
	 and Periodo = year(@FechaDesde)
  -- ahora ponemos los saldos de las cuentas a cero
  update s
     set s.Debe = 0,
	     s.Haber = 0,
		 s.DebeAjusteConsolida = 0,
		 s.HaberAjusteConsolida = 0
	from ConSaldoDiario s
   inner join ConCatalogo c
      on s.Cuenta = c.Oid
   where c.Empresa = @Empresa 
     and s.Periodo = year(@FechaDesde)
	 and s.Fecha between @FechaDesde and @FechaHasta
END
