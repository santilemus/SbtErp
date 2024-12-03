SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ================================================================================================
-- Author:		Santiago Enrique Lemus
-- Create date: 17/11/2024
-- Description:	Extraer información para Balance General Comparativo
-- Modificaciones:
-- NOTA:
-- Pendiente de modificar para agregar caso cuando no hay partida de liquidación y 
-- la utilidad o pérdida se calcula de la diferencia de ingresos - costos - gastos
-- ================================================================================================
create or alter procedure spConBalanceGeneral
(
   @EmpresaOid int,
   @FechaHasta datetime,
   @Moneda     varchar(3)
)
as
begin
  set nocount on;
  declare @FactorCambio numeric(12,4) = 1.00
  declare @ingresosAct     numeric(12,2) = 0.0
  declare @gastosCostosAct numeric(12,2) = 0.0
  declare @ingresosAnt     numeric(12,2) = 0.0
  declare @gastosCostosAnt numeric(12,2) = 0.0
  declare @filasLiq     int = 0;
  declare @CtaOid       int -- id de la cuenta de utilidad o perdida, cuando aun no hay cierre o liq.
  declare @CtaAntOid    int -- id de la cuenta de utilidad o perdida, del periodo anterior al mes que se esta comparando
  
  select @FactorCambio = FactorCambio
    from Moneda
   where Codigo = @Moneda

  if (Month(@FechaHasta) = 12)
  begin
    select @filasLiq = count(*) from ConPartidaDetalle d
	 inner join ConPartida e
	    on d.Partida = e.Oid 
     where e.Empresa = @EmpresaOid
       and e.Periodo = year(@FechaHasta)
	   and e.Tipo in (4, 5)
	   and e.Mayorizada = 1
  end
  -- cuando no hay liquidacion y cierre, entonces se calcula la utilidad de operacion y para eso
  -- se obtienen los ingresos y los costos + gastos
  if @filasLiq = 0  
  begin
    select @ingresosAct = coalesce(s.SaldoFin, 0.00)
	  from ConSaldoMes s
	 inner join ConCatalogo c
	    on s.Cuenta = c.Oid 
	 where c.Empresa = @EmpresaOid 
	   and s.Periodo = year(@fechaHasta)
	   and s.Mes     = month(@fechaHasta)
	   and c.TipoCuenta = 6
	   and c.Nivel = 1
	select @gastosCostosAct = coalesce(s.SaldoFin, 0.00)
	  from ConSaldoMes s
	 inner join ConCatalogo c
	    on s.Cuenta = c.Oid 
	 where c.Empresa = @EmpresaOid 
	   and s.Periodo = year(@fechaHasta)
	   and s.Mes     = month(@fechaHasta)
	   and c.TipoCuenta in (4, 5)
	   and c.CtaMayor = 1	  
    select @ingresosAnt = coalesce(s.SaldoFin, 0.00)
	  from ConSaldoMes s
	 inner join ConCatalogo c
	    on s.Cuenta = c.Oid 
	 where c.Empresa = @EmpresaOid 
	   and s.Periodo = year(@fechaHasta) - 1
	   and s.Mes     = month(@fechaHasta)
	   and c.TipoCuenta = 6
	   and c.Nivel = 1
	select @gastosCostosAct = coalesce(s.SaldoFin, 0.00)
	  from ConSaldoMes s
	 inner join ConCatalogo c
	    on s.Cuenta = c.Oid 
	 where c.Empresa = @EmpresaOid 
	   and s.Periodo = year(@fechaHasta) - 1
	   and s.Mes     = month(@fechaHasta)
	   and c.TipoCuenta in (4, 5)
	   and c.CtaMayor = 1	
	if ((@ingresosAct - @gastosCostosAct) >= 0)
	  select @CtaOid = Oid from ConCatalogo
	   where Empresa = @EmpresaOid
	     and CuentaEspecial = 6
		 and Activa = 1
	else 
	  select @CtaOid = Oid from ConCatalogo
	   where Empresa = @EmpresaOid
	     and CuentaEspecial = 7   -- hay perdida
		 and Activa = 1   
    print 'CtaOid es ==> ' + cast(@ctaOid as varchar(20))
	if ((@ingresosAnt - @gastosCostosAnt) >= 0)
	  select @CtaAntOid = Oid from ConCatalogo
	   where Empresa = @EmpresaOid
	     and CuentaEspecial = 6
		 and Activa = 1
	else 
	  select @CtaAntOid = Oid from ConCatalogo
	   where Empresa = @EmpresaOid
	     and CuentaEspecial = 7   -- hay perdida
		 and Activa = 1  
  end

  ;with Saldo as 
  (
	select c.Empresa, s.Periodo, s.Mes, s.Cuenta, c.CtaPadre, s.SaldoFin as SaldoActual
	  from ConSaldoMes s
	 inner join ConCatalogo c
		on s.Cuenta = c.Oid 
	 where c.Empresa = @EmpresaOid
	   and s.Periodo = Year(@FechaHasta)
	   and s.Mes = Month(@FechaHasta)
	   and c.TipoCuenta in (1, 2, 3)
	   and c.CtaResumen = 1
	   and c.Nivel <= 3
	union all

	select c.Empresa, s.Periodo, s.Mes, s.Cuenta, c.CtaPadre, s.SaldoFin as SaldoActual
	  from ConSaldoMes s
	 inner join ConCatalogo c
	    on s.Cuenta = c.Oid 
	  join Saldo ss
		on s.Cuenta = c.CtaPadre
  )
  select Empresa, Periodo, Mes, Cuenta, CtaPadre, Round(SaldoActual * @FactorCambio, 2) as SaldoActual 
	into #balance
    from Saldo
  if @filasLiq = 0
  begin
    insert into #balance
	       (Empresa, Periodo, Mes, cuenta, CtaPadre, SaldoActual)
	values (@EmpresaOid, Year(@FechaHasta), Month(@FechaHasta), @CtaOid, null, @ingresosAct - @gastosCostosAct)
  end   
  print 'antes del select de salida'
  -- pendiente de registrar el ingreso anterior en la salida y ademas acumularlo en la cuenta padre (ambos - anterior y actual)
  select b.*, Round(dbo.fnConSaldoFinMes(b.Empresa, b.Empresa, Periodo - 1, Mes, c.CodigoCuenta) * @FactorCambio, 2) as SaldoAnterior,
         c.CodigoCuenta, c.Nombre, c.TipoCuenta, c.TipoSaldoCta, c.CtaResumen, c.CuentaEspecial, c.CtaResumen, g.Nombre
    from #balance b
   inner join ConCatalogo c
      on b.Cuenta = c.Oid 
    left join ConCatalogoGrupo g
      on c.Grupo = g.Oid 
   order by c.CodigoCuenta, g.Orden
end
go
grant execute on spConBalanceGeneral to public
go

-- exec spConBalanceGeneral 1, '20230630', 'USD'

--2023	10	362	361	2659.16000000	2872.85000000	3301	UTILIDADES