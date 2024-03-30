using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using System;
using System.Collections.Generic;

namespace SBT.Apps.Empleado.Module
{
    // For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ModuleBase.
    public sealed partial class EmpleadoModule : ModuleBase
    {
        public EmpleadoModule()
        {
            InitializeComponent();
            BaseObject.OidInitializationMode = OidInitializationMode.AfterConstruction;

            // registramos la funcion que retorna el Oid del empleado vinculado al usuario logeado
            CriteriaOperator.RegisterCustomFunction(new EmpleadoActualOidFunction());
        }
        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB)
        {
            ModuleUpdater updater = new DatabaseUpdate.Updater(objectSpace, versionFromDB);
            return new ModuleUpdater[] { updater };
        }
        public override void Setup(XafApplication application)
        {
            base.Setup(application);
            // Manage various aspects of the application UI and behavior at the module level.
        }
        public override void CustomizeTypesInfo(ITypesInfo typesInfo)
        {
            base.CustomizeTypesInfo(typesInfo);
            CalculatedPersistentAliasHelper.CustomizeTypesInfo(typesInfo);
            AgregarEmpleadoAUsuario(typesInfo);
        }

        /// <summary>
        /// Vincular al usuario su correspondiente object de Empleado, porque hay servicios y funciones cuyos BO necesitan 
        /// el ID del empleado relacionado al usuario. Ejemplo, caso de las Agendas (schedule) y los empleados so  recursos. 
        /// </summary>
        /// <param name="typesInfo"></param>
        /// <remarks>
        ///  Alternativa: Implementar en Empleado interface ISecurityUser y pasa a ser el BO para usuarios, como se explica en 
        ///  https://docs.devexpress.com/eXpressAppFramework/113452/task-based-help/security/how-to-implement-a-custom-security-system-user-based-on-an-existing-business-class
        ///  El inconveniente es cuando se tiene usuarios que no son empleados. Ejemplo: personas que laboran subcontratados
        ///  o terceros como Auditoria y que se es necesario darles acceso a servicios de la aplicacion.
        ///  Ademas la clase Usuario esta en un module y Empleado en otro, por eso se implementa de forma dinamica
        /// </remarks>
        private void AgregarEmpleadoAUsuario(ITypesInfo typesInfo)
        {
            ITypeInfo tInfoUsuario = typesInfo.FindTypeInfo(typeof(SBT.Apps.Base.Module.BusinessObjects.Usuario));
            IMemberInfo mInfoEmpleado = tInfoUsuario.FindMember("Empleado");
            if (mInfoEmpleado == null)
            {
                mInfoEmpleado = tInfoUsuario.CreateMember("Empleado", typeof(SBT.Apps.Empleado.Module.BusinessObjects.Empleado));
                mInfoEmpleado.AddAttribute(new DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Empleado"));
                mInfoEmpleado.AddAttribute(new DevExpress.Xpo.IndexedAttribute());
                mInfoEmpleado.AddAttribute(new DevExpress.Persistent.Base.ImmediatePostDataAttribute(true));
                mInfoEmpleado.AddAttribute(new DevExpress.Persistent.Base.DataSourceCriteriaAttribute("[Estado.Categoria] == 9 && !([Estado.Codigo] In ('EMPL02', 'EMPL08', 'EMPL09'))"));
                mInfoEmpleado.AddAttribute(new DevExpress.ExpressApp.Model.DetailViewLayoutAttribute(DevExpress.ExpressApp.Model.LayoutColumnPosition.Left));
            }
        }
    }
}
