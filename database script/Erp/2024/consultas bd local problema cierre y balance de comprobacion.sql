select * from ConPartida
 where Empresa = 2
   and Periodo = 2023
   and Tipo in (4, 5)

select * from ConPartidaDetalle 
 where Partida in (964, 1028)
go
delete from ConPartidaDetalle
 where Partida in (964, 1028)


select x.Cuenta, x.Mes, x.SaldoInicio, x.Debe, x.Haber, x.SaldoFin, c.CodigoCuenta, c.Nombre
  from ConSaldoMes x
 inner join ConCatalogo c
    on x.Cuenta = c.OID 
 where c.Empresa = 2
   and x.Periodo = 2023
   and x.Mes = 12
   and c.CtaMayor = 1