using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SBT.Apps.Medico.Module
{
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

            Usuario usuario = objectSpace.FindObject<Usuario>(
                new BinaryOperator("UserName", customLogonParameters.UserName));

            if (usuario == null)
                throw new ArgumentNullException("CustomAuthentication.Authenticate: Usuario es Nulo");

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
