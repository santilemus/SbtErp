delete EmpresaUnidad
delete from Empresa
dbcc checkident('EmpresaUnidad', reseed, 0)
dbcc checkident('Empresa', reseed, 0)
go
select * from PermissionPolicyRole


select * from Empresa
select * from PermissionPolicyRole
 where Oid = '745C21C8-5ABF-4303-B827-CEFE63D6F418'
select * from [dbo].[PermissionPolicyUserUsers_PermissionPolicyRoleRoles]
 Where Roles = '745C21C8-5ABF-4303-B827-CEFE63D6F418'
select * from PermissionPolicyUser
select * from [dbo].[PermissionPolicyObjectPermissionsObject]
select * from [dbo].[PermissionPolicyTypePermissionsObject]
select * from ModuleInfo
delete from ModelDifference
truncate table ModelDifferenceAspect
delete from Usuario
select * from Usuario
