select *
  from ConCatalogo
 where Empresa = 2
   and Activa = 1

-- 1. validar que no hay cuentas de resumen en partidas contables
select c1.*
  from ConCatalogo c1
 where c1.Empresa = 2
   and c1.CtaResumen = 1
   and exists (select Cuenta from ConPartidaDetalle d
                where d.Cuenta = c1.Oid)

-- 2. validar que una cuenta de detalle no sea cuenta padre. CREAR VALIDACION
select c1.*
  from ConCatalogo c1
 where c1.Empresa = 2
   and c1.CtaResumen = 0
   and exists (select * from ConCatalogo c2
                 where c2.CtaPadre = c1.Oid)


select * from Tercero
 where GCRecord is not null

update Tercero
   set GCRecord = null
 where GCRecord is not null
go

select * from CompraFactura
 where Proveedor = 66


select *
  from ConCatalogo c1
 where c1.Empresa = 2
   and c1.CtaPadre = 745

select * from ConPartidaDetalle
 where Cuenta = 745

 
select e.Numero, e.Fecha, e.Elaboro, c.Oid as OidCuenta, c.CodigoCuenta, d.Concepto, d.Debe, d.Haber, 
       e.Elaboro, e.FechaCrea, c.CtaPadre, c1.CodigoCuenta as CodigoCtaPadre
  from ConPartidaDetalle d
 inner join ConPartida e
    on d.Partida = e.OID
 inner join ConCatalogo c
    on d.Cuenta = c.Oid 
 inner join ConCatalogo c1
    on c.CtaPadre = c1.Oid 
 where e.Empresa = 2
   and d.Cuenta = 1364

--
select c1.Oid, c1.Empresa, c1.CodigoCuenta, c1.Nombre, c1.TipoCuenta, c1.TipoSaldoCta, c1.CtaResumen, 
       c1.Nivel, c2.CodigoCuenta as CodigoCtaPadre, c2.Nivel as NivelPadre, c2.TipoSaldoCta as TipoSaldoCtaPadre, 
	   c2.CtaResumen as CtaResumenPadre
  from ConCatalogo c1
 inner join ConCatalogo c2
    on c1.CtaPadre = c2.Oid
 where c1.Empresa = 2
   and (c1.Nivel - c2.Nivel) > 1
 
s
delete from Tercero
 where GCRecord is not null


select * from ConCatalogo
 where Empresa = 2
   and CodigoCuenta like '1102030301'