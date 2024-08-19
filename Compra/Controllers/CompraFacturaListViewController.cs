using DevExpress.Data.Filtering;
using DevExpress.DataAccess.Native.Sql.ConnectionProviders;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Security;
using DevExpress.Office.Import.OpenXml;
using DevExpress.Persistent.Base;
using Microsoft.Extensions.DependencyInjection;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Compra.Module.BusinessObjects;
using SBT.Apps.Tercero.Module.BusinessObjects;
using SBT.eFactura.Dte;
using SBT.eFactura.Dte.Poco;
using SBT.eFactura.Dte.Poco.Receive;
using System;
using System.IO;
using System.Linq;
using System.Text;


namespace SBT.Apps.Compra.Module.Controllers
{
    public class CompraFacturaListViewController : ViewController<ListView>
    {
        private int fEmpresaOid = -1;
        private PopupWindowShowAction pwsaExportarAnexoPagoCuenta;
        private PopupWindowShowAction pwsaCargaDte;

        private int EmpresaOid
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
        public CompraFacturaListViewController()
        {
            TargetObjectType = typeof(SBT.Apps.Compra.Module.BusinessObjects.CompraFactura);

            pwsaExportarAnexoPagoCuenta = new PopupWindowShowAction(this, "Compras_AnexoPagoCuenta_Exportar", DevExpress.Persistent.Base.PredefinedCategory.RecordEdit.ToString());
            pwsaExportarAnexoPagoCuenta.Caption = "Exportar";
            pwsaExportarAnexoPagoCuenta.AcceptButtonCaption = "Exportar";
            pwsaExportarAnexoPagoCuenta.CancelButtonCaption = "Cancelar";
            pwsaExportarAnexoPagoCuenta.ActionMeaning = ActionMeaning.Accept;
            pwsaExportarAnexoPagoCuenta.ToolTip = "Clic para exportar el anexo con las retenciones de renta al formato requerido de pago a cuenta para cargarlo en la plataforma de declaración de impuestos";
            pwsaExportarAnexoPagoCuenta.TargetObjectType = typeof(SBT.Apps.Compra.Module.BusinessObjects.CompraFactura);
            pwsaExportarAnexoPagoCuenta.ImageName = "ExportToCSV";
            pwsaExportarAnexoPagoCuenta.CustomizePopupWindowParams += PwsaExportarAnexoPagoCuenta_CustomizePopupWindowParams;
            pwsaExportarAnexoPagoCuenta.Execute += PwsaExportarAnexoPagoCuenta_Execute;

            pwsaCargaDte = new PopupWindowShowAction(this, "CargarDteCompra", PredefinedCategory.RecordEdit.ToString());
            pwsaCargaDte.Caption = "Cargar Dte";
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            pwsaCargaDte.CustomizePopupWindowParams += PwsaCargaDte_CustomizePopupWindowParams;
            pwsaCargaDte.Execute += PwsaCargaDte_Execute;
        }

        private void PwsaCargaDte_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            DoCargarDte((FileUploadParameter)e.PopupWindowViewCurrentObject);
        }

        protected override void OnDeactivated()
        {
            pwsaCargaDte.CustomizePopupWindowParams -= PwsaCargaDte_CustomizePopupWindowParams;
            base.OnDeactivated();
        }

        private void PwsaCargaDte_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            IObjectSpace osParam = Application.CreateObjectSpace(typeof(FileUploadParameter));
            FileUploadParameter fileParams = osParam.CreateObject<FileUploadParameter>();
            e.View = Application.CreateDetailView(osParam, fileParams);
            e.View.Caption = "Cargar Dte";
        }

        private void PwsaExportarAnexoPagoCuenta_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            FechaParam pa = (e.PopupWindowViewCurrentObject as FechaParam);
            var fechaInicio = new DateTime(pa.Fecha.Year, pa.Fecha.Month, 01);
            var fechaFin = fechaInicio.AddMonths(1).AddSeconds(-1);
            CriteriaOperator criteria = CriteriaOperator.FromLambda<CompraFactura>(x => x.Empresa.Oid == EmpresaOid &&
                                            x.Fecha >= fechaInicio && x.Fecha <= fechaFin && x.Renta > 0.00m);
            ObjectSpace.ApplyCriteria(View.CollectionSource.Collection, criteria);
            try
            {
                DoExecuteExport(pa);
            }
            catch (Exception ex)
            {
                Application.ShowViewStrategy.ShowMessage($@"Error al exportar Anexo Pago a Cuenta (Retenciones de Renta){System.Environment.NewLine}{ex.Message}",
                     InformationType.Error);
            }
        }

        private void PwsaExportarAnexoPagoCuenta_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var os = Application.CreateObjectSpace(typeof(FechaParam));
            var pa = os.CreateObject<FechaParam>();
            IModelView detailViewModel = Application.FindModelView("FechaParam_DetailView_MonthYear");
            if (detailViewModel != null)
                e.View = Application.CreateDetailView(os, detailViewModel.Id, false, pa);
            else
                e.View = Application.CreateDetailView(os, pa);
            e.View.Caption = "Exportar Anexo Pago a Cuenta";
        }

        protected virtual void DoExecuteExport(FechaParam parametros)
        {

        }

        #region Metodos para cargar el Dte en las compras

        /// <summary>
        /// METODO TEMPORAL BORRAR DESPUES
        /// </summary>
        /// <param name="memoryStream"></param>
        /// <returns></returns>
        static string ConvertMemoryStreamToString(MemoryStream memoryStream)
        {
            // Reset the position of the MemoryStream
            memoryStream.Position = 0;

            // Read the MemoryStream and convert to string
            using (StreamReader reader = new StreamReader(memoryStream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        protected void DoCargarDte(FileUploadParameter parameter)
        {
            using MemoryStream ms = new MemoryStream();
            parameter.FileData.SaveToStream(ms);

            // NOTA. Falta evaluar el tipo de Dte para cargar los otros casos. Factura, Factura de Sujeto Excluido
            //DteRead<FeCcf> dteCcf = new DteRead<FeCcf>(ms);
            DteRead dteRead = new DteRead(ms);

            if (dteRead.JsonDte != null && dteRead.TipoDte == "03")
            {
                var ccf = dteRead.CreateObject<FeCcf>();
                string numFactura = ccf.Identificacion.CodigoGeneracion.Replace("-", "");
                if (ObjectSpace.FirstOrDefault<CompraFactura>(x => x.Empresa.Oid == EmpresaOid && x.NumeroFactura == numFactura) != null)
                {
                    Application.ShowViewStrategy.ShowMessage($@"Dte con código de generación {ccf.Identificacion.CodigoGeneracion} no se cargó porque ya existe", InformationType.Error);
                    return; // el dte ya se cargo en la aplicacion
                }
                var emp = ObjectSpace.GetObjectByKey<Empresa>(EmpresaOid);
                if (emp != null && emp.Nit != ccf.Receptor.Nit)
                {
                    Application.ShowViewStrategy.ShowMessage($@"El receptor del Dte {ccf.Receptor.Nombre}, no corresponde a la empresa de la sesión", InformationType.Error);
                    return;
                }
                if (ccf != null && ccf.Emisor != null)
                {
                    IObjectSpace os = Application.CreateObjectSpace(typeof(CompraFactura));
                    var compraFactura = os.CreateObject<CompraFactura>();
                    compraFactura.NumeroFactura = numFactura;
                    var tercero = ObtenerTercero(ccf.Emisor);
                    compraFactura.Proveedor = os.GetObject<Tercero.Module.BusinessObjects.Tercero>(tercero);
                    if (ccf.Identificacion.TipoDte == "03")
                        compraFactura.TipoFactura = os.FirstOrDefault<Listas>(x => x.Codigo == "COVE01");
                    compraFactura.Fecha = ccf.Identificacion.FechaEmision.DateTime;
                    compraFactura.Clase = EClaseDocumento.Dte;
                    compraFactura.ClasificacionRenta = EClasificacionRenta.Gasto;
                    if (ccf.Resumen.CondicionOperacion == 1)
                        compraFactura.CondicionPago = ECondicionPago.Contado;
                    else if (ccf.Resumen.CondicionOperacion == 2)
                        compraFactura.CondicionPago = ECondicionPago.Credito;
                    compraFactura.DiasCredito = 0;
                    compraFactura.Origen = EOrigenCompra.Local;
                    if (ccf.ResponseMH != null && ccf.ResponseMH.SelloRecibido != null)
                        compraFactura.Serie = ccf.ResponseMH.SelloRecibido;
                    else if (ccf.SelloRecibido != null)
                        compraFactura.Serie = ccf.SelloRecibido;
                    compraFactura.Moneda = os.FirstOrDefault<Moneda>(x => x.Codigo == ccf.Identificacion.TipoMoneda);
                    compraFactura.Exenta = ccf.Resumen.TotalExenta;
                    compraFactura.Gravada = ccf.Resumen.TotalGravada;
                    compraFactura.NoSujeta = ccf.Resumen.TotalNoSujeta;
                    compraFactura.Iva = ccf.Resumen.Tributos.FirstOrDefault<ResumenTributos>(x => x.Codigo == "02")?.Valor ?? 0.0m;
                    compraFactura.IvaPercibido = ccf.Resumen.IvaPercibido;
                    compraFactura.IvaRetenido = ccf.Resumen.IvaRetenido;
                    if (ccf.Resumen.Tributos != null)
                        compraFactura.Fovial = ccf.Resumen.Tributos.Where(x => x.Codigo == "D1" || x.Codigo == "C8").Sum(x => x.Valor);
 //                   compraFactura.Dte = dte.JsonDte; // para guardar el json del dte junto con la factura de compra
                    var compraFacturaDetailView = Application.CreateDetailView(os, compraFactura, true);
                    Application.ShowViewStrategy.ShowViewInPopupWindow(compraFacturaDetailView);
                    ObjectSpace.Refresh();
                }
            } 
        }

        /// <summary>
        /// Buscar el tercero que corresponde al emisor, sino existe lo crea
        /// </summary>
        /// <param name="emisor"></param>
        /// <returns></returns>
        private Tercero.Module.BusinessObjects.Tercero ObtenerTercero(Emisor emisor)
        {
            var os = Application.CreateObjectSpace(typeof(Tercero.Module.BusinessObjects.Tercero));
            var terceroDocumento = os.FirstOrDefault<TerceroDocumento>(x => x.Numero == emisor.Nit);
            if (terceroDocumento != null)
                return terceroDocumento.Tercero;
            terceroDocumento = os.FirstOrDefault<TerceroDocumento>(x => x.Numero == emisor.Nrc);
            if (terceroDocumento != null)
                return terceroDocumento.Tercero;
            var tercero = os.CreateObject<Tercero.Module.BusinessObjects.Tercero>();
            tercero.Nombre = emisor.Nombre;
            var terceroGiro = os.CreateObject<TerceroGiro>();
            terceroGiro.Tercero = tercero;
            terceroGiro.Sector = ESectorSujetoPasivo.Comercio;
            terceroGiro.ActEconomica = os.FirstOrDefault<ActividadEconomica>(x => x.Codigo == emisor.CodigoActividad);
            tercero.Giros.Add(terceroGiro);
            var terceroDireccion = os.CreateObject<TerceroDireccion>();
            terceroDireccion.Tercero = tercero;
            terceroDireccion.Pais = os.FirstOrDefault<ZonaGeografica>(x => x.Codigo == "SLV");
            terceroDireccion.Provincia = os.FirstOrDefault<ZonaGeografica>(x => x.Codigo == string.Concat(terceroDireccion.Pais.Codigo, emisor.Direccion.Departamento));
            terceroDireccion.Ciudad = os.FirstOrDefault<ZonaGeografica>(x => x.Codigo == string.Concat(terceroDireccion.Provincia, emisor.Direccion.Municipio));
            terceroDireccion.Direccion = emisor.Direccion.Complemento;
            tercero.Direcciones.Add(terceroDireccion);
            tercero.DireccionPrincipal = terceroDireccion;
            tercero.EMail = emisor.Correo;
            var terceroTelefono = os.CreateObject<TerceroTelefono>();
            terceroTelefono.Tercero = tercero;
            terceroTelefono.Telefono = os.FirstOrDefault<Telefono>(x => x.Numero == emisor.Telefono);
            if (terceroTelefono.Telefono == null)
            {
                var telefono = os.CreateObject<Telefono>();
                telefono.Numero = emisor.Telefono;
                telefono.Tipo = TipoTelefono.Fijo;
                telefono.Save();
                terceroTelefono.Telefono = telefono;
            }
            tercero.Telefonos.Add(terceroTelefono);
            var terceroNit = os.CreateObject<TerceroDocumento>();
            terceroNit.Tipo = os.FirstOrDefault<Listas>(x => x.Codigo == "NIT");
            terceroNit.Tercero = tercero;
            terceroNit.Numero = emisor.Nit;
            var terceroNrc = os.CreateObject<TerceroDocumento>();
            terceroNrc.Tipo = os.FirstOrDefault<Listas>(x => x.Codigo == "NRC");
            terceroNrc.Tercero = tercero;
            terceroNrc.Numero = emisor.Nrc;
            tercero.Documentos.Add(terceroNit);
            tercero.Documentos.Add(terceroNrc);
            var terceroRole = os.CreateObject<TerceroRole>();
            terceroRole.Tercero = tercero;
            terceroRole.IdRole = TipoRoleTercero.Proveedores;
            tercero.Roles.Add(terceroRole);
            //tercero.Save();
            var terceroDetailView = Application.CreateDetailView(os, tercero, true);
            Application.ShowViewStrategy.ShowViewInPopupWindow(terceroDetailView);
            return tercero;
        }

        #endregion
    }
}
