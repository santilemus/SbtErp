﻿using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Medico.Blazor.Server.Controllers;
using SBT.Medico.Blazor.Server.Services;

namespace SBT.Medico.Blazor.Server;

public class BlazorBlazorApplication : BlazorApplication {
    public BlazorBlazorApplication() {
        ApplicationName = "SBT.Medico.Blazor";
        CheckCompatibilityType = DevExpress.ExpressApp.CheckCompatibilityType.DatabaseSchema;
        DatabaseVersionMismatch += BlazorBlazorApplication_DatabaseVersionMismatch;
        CreateCustomLogonWindowObjectSpace += BlazorBlazorApplication_CreateCustomLogonWindowObjectSpace;
    }

    private void BlazorBlazorApplication_CreateCustomLogonWindowObjectSpace(object sender, CreateCustomLogonWindowObjectSpaceEventArgs e)
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

    protected override void OnSetupStarted() {
        base.OnSetupStarted();
#if DEBUG
        if(System.Diagnostics.Debugger.IsAttached && CheckCompatibilityType == CheckCompatibilityType.DatabaseSchema) {
            DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
        }
#endif
    }
    private void BlazorBlazorApplication_DatabaseVersionMismatch(object sender, DatabaseVersionMismatchEventArgs e) {
#if EASYTEST
        e.Updater.Update();
        e.Handled = true;
#else
        if(System.Diagnostics.Debugger.IsAttached) {
            e.Updater.Update();
            e.Handled = true;
        }
        else {
            string message = "The application cannot connect to the specified database, " +
                "because the database doesn't exist, its version is older " +
                "than that of the application or its schema does not match " +
                "the ORM data model structure. To avoid this error, use one " +
                "of the solutions from the https://www.devexpress.com/kb=T367835 KB Article.";

            if(e.CompatibilityError != null && e.CompatibilityError.Exception != null) {
                message += "\r\n\r\nInner exception: " + e.CompatibilityError.Exception.Message;
            }
            throw new InvalidOperationException(message);
        }
#endif
    }

    /// <summary>
    /// Agregar CustomLogonEditActionVisibilityController a la colección de controladores activos de Login Page
    /// </summary>
    /// <returns></returns>
    protected override List<Controller> CreateLogonWindowControllers()
    {
        List<Controller> result = base.CreateLogonWindowControllers();
        result.Add(new CustomLogonEditActionVisibilityController());
        return result;
    }
}
