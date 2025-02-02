select * from CompraFactura
 where Oid in (1166, 1109)
go

update CompraFactura
   set Saldo = Gravada + Iva + IvaPercibido,
       Estado = 0
 where Oid = 1196

select * from CxPTransaccion
select * from BancoTransaccion
go

delete from CxPTransaccion
go
dbcc checkident('CxPTransaccion', reseed, 0)
go

delete from BancoTransaccion
 where Oid = 2
go
dbcc checkident('BancoTransaccion', reseed, 1)
go

select * from CxCTipoTransaccion
select * from InventarioTipoMovimiento