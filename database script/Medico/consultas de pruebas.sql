select * from Enfermedad

select * from ZonaGeografica where Codigo like 'SLV06%'

drop table SecuritySystemMemberPermissionsObject;
go
drop table SecuritySystemObjectPermissionsObject;
go
drop table SecuritySystemTypePermissionsObject;
go
drop table SecuritySystemRoleParentRoles_SecuritySystemRoleChildRoles;
go
drop table SecuritySystemUserUsers_SecuritySystemRoleRoles
go
drop table SecuritySystemUser;
go
drop table SecuritySystemRole;
go
drop table PermissionPolicyRole
go
drop table PermissionPolicyUser
go
drop table PermissionPolicyMemberPermissionsObject
go
drop table PermissionPolicyNavigationPermissionsObject
go
drop table PermissionPolicyObjectPermissionsObject
go
drop table PermissionPolicyTypePermissionsObject
go
drop table PermissionPolicyUser
go
drop table PermissionPolicyUserUsers_PermissionPolicyRoleRoles
go
drop table UsuarioEmpresa
go

delete from EmpresaUnidad
dbcc checkident('EmpresaUnidad', reseed, 0)

delete from Empresa
dbcc checkident('Empresa', reseed, 0)
go

select * from EmpresaUnidad
select * from Empresa
select * from UsuarioEmpresa

drop table EmpresaUnidad
