with Saldo as 
(
	select c.Empresa, s.Periodo, s.Mes, s.Cuenta, c.CodigoCuenta, c.Nombre, c.Nivel, c.TipoCuenta, 
	       c.TipoSaldoCta, c.CtaMayor, c.CtaPadre, s.SaldoFin as SaldoActual
	  from ConSaldoMes s
	 inner join ConCatalogo c
		on s.Cuenta = c.Oid 
	 where c.Empresa = 1
	   and s.Periodo = 2023
	   and s.Mes = 12
	   and c.TipoCuenta in (1, 2, 3)
	   and c.CtaResumen = 1
	   and c.Nivel <= 3
	
	union all

	select c.Empresa, s.Periodo, s.Mes, s.Cuenta, c.CodigoCuenta, c.Nombre, c.Nivel, c.TipoCuenta, 
	        c.TipoSaldoCta, c.CtaMayor, ss.CtaPadre, s.SaldoFin as SaldoActual
	  from ConSaldoMes s
	 inner join ConCatalogo c
	    on s.Cuenta = c.Oid 
	  join Saldo ss
		on s.Cuenta = c.CtaPadre
)
select *, dbo.fnConSaldoFinMes(Empresa, Empresa, Periodo - 1, Mes, CodigoCuenta) as SaldoAnterior
  from Saldo
 order by CodigoCuenta