select * from Usuario
select * from PermissionPolicyUser
select * from Empresa
select * from BancoTransaccion

sp_help BancoTransaccion

drop index iProveedor_BancoTransaccion on BancoTransaccion
go
alter table BancoTransaccion
 drop constraint FK_BancoTransaccion_Proveedor
go
alter table BancoTransaccion
  drop column Proveedor
go

select * from Tercero 

select * from CxCTipoTransaccion
select * from BancoTipoTransaccion

select * from ReportDataV2