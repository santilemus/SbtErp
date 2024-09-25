using DevExpress.Data.Filtering;
using DevExpress.Data.ODataLinq.Helpers;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Security;
using Microsoft.Extensions.DependencyInjection;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Contabilidad.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.DC;

namespace SBT.Apps.Erp.Module.Controllers
{
    /// <summary>
    /// View Controller para probar mecanismo de generación de Partidas automáticas desde otros módulos y utilizando mecanismo de partidas modelo
    /// Si funciona como se requiere debe ser el controlador base para implementar la funcionalidad generica de generación de partidas contables
    /// de las transacciones en diferentes módulos
    /// </summary>
    /// <remarks>
    /// I D E A S
    /// 1. Es el controlador base, implementar aquí la funcionalidad común o genérica
    /// 2. Heredar de este controlador para implementar la funcionalidad específica de las transacciones (o documentos) que generan asientos contables
    /// 3. Implementar aquí filtrado de las partidas modelo que aplican para las transacciones (o documentos) de acuerdo al TargetObjectType
    /// 4. Evaluar implementar partidas automáticas. Es decir, que se generen cuando se guarda la transacción (ver como funcionaría la modificación
    /// 5. Será  necesario identificar las transacciones para las cuales ya se genero el asiento contable. Idealmente en lugar de un Estado debería de
    ///    registrarse el número de asiento contable que se genero.
    /// 6. Será necesario identificar las partidas generadas con este mecanismo (automáticas o no) para obligar a su revisión antes de aplicarlas
    ///    en la contabilidad. Esto obligará a revisar el proceso de cierre y apertura, aplicando en el cierre solo los asientos contables que han sido
    ///    marcados como revisados.
    /// 7. Será necesario evaluar como proceder cuando se modifica la transacción que dió origen a la partida (factura de venta por ejemplo). Se traslada
    ///    la modificación a la partida de forma automática (que sucede si ya fue cerrado el día o mes), o el contador debe realizar un asiento de diario
    ///    para aplicar el ajuste.
    /// </remarks>
    public class PartidaAutoBaseController: ViewController
    { 
        int empresaOid;
        SingleChoiceAction scaModelos;  // revisar si esta se puede utilizar. Tomar en cuenta que en algunos casos se debe crear un popup para el ingreso de parámetros
        // ejemplo: si es una partida de ventas consolidada por día o período, es necesario poder ingresar: fecha desde y fecha hasta
        public PartidaAutoBaseController(): base()
        {
            // la siguiente linea se debe asignar en los controladores heredados
            //TargetObjectType = typeof(SBT.Apps.Facturacion.Module.BusinessObjects.Venta);
            // PENDIENTE. Ver como creamos las opciones de selección de partidas modelo permitidas para el BO, tomar en cuenta el numeral 3 de I D E A S.
            // una opción es evaluar SingleChoiseAction
        }

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

        /// <summary>
        ///  Generar la partida contable para la partida modelo que se recibe en el argumento oidModelo y para el <b>View.CurrentOBject</b>
        ///  el View.CurrentObject. Es necesario que la propiedad <b>TargetObjectType</b> del controller, sea igual a TipoBO de la partida modelo
        /// </summary>
        /// <param name="oidModelo">El ID de la partida modelo</param>
        /// <remarks>
        /// Esta implementación los parámetros de la partida modelo deben corresponder a propiedades del objet actual de la vista.
        /// Para recibir parámetros hay que re-implementar el método o sobrecargarlo
        /// </remarks>
        protected virtual void DoGenerarPartida(int oidModelo)
        {
            using IObjectSpace os = Application.CreateObjectSpace(typeof(SBT.Apps.Contabilidad.Module.BusinessObjects.Partida));
            var modelo = os.FirstOrDefault<PartidaModelo>(x => x.Oid == oidModelo);
            if (!PuedeGenerarPartida(oidModelo, modelo))
                return;
            var partida = os.CreateObject<Partida>();
            partida.Concepto = modelo.Concepto;
            partida.Tipo = modelo.Tipo;
            using IObjectSpace osDetalle = os.CreateNestedObjectSpace();
            object valor = null;
            CriteriaOperator condicion = null;
            foreach(var item in modelo.Detalles)
            {
                Type tipoBO = item.TipoBO;
                string criteria = item.Criteria.Replace("?EmpresaOid", Convert.ToString(EmpresaOid));
                string[] paramsName = criteria.Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Where(x => x.StartsWith('?')).ToArray();
                condicion = CriteriaOperator.Parse(criteria, GetParametersValue(paramsName));
                valor = ObjectSpace.Evaluate(tipoBO.GetType(), CriteriaOperator.Parse(item.Formula), condicion);
                if (valor == null || Convert.ToDecimal(valor) == 0)
                    continue;
                var detallePtda = osDetalle.CreateObject<PartidaDetalle>();
                detallePtda.Cuenta = item.Cuenta;
                detallePtda.Concepto = Convert.ToString(ObjectSpace.Evaluate(tipoBO.GetType(), CriteriaOperator.Parse(item.Concepto), null));
                //detallePtda.Concepto = NuevoConcepto(item.Concepto);
                
                detallePtda.ValorDebe = 0.0m;
                detallePtda.ValorHaber = 0.0m;
                detallePtda.AjusteConsolidacion = ETipoOperacionConsolidacion.Ninguno;
                if (item.Tipo == Base.Module.BusinessObjects.ETipoOperacion.Cargo)
                    detallePtda.ValorDebe = Convert.ToDecimal(valor);
                else if (item.Tipo == Base.Module.BusinessObjects.ETipoOperacion.Abono)
                    detallePtda.ValorHaber = Convert.ToDecimal(valor);
                partida.Detalles.Add(detallePtda);
            }
            if (partida.Detalles.Count > 0)
            {
                partida.Save();
                os.CommitChanges();
            }
            else
                Application.ShowViewStrategy.ShowMessage($@"La partida modelo con ID {oidModelo} no hay datos en el origen para generar el asiento contable");
        }

        /// <summary>
        /// Validar que se puede generar la partida contable.
        /// </summary>
        /// <param name="oidModelo"></param>
        /// <param name="modelo"></param>
        /// <returns>True cuando es factible generar la partida contable</returns>
        /// <remarks>Pendiente de validar que el día este abierto, así como el período contable</remarks>
        private bool PuedeGenerarPartida(int oidModelo, PartidaModelo modelo)
        {
            if (modelo == null)
            {
                Application.ShowViewStrategy.ShowMessage($@"No se encontro la partida  modelo con Id {oidModelo} para generar la partida solicitada");
                return false; // no hay nada que hacer
            }
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
            if (ObjectSpace.GetObjectType(View.CurrentObject) != modelo.Detalles.FirstOrDefault<PartidaModeloDetalle>()?.TipoBO)
            {
                // REVISAR, porque en el detalle de la partida modelo, [TipoBO] de criteria puede ser diferente en cada registro. Esta
                // validación asume que todos son iguales y solo verifica el primer item del detalle (FistOrDefault). Una partida compleja
                // podría generarse a partir de 2 o más BO relacianados. Ejemplo: CompraFactura y el Pago de contado (BancoTransaccion)
                Application.ShowViewStrategy.ShowMessage($@"Tipo BO en la expresión no es igual al Tipo de BO del objeto actual");
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

        /// <summary>
        /// Retornar el concepto con los valores de las propiedades cuyos nombres estan en la cadena de texto identificados con ? al inicio
        /// </summary>
        /// <param name="concepto">El concepto con las propiedades identificadas con el caracter ? al inicio del nombre</param>
        /// <returns>El concepto para la linea de la partida contable con los valores de las propiedades</returns>
        private string NuevoConcepto(string concepto)
        {
            string[] propiedadesNombre = concepto.Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Where(x => x.StartsWith('?')).ToArray();
            string valor = concepto;
            string propiedad;
            foreach (string item in propiedadesNombre)
            {
                propiedad = item.Substring(1);
                valor = valor.Replace(item, (View.CurrentObject as XPBaseObject).GetMemberValue(propiedad)?.ToString(), StringComparison.CurrentCulture);
            }
            return valor;
        }

        private void CreateOpcionesModelo(int oidEmpresa)
        {
            IQueryable<PartidaModeloDetalle> queryModelo  = ObjectSpace.GetObjectsQuery<PartidaModeloDetalle>();
            var listaModelo = queryModelo.Where(x => x.TipoBO.GetType().Name == "SBT.Apps.Facturacion.Module.BusinessObjects.Venta"
                                                        && x.PartidaModelo.Empresa.Oid == oidEmpresa).
                                                        Select(y => new { y.PartidaModelo.Oid, y.PartidaModelo.Nombre, y.Tipo, y.TipoBO }).ToList();
            scaModelos = new SingleChoiceAction(this, "scaPartidaModelo", DevExpress.Persistent.Base.PredefinedCategory.RecordEdit);
            scaModelos.TargetObjectType = this.TargetObjectType;
            foreach(var modelo in listaModelo)
            {
                scaModelos.Items.Add(new ChoiceActionItem(modelo.Nombre, modelo.Oid));
            }           
        }
    }
}
