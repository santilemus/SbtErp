-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ================================================================================================
-- Author:		Santiago Enrique Lemus
-- Create date: 30/10/2024
-- Description:	Generar Balance de comprobación, que incluya cuentas de resultados
-- ================================================================================================
create or alter procedure spConBalanceComprobacion (
  @EmpresaOid int,
  @FechaHasta datetime,
  @Moneda     varchar(3)
)
as
begin
  set nocount on
  declare @filasLiquidacion bit
  declare @CuentaUtilidad  varchar(20)
  declare @CuentaPerdida   varchar(20)
  declare @CargosLiquida numeric(12, 2) = 0.0
  declare @AbonosLiquida numeric(12, 2) = 0.0

  if (month(@FechaHasta) = 12)
  begin
	select @filasLiquidacion = count(*)
	  from ConSaldoDiario s
	 inner join ConCatalogo c
	    on s.Cuenta = c.Oid 
	 where c.Empresa = @EmpresaOid
	   and s.Periodo = year(@FechaHasta)
	   and month(s.Fecha) = month(@FechaHasta)
	   and s.TipoSaldoDia in (2, 3)  -- liquidacion y cierre
	if (@filasLiquidacion > 0)
	begin
	  select @CuentaUtilidad = c.CodigoCuenta from ConCatalogo c
       where Empresa = @EmpresaOid and c.CuentaEspecial = 6
      select @CuentaPerdida = c.CodigoCuenta from ConCatalogo c
       where Empresa = @EmpresaOid and c.CuentaEspecial = 7

      select @CargosLiquida = sum(coalesce(Debe, 0)), 
	         @AbonosLiquida = sum(coalesce(Haber, 0))
	    from ConSaldoMes s
	   inner join ConCatalogo c
	      on s.Cuenta = c.Oid
	   where c.Empresa = @EmpresaOid
	     and s.Periodo = year(@FechaHasta)
	     and s.Mes = month(@FechaHasta)
	     and s.Cuenta = @CuentaUtilidad
	  if (@CargosLiquida = 0.0 And @AbonosLiquida = 0.0)
        select @CargosLiquida = sum(coalesce(Debe, 0)), 
	           @AbonosLiquida = sum(coalesce(Haber, 0))
	      from ConSaldoMes s
	     inner join ConCatalogo c
	        on s.Cuenta = c.Oid
	     where c.Empresa = @EmpresaOid
	       and s.Periodo = year(@FechaHasta)
	       and s.Mes = month(@FechaHasta)
	       and s.Cuenta = @CuentaPerdida
	end
  end
  ;with cteSaldoMes as
  (
    select s.Periodo, s.Mes, c.CodigoCuenta, c.Nombre, c.TipoCuenta, c.TipoSaldoCta, c.CtaMayor, 
	       c.CuentaEspecial, c.CtaResumen, s.SaldoInicio, 
           iif(c.TipoSaldoCta = 0, s.SaldoInicio + s.Debe, s.Debe) as Debe,
	       iif(c.TipoSaldoCta = 1, s.SaldoInicio + s.Haber, s.Haber) as Haber,
	       s.SaldoFin
      from ConSaldoMes s
     inner join ConCatalogo c
        on s.Cuenta = c.Oid
     where c.Empresa = 2
       and s.Periodo = 2023
       and s.Mes = 12
       and c.CuentaEspecial not in (6, 7)
     -- group by c.CodigoCuenta, c.TipoSaldoCta, c.CtaMayor, c.CuentaEspecial, c.TipoCuenta
  )
  select Periodo, Mes, CodigoCuenta, Nombre, TipoCuenta, TipoSaldoCta, CtaMayor, CuentaEspecial,
         CtaResumen, SaldoInicio, 
	     iif(TipoCuenta <> 3 And Mes <> 12, Debe, Debe - @CargosLiquida) as Debe,
		 iif(TipoCuenta <> 3 And Mes <> 12, Haber, Haber - @AbonosLiquida) as Haber,
		 iif(TipoCuenta <> 3 And Mes <> 12, SaldoFin, 
		 SaldoFin
    from cteSaldoMes
   order by CodigoCuenta
end
go
grant execute on spConBalanceComprobacion to public
go

execute spConBalanceComprobacion 2, '20231130', 'USD'
