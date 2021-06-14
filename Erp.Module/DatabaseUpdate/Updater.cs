using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.Linq;


namespace SBT.Apps.Erp.Module.DatabaseUpdate
{
    // For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Updating.ModuleUpdater
    public class Updater : ModuleUpdater
    {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
            base(objectSpace, currentDBVersion)
        {
        }
        public override void UpdateDatabaseAfterUpdateSchema()
        {
            base.UpdateDatabaseAfterUpdateSchema();
            CreateDatabaseObjects();

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
            ObjectSpace.CommitChanges(); //This line persists created object(s).
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
            Empresa emp;
            if (Convert.ToInt16(iCant) == 0)
            {
                emp = ObjectSpace.CreateObject<Empresa>();
                emp.RazonSocial = "SBT Technology, SA de CV";
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
                iCant = ObjectSpace.Evaluate(typeof(EmpresaUnidad), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("Empresa = ? And Activa = true", emp));
                EmpresaUnidad suc = ObjectSpace.CreateObject<EmpresaUnidad>();
                suc.Empresa = emp;
                suc.Nombre = "Agencia 1";
                suc.Role = ETipoRoleUnidad.Agencia;
                suc.Codigo = "AG0001";
                suc.Activa = true;
                emp.Unidades.Add(suc);
                emp.Save();
                emp.Session.CommitTransaction();
                return emp;
            }
            else
                return null;
        }

        private void CreateDatabaseObjects()
        {
            var Os = (XPObjectSpace)this.ObjectSpace;
            var DataLayer = Os.Session.DataLayer as DevExpress.Xpo.SimpleDataLayer;
            var provider = DataLayer.Connection.ConnectionString.Split(new char[] { ';' }).FirstOrDefault(z => z.Contains("XpoProvider"));
            if (provider == "MSSqlServer")
            {
                AddComputedColumnSqlServer("ConCatalogo", "Nivel", "(case when [CtaPadre] IS NULL then (1) else len(ltrim(rtrim([CodigoCuenta])))/(2)+(1) end)");
            }
        }

        /// <summary>
        /// Agregar columnas calculadas a algunas tablas cuando el provider es MSSqlServer
        /// </summary>
        private void AddComputedColumnSqlServer(string tableName, string colName, string expression)
        {
            if (string.IsNullOrEmpty(tableName) || string.IsNullOrEmpty(colName) || string.IsNullOrEmpty(expression))
                return;
            string ddl = $"if not exists(select * from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = '{tableName}' and COLUMN_NAME = '{colName}') " +
                $"alter table {tableName} add {colName} as {expression}";
            this.ExecuteNonQueryCommand(ddl, true);
        }

    }
}
