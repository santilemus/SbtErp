using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Updating;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.Linq;

namespace SBT.Apps.Medico.Module.DatabaseUpdate
{
    // For more typical usage scenarios, be sure to check out http://documentation.devexpress.com/#Xaf/clsDevExpressExpressAppUpdatingModuleUpdatertopic
    public class Updater : ModuleUpdater
    {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
            base(objectSpace, currentDBVersion)
        {
        }
        public override void UpdateDatabaseAfterUpdateSchema()
        {
            base.UpdateDatabaseAfterUpdateSchema();

            var empresa = CreateDefaultEmpresa();
            // If a role with the Administrators name doesn't exist in the database, create this role
            PermissionPolicyRole adminRole = ObjectSpace.FindObject<PermissionPolicyRole>(new BinaryOperator("Name", "Administrators"));
            if (adminRole == null)
            {
                adminRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                adminRole.Name = "Administrators";
                adminRole.AddTypePermissionsRecursively<Empresa>(SecurityOperations.FullAccess, SecurityPermissionState.Allow);
                adminRole.AddTypePermissionsRecursively<PermissionPolicyUser>(SecurityOperations.FullAccess, SecurityPermissionState.Allow);
                adminRole.AddTypePermissionsRecursively<Usuario>(SecurityOperations.FullAccess, SecurityPermissionState.Allow);
                adminRole.AddTypePermissionsRecursively<PermissionPolicyRole>(SecurityOperations.FullAccess, SecurityPermissionState.Allow);
                adminRole.IsAdministrative = true;
                adminRole.Save();
            }
            Usuario userAdmin = ObjectSpace.FindObject<Usuario>(new BinaryOperator("UserName", "Admin"));
            if (userAdmin == null)
            {
                userAdmin = ObjectSpace.CreateObject<Usuario>();
                userAdmin.UserName = "Admin";
                userAdmin.SetPassword("Admin#2021");
                userAdmin.ChangePasswordOnFirstLogon = true;
                userAdmin.Empresa = empresa;
                userAdmin.Agencia = empresa.Unidades.FirstOrDefault();
                userAdmin.Roles.Add(adminRole);
                userAdmin.Save();
            }

            // agregar role de super usuario. Es un usuario que tiene facultades para dar permisos y crear usuarios
            // pero NO es parte del role Administrators
            PermissionPolicyRole secAdminRole = CreateSecurityAdminRole();
            Usuario sysAdmin = ObjectSpace.FindObject<Usuario>(new BinaryOperator("UserName", "SysAdmin"));
            if (sysAdmin == null)
            {
                sysAdmin = ObjectSpace.CreateObject<Usuario>();
                sysAdmin.UserName = "SysAdmin";
                sysAdmin.SetPassword("");
                //sysAdmin.ChangePasswordOnFirstLogon = true;
                sysAdmin.Empresa = empresa;
                sysAdmin.Agencia = empresa.Unidades.FirstOrDefault();
                sysAdmin.Roles.Add(secAdminRole);
                sysAdmin.Save();
            }

            PermissionPolicyRole defaultRole = CreateDefaultRole();
            Usuario sampleUser = ObjectSpace.FindObject<Usuario>(new BinaryOperator("UserName", "Chamba"));
            if (sampleUser == null)
            {
                sampleUser = ObjectSpace.CreateObject<Usuario>();
                sampleUser.UserName = "Chamba";
                sampleUser.SetPassword("");
                sampleUser.Roles.Add(defaultRole);
                sampleUser.Save();
            }
            ObjectSpace.CommitChanges();
        }
        public override void UpdateDatabaseBeforeUpdateSchema()
        {
            base.UpdateDatabaseBeforeUpdateSchema();
            //if(CurrentDBVersion < new Version("1.1.0.0") && CurrentDBVersion > new Version("0.0.0.0")) {
            //    RenameColumn("DomainObject1Table", "OldColumnName", "NewColumnName");
            //}
        }
        private PermissionPolicyRole CreateDefaultRole()
        {
            PermissionPolicyRole defaultRole = ObjectSpace.FindObject<PermissionPolicyRole>(new BinaryOperator("Name", "Default"));
            if (defaultRole == null)
            {
                defaultRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                defaultRole.Name = "Default";
                defaultRole.AddObjectPermission<PermissionPolicyUser>(SecurityOperations.ReadOnlyAccess, "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                defaultRole.AddMemberPermission<PermissionPolicyUser>(SecurityOperations.Write, "ChangePasswordOnFirstLogon", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                defaultRole.AddMemberPermission<PermissionPolicyUser>(SecurityOperations.Write, "StoredPassword", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                defaultRole.AddMemberPermission<Usuario>(SecurityOperations.Write, "Agencia", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<PermissionPolicyRole>(SecurityOperations.Read, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<Empresa>(SecurityOperations.Read, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<Usuario>(SecurityOperations.Read, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<EmpresaUnidad>(SecurityOperations.Read, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.Create, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.Create, SecurityPermissionState.Allow);
                defaultRole.Save();
            }
            return defaultRole;
        }

        /// <summary>
        /// Super Usuario con algunos privilegios de Administrador
        /// Este usuario es para proporcionar en las implementaciones un usuario con privilegios de administracion limitados
        /// y restringir el role de Administradores a SBT
        /// </summary>
        /// <returns></returns>
        private PermissionPolicyRole CreateSecurityAdminRole()
        {
            PermissionPolicyRole securityAdminRole = ObjectSpace.FindObject<PermissionPolicyRole>(new BinaryOperator("Name", "Security Admin"));
            if (securityAdminRole == null)
            {
                securityAdminRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                securityAdminRole.Name = "Security Admin";
                securityAdminRole.PermissionPolicy = SecurityPermissionPolicy.AllowAllByDefault;
                securityAdminRole.AddTypePermissionsRecursively<Empresa>(SecurityOperations.FullAccess, SecurityPermissionState.Allow);
                securityAdminRole.AddTypePermissionsRecursively<EmpresaUnidad>(SecurityOperations.FullAccess, SecurityPermissionState.Allow);
                securityAdminRole.AddTypePermissionsRecursively<PermissionPolicyUser>(SecurityOperations.FullAccess, SecurityPermissionState.Allow);
                securityAdminRole.AddTypePermissionsRecursively<Usuario>(SecurityOperations.FullAccess, SecurityPermissionState.Allow);
                securityAdminRole.AddTypePermissionsRecursively<PermissionPolicyRole>(SecurityOperations.FullAccess, SecurityPermissionState.Allow);
                securityAdminRole.AddTypePermission<PermissionPolicyUser>(SecurityOperations.FullAccess, SecurityPermissionState.Allow);
                securityAdminRole.AddTypePermission<PermissionPolicyMemberPermissionsObject>(SecurityOperations.FullAccess, SecurityPermissionState.Allow);
                securityAdminRole.AddTypePermission<PermissionPolicyActionPermissionObject>(SecurityOperations.FullAccess, SecurityPermissionState.Allow);
                securityAdminRole.AddTypePermission<PermissionPolicyNavigationPermissionObject>(SecurityOperations.FullAccess, SecurityPermissionState.Allow);
                securityAdminRole.AddTypePermission<PermissionPolicyObjectPermissionsObject>(SecurityOperations.FullAccess, SecurityPermissionState.Allow);
                securityAdminRole.AddTypePermission<PermissionPolicyTypePermissionObject>(SecurityOperations.FullAccess, SecurityPermissionState.Allow);
                securityAdminRole.AddTypePermission<ReportDataV2>("Create;Write;Delete", SecurityPermissionState.Deny);
                securityAdminRole.AddTypePermission<ReportDataV2>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                securityAdminRole.AddTypePermission<Analysis>("Create;Write;Delete", SecurityPermissionState.Deny);
                securityAdminRole.AddTypePermission<Analysis>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                securityAdminRole.AddTypePermission<DashboardData>("Create;Write;Delete", SecurityPermissionState.Deny);
                securityAdminRole.AddTypePermission<DashboardData>(SecurityOperations.ReadOnlyAccess, SecurityPermissionState.Allow);
                securityAdminRole.IsAdministrative = false;
                securityAdminRole.CanEditModel = false;
                // definimos la estructura de permisos para evitar que puedan escalar a Administrador, pero que siga teniendo role para dar permisos
                securityAdminRole.AddNavigationPermission(@"Application/NavigationItems/Items/Seguridad/Items/Role", SecurityPermissionState.Allow);
                securityAdminRole.AddMemberPermission<PermissionPolicyRole>("Read;Write", "IsAdministrative;CanEditModel", "", SecurityPermissionState.Deny);
                securityAdminRole.AddObjectPermission<PermissionPolicyRole>("Read;Write;Delete;Navigate", "[Name] == 'Administrators'", SecurityPermissionState.Deny);
                securityAdminRole.AddMemberPermission<PermissionPolicyUser>(SecurityOperations.Write, "IsActive;Roles", "[UserName] = 'Admin'", SecurityPermissionState.Deny);
                securityAdminRole.AddObjectPermission<PermissionPolicyUser>("Read;Write;Delete;Navigate", "[UserName] = 'Admin'", SecurityPermissionState.Deny);
                securityAdminRole.AddObjectPermission<PermissionPolicyTypePermissionObject>("Write;Delete",
                    "[TargetType] In ('DevExpress.Persistent.BaseImpl.ReportDataV2', 'DevExpress.Persistent.BaseImpl.Analysis')", SecurityPermissionState.Deny);
                securityAdminRole.Save();
            }
            return securityAdminRole;
        }


        private Empresa CreateDefaultEmpresa()
        {
            var iCant = ObjectSpace.Evaluate(typeof(Empresa), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("Activa = true And Oid > 0"));
            Empresa emp;
            if (Convert.ToInt16(iCant) == 0)
            {
                emp = ObjectSpace.CreateObject<Empresa>();
                emp.RazonSocial = "Servicios Medicos, S.A de C.V";
                var zg = ObjectSpace.FindObject<ZonaGeografica>(new BinaryOperator("Codigo", "SLV"));
                if (zg != null)
                    emp.Pais = zg;
                zg = ObjectSpace.FindObject<ZonaGeografica>(new BinaryOperator("Codigo", "SLV06"));
                if (zg != null)
                    emp.Provincia = zg;
                zg = ObjectSpace.FindObject<ZonaGeografica>(new BinaryOperator("Codigo", "SLV0614"));
                if (zg != null)
                    emp.Ciudad = zg;
                emp.Direccion = "S/D";
                emp.Activa = true;
                emp.Unidades.Add(SucursalDefault(emp));
                emp.Save();
                emp.Session.CommitTransaction();
            }
            else
            {
                emp = ObjectSpace.FirstOrDefault<Empresa>(x => (x.Activa == true || x.Oid > 0));
                if (emp.Unidades.Count == 0)
                {
                    emp.Unidades.Add(SucursalDefault(emp));
                    emp.Save();
                    emp.Session.CommitTransaction();
                }
            }
            return emp;
        }

        private EmpresaUnidad SucursalDefault(Empresa empresa)
        {
            EmpresaUnidad suc = ObjectSpace.FindObject<EmpresaUnidad>(CriteriaOperator.Parse("Empresa = ? And Activa = true", empresa));
            if (suc == null)
            {
                suc = ObjectSpace.CreateObject<EmpresaUnidad>();
                suc.Empresa = empresa;
                suc.Nombre = "Agencia 1";
                suc.Role = ETipoRoleUnidad.Agencia;
                suc.Codigo = "AG0001";
                suc.Activa = true;
            }
            return suc;
        }
    }
}
