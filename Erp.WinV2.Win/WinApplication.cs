using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ApplicationBuilder;
using DevExpress.ExpressApp.Win;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Win.Utils;
using DevExpress.ExpressApp.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using DevExpress.ExpressApp.Security.ClientServer.WebApi;

namespace SBT.Apps.Erp.WinV2.Win;

// For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Win.WinApplication._members
public class WinV2WindowsFormsApplication : WinApplication {
    public WinV2WindowsFormsApplication() {
		SplashScreen = new DXSplashScreen(typeof(XafSplashScreen), new DefaultOverlayFormOptions());
        ApplicationName = "SBT.Apps.Erp.WinV2";
        CheckCompatibilityType = DevExpress.ExpressApp.CheckCompatibilityType.DatabaseSchema;
        UseOldTemplates = false;
        DatabaseVersionMismatch += WinV2WindowsFormsApplication_DatabaseVersionMismatch;
        CustomizeLanguagesList += WinV2WindowsFormsApplication_CustomizeLanguagesList;
        CreateCustomLogonWindowObjectSpace += WinV2WindowsFormsApplication_CreateCustomLogonWindowObjectSpace;
        WebApiDataServerHelper.AddKnownType(typeof(SBT.Apps.Base.Module.BusinessObjects.CustomLogonParameters));
    }

    private void WinV2WindowsFormsApplication_CreateCustomLogonWindowObjectSpace(object sender, CreateCustomLogonWindowObjectSpaceEventArgs e)
    {
        e.ObjectSpace = ((XafApplication)sender).CreateObjectSpace(typeof(CustomLogonParameters));
        NonPersistentObjectSpace nonPersistentObjectSpace = e.ObjectSpace as NonPersistentObjectSpace;
        if (nonPersistentObjectSpace != null)
        {
            if (!nonPersistentObjectSpace.IsKnownType(typeof(SBT.Apps.Base.Module.BusinessObjects.Empresa), true))
            {
                IObjectSpace additionalObjectSpace = ((XafApplication)sender).CreateObjectSpace(typeof(SBT.Apps.Base.Module.BusinessObjects.Empresa));
                nonPersistentObjectSpace.AdditionalObjectSpaces.Add(additionalObjectSpace);
                nonPersistentObjectSpace.Disposed += (s2, e2) => {
                    additionalObjectSpace.Dispose();
                };
            }
        }
        ((CustomLogonParameters)e.LogonParameters).RefrescarAgencias(e.ObjectSpace);
    }

    private void WinV2WindowsFormsApplication_CustomizeLanguagesList(object sender, CustomizeLanguagesListEventArgs e) {
        string userLanguageName = System.Threading.Thread.CurrentThread.CurrentUICulture.Name;
        if(userLanguageName != "en-US" && e.Languages.IndexOf(userLanguageName) == -1) {
            e.Languages.Add(userLanguageName);
        }
    }
    private void WinV2WindowsFormsApplication_DatabaseVersionMismatch(object sender, DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs e) {
		string message = "Application cannot connect to the specified database.";
		
		CompatibilityDatabaseIsOldError isOldError = e.CompatibilityError as CompatibilityDatabaseIsOldError;
		if(isOldError != null && isOldError.Module != null) {
			message = "The client application cannot connect to the Middle Tier Application Server and its database. " +
					  "To avoid this error, ensure that both the client and the server have the same modules set. Problematic module: " + isOldError.Module.Name +
					  ". For more information, see https://docs.devexpress.com/eXpressAppFramework/113439/concepts/security-system/middle-tier-security-wcf-service#troubleshooting";
		}
		if(e.CompatibilityError == null) {
			message = "You probably tried to update the database in Middle Tier Security mode from the client side. " +
					  "In this mode, the server application updates the database automatically. " +
					  "To disable the automatic database update, set the XafApplication.DatabaseUpdateMode property to the DatabaseUpdateMode.Never value in the client application.";
		}
		throw new InvalidOperationException(message);
	}

    /// <summary>
    /// Para habilitar las fuentes de datos personalizadas
    /// </summary>
    /// <param name="args"></param>
    /// <remarks>
    /// mas info en https://supportcenter.devexpress.com/ticket/details/t318553/reportsv2-how-to-activate-the-add-report-data-source-option-to-add-new-custom-non-xaf
    /// </remarks>
    protected override void OnLoggedOn(LogonEventArgs args)
    {
        base.OnLoggedOn(args);
        DevExpress.ExpressApp.ReportsV2.Win.ReportDesignerTypeDescriptionProvider.RemoveProvider();
    }
}
