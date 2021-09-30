using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SBT.Apps.Erp.Module
{
    /// <summary>
    /// Implementacion de CustomAuthentication para el SBT-ERP. 
    /// Se requiere la seleccion de la empresa y la Agencia, antes de autenticarse. El usuario debe existir en la empresa
    /// para que permita autenticarse. 
    /// </summary>
    /// <remarks>
    /// Documentacion en: https://docs.devexpress.com/eXpressAppFramework/112982/task-based-help/security/how-to-use-custom-logon-parameters-and-authentication
    /// Pendiente de agregar actualizacion de la agencia seleccionada en el perfil del usuario
    /// </remarks>
    public class CustomAuthentication : AuthenticationBase, IAuthenticationStandard
    {
        private CustomLogonParameters customLogonParameters;
        public CustomAuthentication()
        {
            customLogonParameters = new CustomLogonParameters();
        }
        public override void Logoff()
        {
            base.Logoff();
            customLogonParameters = new CustomLogonParameters();
        }
        public override void ClearSecuredLogonParameters()
        {
            customLogonParameters.Password = "";
            base.ClearSecuredLogonParameters();
        }
        public override object Authenticate(IObjectSpace objectSpace)
        {

            Usuario usuario = objectSpace.FindObject<Usuario>(CriteriaOperator.Parse("UserName == ?", customLogonParameters.UserName));           
                //new BinaryOperator("UserName", customLogonParameters.UserName));

            if (usuario == null)
                throw new ArgumentNullException("Usuario", $"No Existe el Usuario {customLogonParameters.UserName}, en {customLogonParameters.Empresa.RazonSocial}");
            if (usuario.Empresa.Oid != customLogonParameters.Empresa.Oid)
                throw new ArgumentException($"Al usuario {customLogonParameters.UserName} no se le permite ingresar a la empresa {customLogonParameters.Empresa.RazonSocial}");

            if (!usuario.ComparePassword(customLogonParameters.Password))
                throw new AuthenticationException(
                    usuario.UserName, "Password Incorrecto.");
            return usuario;
        }

        public override void SetLogonParameters(object logonParameters)
        {
            this.customLogonParameters = (CustomLogonParameters)logonParameters;
        }

        public override IList<Type> GetBusinessClasses()
        {
            return new Type[] { typeof(CustomLogonParameters) };
        }
        public override bool AskLogonParametersViaUI
        {
            get { return true; }
        }
        public override object LogonParameters
        {
            get { return customLogonParameters; }
        }
        public override bool IsLogoffEnabled
        {
            get { return true; }
        }
    }
}
