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
                userAdmin.Empresa = empresa;
                userAdmin.Agencia = empresa.Unidades.FirstOrDefault();
                // Set a password if the standard authentication type is used
                userAdmin.SetPassword("");
                userAdmin.Roles.Add(adminRole);
                userAdmin.Save();
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

        private Empresa CreateDefaultEmpresa()
        {
            var iCant = ObjectSpace.Evaluate(typeof(Empresa), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("Activa = true And Oid > 0"));
            Empresa emp = ObjectSpace.FindObject<Empresa>(CriteriaOperator.Parse("Activa = true And Oid > 0"));
            if (emp == null)
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
