using DevExpress.ExpressApp.Security;
using Microsoft.Extensions.Options;
using SBT.Apps.Erp.Module;

namespace SBT.Apps.Erp.WinV2.MiddleTier
{
    public class CustomAuthenticationStandardProvider : AuthenticationStandardProviderV2
    {
        public CustomAuthenticationStandardProvider(IOptions<AuthenticationStandardProviderOptions> options, 
            IOptions<SecurityOptions> securityOptions) :  base(options, securityOptions)
        { 

        }

        protected override AuthenticationBase CreateAuthentication(Type userType, Type logonParametersType)
        {
            return new CustomAuthentication();
        }
    }
}
