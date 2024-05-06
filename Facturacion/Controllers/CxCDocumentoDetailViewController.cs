using DevExpress.ExpressApp;
using SBT.Apps.CxC.Module.BusinessObjects;


namespace SBT.Apps.Facturacion.Module.Controllers
{
    public class CxCDocumentoDetailViewController: ViewController<DetailView>
    {
        public CxCDocumentoDetailViewController(): base()
        {
            TargetObjectType = typeof(SBT.Apps.CxC.Module.BusinessObjects.CxCDocumento);
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
        }

        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            if (View == null || View.CurrentObject == null || e.Object == null)
                return;
            if (View.CurrentObject == e.Object && ObjectSpace.IsNewObject(View.CurrentObject))
            {
                var co = (View.CurrentObject as CxCDocumento);
                if (e.PropertyName == "AutorizacionDocumento")
                {
                    if (e.NewValue == null)
                    {
                        ((CxCDocumento)View.CurrentObject).Referencia = null;
                        Application.ShowViewStrategy.ShowMessage($@"No se encontrón autorización de correlativos para {co.Tipo.Nombre}. No se conoce el número de documento", InformationType.Error);
                        return;
                    }
                }
                if (e.PropertyName == "Referencia")
                {
                    if (co.AutorizacionDocumento.Clase != Base.Module.BusinessObjects.EClaseDocumento.Dte)
                    {
                        int intReferencia;
                        if (int.TryParse(co.Referencia.Trim(), out intReferencia) && (intReferencia < co.AutorizacionDocumento.NoDesde || intReferencia > co.AutorizacionDocumento.NoHasta))
                        {
                            Application.ShowViewStrategy.ShowMessage($@"El número de documento esta fuera del rango aprobado para {co.AutorizacionDocumento.Tipo.Nombre}", InformationType.Error);
                            return;
                        }
                    }
                }
            }
        }

        protected override void OnDeactivated()
        {
            ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
            base.OnDeactivated();
        }
    }
}
