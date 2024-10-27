select * from ConPartida
 where Empresa = 2 
   and Periodo = 2023
   and Tipo in (4, 5)
select * from ConPartidaDetalle
 where Partida in (964, 1028)


 /*
  delete from ConPartidaDetalle
   where Partida in (1028)
 */


 select dbo.fnConSaldoCuentaOid(2, '20231231 23:59:59', 1388, 3) as SaldoCuentaOid,
        dbo.fnConSaldoCuenta(2, '20231231 23:59:59', '11080103') as SaldoCuenta


 select dbo.fnConSaldoCuentaOid(2, '20231231 23:59:59', 1023, 3) as SaldoCuentaOid,
        dbo.fnConSaldoCuenta(2, '20231231 23:59:59', '330201') as SaldoCuenta,
		dbo.fnConSaldoFinMes(2, 2, 2023, 12, '330201') as SaldoFinMes

select s.*, c.TipoCuenta, c.TipoSaldoCta, c.CuentaEspecial from ConSaldoDiario s
 inner join ConCatalogo c
    on s.Cuenta = c.OID
 where c.Empresa = 2
   and s.Periodo = 2023
   and c.CodigoCuenta = '330201'
   and s.Fecha >= '20231231'

select s.*, c.TipoCuenta, c.TipoSaldoCta, c.CuentaEspecial from ConSaldoMes s
 inner join ConCatalogo c
    on s.Cuenta = c.OID
 where c.Empresa = 2
   and s.Periodo = 2023
   and c.CodigoCuenta = '330201'
   and s.Mes = 12

select * from ConCatalogo
 where Empresa = 2 
   and CuentaEspecial in (6, 7)
---
declare @mesAnio int = 2122023
declare @iMes int = 12
declare @FechaDesde datetime = '20231201'
declare @FechaHasta datetime = '20231231'
declare @TipoSaldoDia int = 2
declare @Empresa int = 2
select c.Empresa, s.Periodo, s.Cuenta, c.CodigoCuenta, @MesAnio, 
       dbo.fnConSaldoCuentaOid(c.Empresa, @FechaDesde, s.Cuenta, @tipoSaldoDia) as SaldoInicio,
	   dbo.fnConCargosCuentaMes(s.Periodo, @iMes, s.Cuenta) as CargosMes, 
	   dbo.fnConAbonosCuentaMes(s.Periodo, @iMes, s.Cuenta) as AbonosMes,
	   sum(iif(c.TipoSaldoCta = 0, coalesce(s.Debe, 0) - coalesce(s.Haber, 0), coalesce(s.Haber, 0) - coalesce(s.Debe, 0))) as SaldoFin
  from ConSaldoDiario s
 inner join ConCatalogo c
    on s.Cuenta = c.OID
 where (c.Empresa = @Empresa
   and s.Periodo = year(@FechaHasta)
   and cast(s.Fecha as Date) <= @FechaHasta)
-- las siguientes condiciones solo considera los saldos de Apertura y Operaciones Diarias TipoSaldoDia <= 1
-- o cuando el saldo es de liquidacion (TipoSaldoDia = 2) y la cuenta es de Activo, Pasivo, Patrimonio y Perdidas y Ganancias
   and (s.TipoSaldoDia <= 1 
    or  ((s.TipoSaldoDia = 2 And c.TipoCuenta in (1, 2, 3, 7)))) -- And c.CuentaEspecial not in (6, 7)))
 group by c.Empresa, s.Periodo, s.Cuenta, c.CodigoCuenta


 	  select s.Cuenta, c.CtaPadre, c.CtaMayor,
	         sum(coalesce(s.Debe, 0)) as Debe, sum(coalesce(s.Haber, 0)) as haber,
	         coalesce(sum(iif(c.TipoSaldoCta = 0, coalesce(s.Debe, 0) - coalesce(s.Haber, 0), 
			                    coalesce(s.Haber, 0) - coalesce(s.Debe, 0))), 0) as UtilidadPerdida
		from ConSaldoDiario s
	   inner join ConCatalogo c
		  on s.Cuenta = c.Oid
	   where c.Empresa = @Empresa
		 and s.Periodo = Year(@FechaHasta)
		 and s.TipoSaldoDia = 2         -- Saldos originados en la partida de liquidacion
		 and c.CuentaEspecial in (6, 7) -- UtilidadEjercicio, Perdida del Ejercicio 
	  group by s.Cuenta, c.CtaPadre, c.CtaMayor

execute spConGeneraSaldosMes 2, '20231231', 'admindit'

update x
   set x.Debe = 0.0,
       x.Haber = 0.0,
	   x.SaldoInicio = 0.0,
	   x.SaldoFin = 0.0
  from ConSaldoMes x
 inner join ConCatalogo c
    on x.Cuenta = c.OID 
 where c.Empresa = 2
   and x.Periodo = 2023
   and x.Mes = 12

select * from ConSaldoMes 
 where SaldoInicio is null or SaldoFin is null or  Debe is null or Haber is Null

on 

update x
   set x.Debe = 0.0,
       x.Haber = 0.0,
	   x.SaldoFin = 0.0,
	   x.SaldoInicio = 0.0
  from ConSaldoMes x
 inner join ConCatalogo c
    on x.Cuenta = c.OID 
 where c.Empresa = 2
   and x.Periodo = 2023
   and x.MesAnio = 2122023
 go


select sum(coalesce(Debe, 0)), sum(Coalesce(haber, 0))
  from ConSaldoMes x
 inner join ConCatalogo c
    on x.Cuenta = c.OID
 where c.Empresa = 2
   and x.Periodo = 2023
   and x.Mes = 12
   and c.CtaMayor = 1

select Month(Fecha), sum(coalesce(Debe, 0)), sum(Coalesce(haber, 0))
  from ConSaldoDiario x
 inner join ConCatalogo c
    on x.Cuenta = c.OID
 where c.Empresa = 2
   and x.Periodo = 2023
   and c.CtaMayor = 1
 group by Month(Fecha)


select c1.CodigoCuenta as CtaPadre, c.CodigoCuenta, x.SaldoInicio, x.Debe, x.Haber, x.SaldoFin from ConSaldoMes x
 inner join ConCatalogo c
    on x.Cuenta = c.Oid 
 inner join ConCatalogo c1
    on c.CtaPadre = c1.Oid 
 where Periodo = 2023
   and c.Empresa = 2
   and x.Mes = 12
   and c.CodigoCuenta like '3%'
   and x.


select x.* 
  from ConSaldoMes x
 inner join ConCatalogo c
    on x.Cuenta = c.OID 
 where c.Empresa = 2
   and x.Periodo = 2023
   and x.MesAnio = 2122023


select c1.Oid, c1.CtaPadre as OidPadre, c2.CodigoCuenta as CuentaPadre, c1.CodigoCuenta, c1.Nombre, c1.Nivel, c2.Nivel, c1.TipoSaldoCta, c2.TipoSaldoCta,
       (c1.Nivel - c2.Nivel) as DifNivel
  from ConCatalogo c1
 inner join ConCatalogo c2
    on c1.CtaPadre = c2.Oid 
 where c1.Empresa = 2
 order by c1.CodigoCuenta


 select * from ConPartidaDetalle 
  where GCRecord is not null

  delete from ConPartidaDetalle 
   where GCRecord is not null
go


select x.*, c.CodigoCuenta, c.Nombre
  from ConSaldoMes x
 inner join ConCatalogo c
    on x.Cuenta = c.OID 
 where c.Empresa = 2
   and x.Periodo = 2023
   and x.Mes = 12
  -- and c.CtaMayor = 1
   and (x.Debe = 14665.90 or x.Haber = 14665.90)


select x.*, c.CodigoCuenta, c.Nombre
  from ConSaldoDiario x
 inner join ConCatalogo c
    on x.Cuenta = c.OID 
 where c.Empresa = 2
   and x.Periodo = 2023
   and Month(x.Fecha) = 12
   and (x.Debe <> 0.0 or x.Haber <> 0.0)
   and c.Nivel < 4
   and (x.Debe = 14665.90 or x.Haber = 14665.90)