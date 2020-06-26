using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Banco.Module.BusinessObjects;
using SBT.Apps.Base.Module.Controllers;

namespace SBT.Apps.Banco.Module.Controllers
{
    /// <summary>
    /// Bancos
    /// Controlador que corresponde al BO de Conciliaciones Bancarias
    /// </summary>
    public class vcBancoConciliacion: ViewControllerBase
    {
        private SimpleAction saDetalleConciliacion;
        public vcBancoConciliacion(): base()
        {
            
        }

        protected override void DoInitializeComponent()
        {
            base.DoInitializeComponent();
            TargetObjectType = typeof(BancoConciliacion);
            saDetalleConciliacion = new SimpleAction(this, "saDetalleConciliacion", PredefinedCategory.Edit);
            saDetalleConciliacion.Caption = "Operaciones del Período";
            saDetalleConciliacion.ToolTip = "Clic para cargar el detalle de las transacciones de la cuenta bancaria en el período";
            saDetalleConciliacion.TargetObjectType = typeof(BancoConciliacion);
            saDetalleConciliacion.TargetViewType = ViewType.DetailView;
            saDetalleConciliacion.ImageName = "service";
            this.Actions.Add(this.saDetalleConciliacion);
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            saDetalleConciliacion.Execute += saDetalleConciliacion_Execute;
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
        }

        protected override void OnDeactivated()
        {
            saDetalleConciliacion.Execute -= saDetalleConciliacion_Execute;
            base.OnDeactivated();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (saDetalleConciliacion != null)
                    saDetalleConciliacion.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Evento para generar el detalle de la conciliacion con las transacciones bancarias del período.
        /// Se ejecuta procedimiento almacenado en la bae de datos
        /// </summary>
        /// <param name="sender">Objeto que dispara el evento</param>
        /// <param name="e">Argumento con la informacion del action </param>
        private void saDetalleConciliacion_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (((BancoConciliacion)View.CurrentObject).Detalles.Count == 0)
            {
                IObjectSpace os = Application.ObjectSpaceProvider.CreateObjectSpace();
                try
                {              
                    ((BancoConciliacion)View.CurrentObject).Session.CommitTransaction();
                    ((BancoConciliacion)View.CurrentObject).Reload();
                    var Oid = ((BancoConciliacion)View.CurrentObject).Oid;
                    if (Oid > 0)
                    {
                        ((XPObjectSpace)os).Session.ExecuteNonQuery("exec spBanDetalleConciliacion @Oid", new string[] { "Oid" }, new object[] { Oid });
                        ((BancoConciliacion)View.CurrentObject).Detalles.Reload();
                    }
                    MostrarMensajeResultado(string.Format("Detalle Generado. Son {0:N0} transacciones", ((BancoConciliacion)View.CurrentObject).Detalles.Count));
                }
                catch (Exception ex)
                {
                    MostrarError(string.Format("Detalle de la Conciliación no se pudo generar. Error {0}", ex.Message));
                }
                finally
                {
                    os.Dispose();
                }
            }
        }
    }
}
