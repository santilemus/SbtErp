SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ================================================================================================
-- Author:		Santiago Enrique Lemus
-- Create date: 30/04/2022
-- Description:	Generar Auxiliar de Cuentas
-- ================================================================================================
create procedure spConAuxiliarCuenta (
        @Empresa     int,
		@FechaDesde  datetime,
        @FechaHasta  datetime,
		@CuentaDesde varchar(20),
		@CuentaHasta varchar(20),
		@Moneda      varchar(3)
 )
as
begin
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.

	set nocount on;

	declare @valorMonedaRep money = 1.0;
	declare @Plural varchar(25) = '';
	declare @Logo varbinary(max);
	declare @RazonSocial varchar(200)

	select @valorMonedaRep = FactorCambio, @Plural = Plural
	  from Moneda
	 where Codigo = @Moneda;

	select @RazonSocial = RazonSocial, @Logo = Logo
	  from Empresa
	 where Oid = @Empresa;

	with SaldoMes (Cuenta, Mes, SaldoInicio, Debe, Haber, SaldoFin) as
	(
	     select s.Cuenta, s.Mes, round(s.SaldoInicio * @valorMonedaRep, 2), round(s.Debe * @valorMonedaRep, 2), 
		        round(s.Haber * @valorMonedaRep, 2),
				round(iif(Month(@FechaDesde) = Month(@FechaHasta), s.SaldoFin, 
				      dbo.fnConSaldoCuentaOid(@Empresa, @FechaHasta, s.Cuenta)) * @valorMonedaRep, 2)
		   from ConSaldoMes s
		  inner join ConCatalogo c
		     on s.Cuenta = c.Oid
		  where c.Empresa = @Empresa
		    and s.Periodo = Year(@FechaDesde)
			and s.Mes = Month(@FechaDesde)
			and c.CodigoCuenta between @CuentaDesde and @CuentaHasta
	),
	Partidas (Numero, Fecha, Cuenta, Concepto, Debe, Haber, CtaPresupuesto, Presupuesto, Moneda, ValorMoneda) as
	(
		select e.Numero, e.Fecha, d.Cuenta, d.Concepto, round(d.Debe / e.ValorMoneda * @valorMonedaRep, 2), 
		       round(d.Haber / e.ValorMoneda * @valorMonedaRep, 2), d.CtaPresupuesto, e.Presupuesto,
		       e.Moneda, e.ValorMoneda
		  from ConPartidaDetalle d 
		 inner join ConPartida e
			on d.Partida = e.Oid
		 inner join ConCatalogo c
		    on d.Cuenta = c.Oid
		 where e.Empresa = @Empresa
		   and e.Periodo = Year(@FechaDesde)
		   and e.Fecha between @FechaDesde and @FechaHasta
		   and c.CodigoCuenta between @CuentaDesde and @CuentaHasta
	)
	select c.CodigoCuenta, c.Nombre, s.Mes, p.Numero, p.Fecha, p.Concepto, s.SaldoInicio,p.Presupuesto, 
	       s.Debe as TotalDebe,  s.Haber as TotalHaber, s.SaldoFin, p.Debe, p.Haber, 
		   p.CtaPresupuesto, c.TipoCuenta, c.TipoSaldoCta, @Plural as Plural, @RazonSocial as RazonSocial, @Logo as Logo
      from SaldoMes s
	  left join Partidas p
	    on s.Cuenta = p.Cuenta
	 inner join ConCatalogo c
	    on s.Cuenta = c.Oid
END
GO
grant execute on spConAuxiliarCuenta to public
go
