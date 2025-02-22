﻿using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Xpo;
using Microsoft.Extensions.DependencyInjection;
using SBT.Apps.Base.Module.BusinessObjects;
using System;

namespace SBT.Apps.Base.Module.Controllers
{
    /// <summary>
    /// ViewController
    /// ViewController base del cual se debe heredar para disponer de funcionalidad comun para todos los controllers 
    /// independiente de la plataforma. 
    /// Implementa
    /// 1. Manejo de mensajes y notificaciones al usuario
    /// 2. Filtrar los datos para la empresa a la cual esta vinculado el usuario de la sesion,  siempre que el
    ///    TargetObjectType del ViewController sea un BO con la propiedad Empresa
    /// notificaciones de la ejecucion de los procesos
    /// </summary>
    /// <remarks>
    /// ADVERTENCIA: Tener cuidado al cambiar o agregar funcionalidad a este viewcontroller, porque afecta a todos los
    ///              que heredan de él (en la practica la mayoria de los controllers)
    ///              Ademas funcionalidad comun para todos los controllers heredados debe implementarse aca
    /// </remarks>
    public class ViewControllerBase : ViewController
    {
        private int fEmpresaOid;

        public ViewControllerBase()
        {
            DoInitializeComponent();
        }

        protected int EmpresaOid
        {
            get
            {
                if (fEmpresaOid <= 0)
                {
                    if (SecuritySystem.CurrentUser == null)
                        fEmpresaOid = ObjectSpace.GetObjectByKey<Usuario>(ObjectSpace.ServiceProvider.GetRequiredService<ISecurityStrategyBase>().User).Empresa.Oid;
                    else
                        fEmpresaOid = ((Usuario)SecuritySystem.CurrentUser).Empresa.Oid;
                }
                return fEmpresaOid;
            }
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            // revisar, se agrego aquí para no tenerlo repetido en cada vc, pero si da problemas habrá que quitarlo
            // Es para filtrar los datos para la empresa de la sesion y evitar que se mezclen cuando hay más de una empresa
            int empresaOid = 0;
            if (SecuritySystem.CurrentUser != null && SecuritySystem.CurrentUser is SBT.Apps.Base.Module.BusinessObjects.Usuario &&
                ((Usuario)SecuritySystem.CurrentUser).Empresa != null)
                empresaOid = ((Usuario)SecuritySystem.CurrentUser).Empresa.Oid;
            if ((string.Compare(View.GetType().Name, "ListView", StringComparison.Ordinal) == 0) && (((ListView)View).ObjectTypeInfo.FindMember("Empresa") != null) &&
                !(((ListView)View).CollectionSource.Criteria.ContainsKey("Empresa Actual")) && SecuritySystem.CurrentUser != null)
                ((ListView)View).CollectionSource.Criteria["Empresa Actual"] = CriteriaOperator.Parse("[Empresa.Oid] = ?", empresaOid);
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }

        protected override void Dispose(bool disposing)
        {
            DoDispose();
            base.Dispose(disposing);
        }

        protected virtual void DoInitializeComponent()
        {

        }

        protected virtual void DoDispose()
        {

        }

        /// <summary>
        /// Desplegar un mensaje con el resultado de la ejecución de un acción, valido en plataforma Windows y Web
        /// </summary>
        /// <param name="AMsg">Si necesita desplega algún valor debe formatear el mensaje con los valores a desplegar</param>
        /// <remarks>
        /// Ver https://docs.devexpress.com/eXpressAppFramework/118549/concepts/ui-construction/text-notifications 
        /// alternativa: https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ShowViewStrategyBase.ShowViewInPopupWindow(DevExpress.ExpressApp.View-System.Action-System.Action-System.String-System.String)
        /// 
        /// </remarks>
        private void DoMensaje(InformationType AType, string AMsg, string ACaption)
        {
            MessageOptions options = new()
            {
                Duration = 2000,
                Message = AMsg,
                Type = AType,
            };
            if (AType == InformationType.Error)
                options.Duration = 3000;
            options.Web.Position = InformationPosition.Bottom;
            options.Win.Caption = ACaption;
            options.Win.Type = WinMessageType.Flyout;
            Application.ShowViewStrategy.ShowMessage(options);
        }

        protected void MostrarMensajeResultado(string AMsg)
        {
            DoMensaje(InformationType.Success, AMsg, "Resultado");
        }

        protected void MostrarInformacion(string AMsg)
        {
            DoMensaje(InformationType.Info, AMsg, "Información");
        }

        protected void MostrarError(string AMsg)
        {
            DoMensaje(InformationType.Error, AMsg, "Error");
        }

        protected void MostrarAdvertencia(string AMsg)
        {
            DoMensaje(InformationType.Warning, AMsg, "Advertencia");
        }

    }
}
