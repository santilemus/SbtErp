select * from ConPartida
 where Empresa = 2
   and Periodo = 2023
   and Tipo in (4, 5)

select * from ConPartidaDetalle 
 where Partida in (964, 1028)
go
delete from ConPartidaDetalle
 where Partida in (964, 1028)


select x.Cuenta, x.Mes, x.SaldoInicio, x.Debe, x.Haber, x.SaldoFin, c.CodigoCuenta, c.Nombre, iif(c.TipoSaldoCta = 0, 'Deudor', 'Acreedor') as TipoSaldo
  from ConSaldoMes x
 inner join ConCatalogo c
    on x.Cuenta = c.OID 
 where c.Empresa = 2
   and x.Periodo = 2023
   and x.Mes = 12
   and c.CtaMayor = 1
 order by c.CodigoCuenta

select * from ConCatalogo
 where CodigoCuenta like '7%'

execute spConGeneraSaldosMes 2, '20231231', 'AdminDit'

select c.CodigoCuenta, sum(d.Debe) as Debe, sum(d.Haber) as Haber from ConPartidaDetalle d
 inner join ConPartida p
    on d.Partida = p.Oid 
 inner join ConCatalogo c
    on d.Cuenta = c.Oid 
 where p.Empresa = 2
   and p.Periodo = 2023
  group by c.CodigoCuenta
  order by c.CodigoCuenta

select c.CodigoCuenta, sum(s.Debe) as Debe, sum(s.Haber) as Haber,
       sum(iif(c.TipoSaldoCta = 0, s.Debe - s.Haber, s.Haber - s.Debe)) as Saldo
  from ConSaldoDiario s
 inner join ConCatalogo c
    on s.Cuenta = c.Oid
 where c.Empresa = 2
   and s.Periodo = 2023
   --and c.CtaResumen = 0
   and c.Nivel = 2
   and s.TipoSaldoDia < 2
 group by c.CodigoCuenta
  order by c.CodigoCuenta

select c.CodigoCuenta, sum(iif(s.Mes = 1, s.SaldoInicio, 0)) as SaldoInicio, 
       sum(iif(c.TipoSaldoCta = 0, iif(s.Mes = 1, s.SaldoInicio, 0) + s.Debe, s.Debe)) as Debe,
	   sum(iif(c.TipoSaldoCta = 1, iif(s.Mes = 1, s.SaldoInicio, 0) + s.Haber, s.Haber)) as Haber,
       sum(iif(s.Mes = 12, s.SaldoFin, 0.0)) as SaldoFin
  from ConSaldoMes s
 inner join ConCatalogo c
    on s.Cuenta = c.Oid
 where c.Empresa = 2
   and s.Periodo = 2023
   --and s.Mes = 12
   and c.CtaResumen = 0
   --and c.Nivel = 2
   --and c.TipoCuenta <> 7
  group by c.CodigoCuenta
  order by c.CodigoCuenta

/*
3,485,315.38
2,029,844.86

*/

select * from ConCatalogo
 where Empresa = 2
   and CodigoCuenta like '3%'

-- hay que hacer un procedimiento almacenado para el caso del balance de comprobación que incluya 
-- cuentas de resultado (por el caso que ya exista partida de liquidación)
-- sino hacerlo desde la tabla ConSaldoDiario
select c.CodigoCuenta, c.Nombre, c.TipoCuenta, c.TipoSaldoCta, c.CtaMayor, c.CuentaEspecial, 
       c.CtaResumen, s.SaldoInicio, 
       iif(c.TipoSaldoCta = 0, s.SaldoInicio + s.Debe, s.Debe) as Debe,
	   iif(c.TipoSaldoCta = 1, s.SaldoInicio + s.Haber, s.Haber) as Haber,
	   s.SaldoFin,
	   iif(s.Mes = 12 And c.TipoCuenta = 3, dbo.fnConCargosCuentaMes(s.Periodo, s.Mes, ), 0.0) as DebeAjusteResultado,
	   iif(s.Mes = 12 And c.TipoCuenta = 3, dbo.fnCarg
  from ConSaldoMes s
 inner join ConCatalogo c
    on s.Cuenta = c.Oid
 where c.Empresa = 2
   and s.Periodo = 2023
   and s.Mes = 12
   and c.CuentaEspecial not in (6, 7)
 -- group by c.CodigoCuenta, c.TipoSaldoCta, c.CtaMayor, c.CuentaEspecial, c.TipoCuenta
  order by c.CodigoCuenta


select * from ReportDataV2
 where GCRecord is not null