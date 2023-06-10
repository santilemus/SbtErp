select * from ConCatalogo
 where Empresa = 1 
   and (CodigoCuenta like '6105%' or CodigoCuenta like '4103%' or CodigoCuenta like '610301')

select * from ConCatalogo
 where Empresa = 2
   and CuentaEspecial > 0

select * from ConPartida
 where Empresa = 2 and Periodo = 2021
   and Numero = 24
select * from ConPartidaDetalle
  where Partida = 246

delete from ConPartidaDetalle
 where Partida = 246
go
delete from ConPartida
 where Oid = 246 and Empresa = 2
go


select * from ConSaldoMes
 where Cuenta in (select Oid from ConCatalogo where Empresa = 1 and CodigoCuenta = '32010102')

select c.CodigoCuenta, c.Nombre, s.MesAnio, s.SaldoInicio, s.Debe, s.Haber, s.SaldoFin, 
       iif(c.TipoSaldoCta = 0, 'Deudor', 'Acreedor') as TipoSaldoCuenta, c.CtaResumen, c.CtaMayor
 from ConSaldoMes s
 inner join ConCatalogo c
    on s.Cuenta = c.Oid
 where c.Empresa = 2 and s.Periodo = 2021 and s.MesAnio = 2122021 and c.Nivel <= 3 and c.CodigoCuenta <> '7101'


select sum(s.Debe) as Debe, sum(s.Haber) as Haber 
 from ConSaldoMes s
 inner join ConCatalogo c
    on s.Cuenta = c.Oid
 where c.Empresa = 2 and s.Periodo = 2021 and s.MesAnio = 2122021 and c.CtaMayor = 1 
   and c.TipoCuenta in (1, 2, 3, 4, 5, 6, 7) --c.Nivel = 5 and c.CodigoCuenta <> '7101'

select c.CodigoCuenta, c.Nombre, s.MesAnio, s.SaldoInicio, s.Debe, s.Haber, s.SaldoFin, 
       iif(c.TipoSaldoCta = 0, 'Deudor', 'Acreedor') as TipoSaldoCuenta, c.CtaResumen, c.CtaMayor
 from ConSaldoMes s
 inner join ConCatalogo c
    on s.Cuenta = c.Oid
 where c.Empresa = 2 and s.Periodo = 2021 and s.MesAnio = 2122021 --and c.CtaResumen = 0 
   and (c.CodigoCuenta like '3%' or c.CodigoCuenta like '7%')

select max(Oid) from ConPartida
select max(Oid) from ConPartidaDetalle

dbcc checkident('ConPartida', reseed, 245)
go
dbcc checkident('ConPartidaDetalle', reseed, 1872)
go

select s.MesAnio, s.Cuenta, c.CodigoCuenta, c.Nombre, s.Mes, s.SaldoInicio, s.Debe, s.Haber, s.SaldoFin
 from ConSaldoMes s
 inner join ConCatalogo c
    on s.Cuenta = c.Oid
 where c.Empresa = 2 and s.Periodo = 2021 and s.MesAnio = 2122021 and c.CtaMayor = 1 
   and c.TipoCuenta in (1, 2, 3, 4, 5, 6, 7) --c.Nivel = 5 and c.CodigoCuenta <> '7101'


--- *********
declare @Empresa int = 2
declare @FechaHasta datetime = '2021/12/31'
declare @filas int = 1
declare @Oid int = 0
declare @OidPadre int
declare @OidTmp int
declare @CodigoCuenta  varchar(20)
declare @nombre varchar(120)
select @Oid = s.Cuenta, @OidPadre = c.CtaPadre, @CodigoCuenta = c.CodigoCuenta, @nombre = c.Nombre
  from ConSaldoDiario s
  inner join ConCatalogo c
	on s.Cuenta = c.Oid
 where c.Empresa = @Empresa
   and s.Periodo = Year(@FechaHasta)
   and s.TipoSaldoDia = 2
   and c.CuentaEspecial in (6, 7) -- UtilidadEjercicio, Perdida del Ejercicio
  and (s.Debe <> 0 or s.Haber <> 0)
while @@rowcount > 0
begin
  set @OidTmp = @OidPadre
  select @Oid as Oid, @OidPadre as OidPadre, @CodigoCuenta as CodigoCuenta, @Nombre as Nombre
  
  select @Oid = c.Oid, @OidPadre = c.CtaPadre, @CodigoCuenta = c.CodigoCuenta, @nombre = c.Nombre
    from ConCatalogo c
   where c.Oid = @OidTmp
     --and c.CtaPadre is not null
end

exec spConGeneraSaldosMes 2, '2021/12/31', 'AdminDit'


