using DevExpress.Data.Filtering;
using DevExpress.Data.ODataLinq.Helpers;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Security;
using Microsoft.Extensions.DependencyInjection;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Contabilidad.Module.BusinessObjects;
using System;
using System.Linq;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using System.Collections.Generic;
using SBT.Apps.Contabilidad.BusinessObjects;
using System.Reflection;

namespace SBT.Apps.Erp.Module.Controllers.Contabilidad
{
    /// <summary>
    /// View Controller para probar mecanismo de generación de Partidas automáticas desde otros módulos y utilizando mecanismo de partidas modelo
    /// Si funciona como se requiere debe ser el controlador base para implementar la funcionalidad generica de generación de partidas contables
    /// de las transacciones en diferentes módulos
    /// </summary>
    /// <remarks>
    /// I D E A S
    /// 1. Es el controlador base, implementar aquí la funcionalidad común o genérica
    /// 2. Será necesario evaluar como proceder cuando se modifica la transacción que dió origen a la partida (factura de venta por ejemplo). Se traslada
    ///    la modificación a la partida de forma automática (que sucede si ya fue cerrado el día o mes), o el contador debe realizar un asiento de diario
    ///    para aplicar el ajuste.
    /// </remarks>
    public class IntegracionAsientoContableController : ViewController
    {
        int empresaOid;
        //IObjectSpace osPartidaModelo;
        SingleChoiceAction scaModelos;  // revisar si esta se puede utilizar. Tomar en cuenta que en algunos casos se debe crear un popup para el ingreso de parámetros
                                        // ejemplo: si es una partida de ventas consolidada por día o período, es necesario poder ingresar: fecha desde y fecha hasta
        private int EmpresaOid
        {
            get
            {
                if (empresaOid <= 0)
                {
                    if (SecuritySystem.CurrentUser == null)
                        empresaOid = ObjectSpace.GetObjectByKey<Usuario>(ObjectSpace.ServiceProvider.GetRequiredService<ISecurityStrategyBase>().User).Empresa.Oid;
                    else
                        empresaOid = ((Usuario)SecuritySystem.CurrentUser).Empresa.Oid;
                }
                return empresaOid;
            }
        }

        public IntegracionAsientoContableController() : base()
        {
            // la siguiente linea se debe asignar en los controladores heredados
            scaModelos = new SingleChoiceAction(this, "Seleccionar Partidas Modelo", DevExpress.Persistent.Base.PredefinedCategory.Tools);
            scaModelos.Caption = "Generar Partida";
            scaModelos.ToolTip = @"Seleccionar modelo para generar el asiento contable del BO actual";
            scaModelos.ItemType = SingleChoiceActionItemType.ItemIsMode;
            scaModelos.DefaultItemMode = DefaultItemMode.LastExecutedItem;
            scaModelos.EmptyItemsBehavior = EmptyItemsBehavior.Disable;
            scaModelos.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            scaModelos.ShowItemsOnClick = true;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            FillItemFromModelo();
            scaModelos.Execute += ScaModelos_Execute;
        }

        protected override void OnDeactivated()
        {
            scaModelos.Execute -= ScaModelos_Execute;
            base.OnDeactivated();
        }

        private void FillItemFromModelo()
        {
            CriteriaOperator criteria = CriteriaOperator.FromLambda<PartidaModelo>(x => x.Empresa.Oid == EmpresaOid && x.TipoBO == TargetObjectType);
            IList<PartidaModelo> modelos = ObjectSpace.GetObjects<PartidaModelo>(criteria);
            if (modelos.Count > 0)
            {
                scaModelos.Items.Clear();
                foreach (var modelo in modelos)
                    scaModelos.Items.Add(new ChoiceActionItem(modelo.Nombre, modelo));
            }
        }

        private void ScaModelos_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            DoExecute(e);
        }

        protected void DoSourceBO<T>(string caption, ViewType viewType, string viewId = "")
        {
            TargetObjectType = typeof(T);
            TargetViewType = viewType;
            scaModelos.Caption = caption;
            scaModelos.TargetObjectType = typeof(T);
            scaModelos.TargetViewType = viewType;
            scaModelos.Id = string.Format("{0}_GenerarPartida", GetType().Name);
            if (!string.IsNullOrEmpty(viewId))
                scaModelos.TargetViewId = viewId;
            scaModelos.EmptyItemsBehavior = EmptyItemsBehavior.Deactivate;
        }

        /// <summary>
        ///  Generar la partida contable para la partida modelo que se recibe en el argumento oidModelo y para el <b>View.CurrentOBject</b>
        ///  el View.CurrentObject. Es necesario que la propiedad <b>TargetObjectType</b> del controller, sea igual a TipoBO de la partida modelo
        /// </summary>
        /// <param name="oidModelo">El ID de la partida modelo</param>
        /// <remarks>
        /// Esta implementación los parámetros de la partida modelo deben corresponder a propiedades del objet actual de la vista.
        /// Para recibir parámetros hay que re-implementar el método o sobrecargarlo
        /// </remarks>
        protected virtual void DoExecute(SingleChoiceActionExecuteEventArgs e)
        {
            IObjectSpace osPartida = Application.CreateObjectSpace(typeof(Partida));
            var modelo = scaModelos.SelectedItem.Data as PartidaModelo;
            if (!PuedeGenerarPartida(modelo.Oid, modelo, e))
                return;
            var partida = osPartida.CreateObject<Partida>();
            partida.Concepto = modelo.Concepto;
            partida.Tipo = modelo.Tipo;
            partida.Estado = EPartidaEstado.Registrada;
            //partida.Empresa = modelo.Empresa;
            if (!string.IsNullOrEmpty(modelo.PropiedadFecha))
                partida.Fecha = GetFechaPartida(modelo.PropiedadFecha) ?? DateTime.Now;
            object valor = null;
            CriteriaOperator condicion = null;
            foreach (var item in modelo.Detalles)
            {
                string[] paramsName = item.Criteria.Split(' ', StringSplitOptions.RemoveEmptyEntries).Where(x => x.StartsWith('?')).ToArray();
                condicion = CriteriaOperator.Parse(item.Criteria, GetParametersValue(paramsName));
                valor = osPartida.Evaluate(item.TipoBO, CriteriaOperator.Parse(item.Formula), condicion);
                if (valor == null || Convert.ToDecimal(valor) == 0)
                    continue;
                var detalle = osPartida.CreateObject<PartidaDetalle>();
                detalle.Partida = partida;
                detalle.Cuenta = osPartida.GetObjectByKey<Catalogo>(item.Cuenta.Oid);
                detalle.Concepto = Convert.ToString(osPartida.Evaluate(item.TipoBO, CriteriaOperator.Parse(item.Concepto), condicion)).Substring(0, 150);
                detalle.AjusteConsolidacion = ETipoOperacionConsolidacion.Ninguno;
                detalle.ValorDebe = (item.Tipo == ETipoOperacion.Cargo) ? Convert.ToDecimal(valor) : 0.0m;
                detalle.ValorHaber = (item.Tipo == ETipoOperacion.Abono) ? Convert.ToDecimal(valor) : 0.0m;
                detalle.Save();
                partida.Detalles.Add(osPartida.GetObject(detalle));
            }
            if (partida.Detalles.Count > 0)
            {
                osPartida.CommitChanges();
                var partidaDetalleView = Application.CreateDetailView(osPartida, partida, true);
                try
                {
                    Application.ShowViewStrategy.ShowViewInPopupWindow(partidaDetalleView, () =>
                    {
                        osPartida.CommitChanges();
                        // actualizar la propiedad Partida en el source cuando existe, para indicar el numero de partida que corresponde al documento
                        PropertyInfo propInfo = ObjectSpace.GetObjectType(e.CurrentObject).GetProperty("Partida");
                        if (propInfo != null)
                            propInfo.SetValue(e.CurrentObject, partida.Oid);
                        ObjectSpace.CommitChanges();
                        partidaDetalleView.Close();
                        osPartida.Dispose();
                    });
                }
                catch (Exception ex)
                {
                    Application.ShowViewStrategy.ShowMessage(ex.Message);
                    throw;
                }
            }
            else
                Application.ShowViewStrategy.ShowMessage($@"No hay datos en el origen {modelo.TipoBO.Name} para generar el asiento contable");
        }

        /// <summary>
        /// Validar que se puede generar la partida contable.
        /// </summary>
        /// <param name="oidModelo"></param>
        /// <param name="modelo"></param>
        /// <returns>True cuando es factible generar la partida contable</returns>
        /// <remarks>Pendiente de validar que el día este abierto, así como el período contable</remarks>
        private bool PuedeGenerarPartida(int oidModelo, PartidaModelo modelo, SingleChoiceActionExecuteEventArgs e)
        {
            if (modelo.Detalles.Count == 0)
            {
                Application.ShowViewStrategy.ShowMessage($@"La partida modelo con ID {oidModelo} no tiene detalles, no hay nada que generar");
                return false; //  no hay nada que hacer
            }
            if (ObjectSpace.IsNewObject(View.CurrentObject))
            {
                Application.ShowViewStrategy.ShowMessage($@"La transacción es nueva y aún no se ha guardado, no puede generar aún la partida contable");
                return false;
            }
            if (ObjectSpace.GetObjectType(View.CurrentObject) != modelo.Detalles.FirstOrDefault()?.TipoBO)
            {
                // REVISAR, porque en el detalle de la partida modelo, [TipoBO] de criteria puede ser diferente en cada registro. Esta
                // validación asume que todos son iguales y solo verifica el primer item del detalle (FistOrDefault). Una partida compleja
                // podría generarse a partir de 2 o más BO relacianados. Ejemplo: CompraFactura y el Pago de contado (BancoTransaccion)
                Application.ShowViewStrategy.ShowMessage($@"Tipo BO en la expresión no es igual al Tipo de BO del objeto actual");
                return false;
            }
            PropertyInfo propInfo = ObjectSpace.GetObjectType(e.CurrentObject).GetProperty("Partida");
            int oidPartida = Convert.ToInt32(propInfo.GetValue(e.CurrentObject));
            if (propInfo != null && oidPartida > 0)
            {
                Application.ShowViewStrategy.ShowMessage($@"NO se puede generar partida para documento seleccionado, porque ya existe una con Id {oidPartida}", 
                    InformationType.Error);
                return false;
            }
            // AGREGAR aquí validación de día y período abierto, antes de generar la partida
            return true;
        }

        /// <summary>
        ///  Retornar el arreglo con los valores de los parametros, que se obtienen del <b>View.CurrentObject</b>, que es el objeto
        ///  para el cual se va a generar la partida contable, por ejemplo para una partida de Ingreso por Venta es 
        ///  el documento de venta seleccionado para generar el asiento contable
        /// </summary>
        /// <param name="paramsName">arreglo con los nombres de los parámetros</param>
        /// <returns>Arreglo con los valores de los parámetros</returns>
        /// <remarks>
        ///  SUGERENCIA: Implementar validacion de los parametros, para identificar que existe la propiedad (de la cual toman el valor)
        ///  de esa manera se simplifica el codigo, porque no habria necesidad de testear si la propiedad existe por cada fila (lo cual además
        ///  tiene el inconveniente que cuando no existe retorna null para ese parametro).
        ///  La validación solo se ejecutaría una vez al inicio y después solo se obtiene su valor con GetMember
        /// </remarks>
        private object[] GetParametersValue(params string[] paramsName)
        {
            if (paramsName == null || paramsName.Length == 0)
                return null;
            object[] paramValues = new object[paramsName.Length];
            //XPClassInfo info = XpoDefault.Dictionary.GetClassInfo(View.CurrentObject);
            int idx = 0;
            foreach (string name in paramsName)
            {
                var propertyName = name[1..];
                XPMemberInfo memberInfo = (View.CurrentObject as XPBaseObject).ClassInfo.GetMember(propertyName);
                if (memberInfo != null)
                    paramValues[idx] = (View.CurrentObject as XPBaseObject).GetMemberValue(propertyName);
                else
                    paramValues[idx] = null;
                idx++;
            }
            return paramValues;
        }

        private DateTime? GetFechaPartida(string propiedadFecha)
        {
            XPMemberInfo memberInfo = (View.CurrentObject as XPBaseObject).ClassInfo.GetMember(propiedadFecha);
            return (memberInfo != null) ? Convert.ToDateTime((View.CurrentObject as XPBaseObject).GetMemberValue(propiedadFecha)) : null;
        }

        /// <summary>
        /// Retornar el concepto con los valores de las propiedades cuyos nombres estan en la cadena de texto identificados con ? al inicio
        /// </summary>
        /// <param name="concepto">El concepto con las propiedades identificadas con el caracter ? al inicio del nombre</param>
        /// <returns>El concepto para la linea de la partida contable con los valores de las propiedades</returns>
        private string NuevoConcepto(string concepto)
        {
            string[] propiedadesNombre = concepto.Split(' ', StringSplitOptions.RemoveEmptyEntries).Where(x => x.StartsWith('?')).ToArray();
            string valor = concepto;
            string propiedad;
            foreach (string item in propiedadesNombre)
            {
                propiedad = item.Substring(1);
                valor = valor.Replace(item, (View.CurrentObject as XPBaseObject).GetMemberValue(propiedad)?.ToString(), StringComparison.CurrentCulture);
            }
            return valor;
        }

    }
}
