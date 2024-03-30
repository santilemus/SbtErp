using DevExpress.Data.Filtering;
using DevExpress.Data.ODataLinq.Helpers;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Core;
using DevExpress.Pdf.Native.BouncyCastle.Asn1.X509;
using DevExpress.XtraRichEdit.Layout;
using SBT.Apps.Contabilidad.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        SingleChoiceAction opcionesModelo;  // revisar si esta se puede utilizar. Tomar en cuenta que en algunos casos se debe crear un popup para el ingreso de parámetros
        // ejemplo: si es una partida de ventas consolidada por día o período, es necesario poder ingresar: fecha desde y fecha hasta
        public PartidaAutoBaseController(): base()
        {
            // la siguiente linea se debe asignar en los controladores heredados
            //TargetObjectType = typeof(SBT.Apps.Facturacion.Module.BusinessObjects.Venta);
            // PENDIENTE. Ver como creamos las opciones de selección de partidas modelo permitidas para el BO, tomar en cuenta el numeral 3 de I D E A S.
            // una opción es evaluar SingleChoiseAction
        }

        private void DoGenerarPartida(int oidModelo, params object[] parameters)
        {
            using IObjectSpace os = Application.CreateObjectSpace(typeof(SBT.Apps.Contabilidad.Module.BusinessObjects.Partida));
            var modelo = os.FirstOrDefault<PartidaModelo>(x => x.Oid == oidModelo);
            if (modelo == null)
            {
                Application.ShowViewStrategy.ShowMessage($@"No se encontro la partida  modelo con Id {oidModelo} para generar la partida solicitada");
                return; // no hay nada que hacer
            }
            if (modelo.Detalles.Count == 0)
            {
                Application.ShowViewStrategy.ShowMessage($@"La partida modelo con ID {oidModelo} no tiene detalles, no hay nada que generar");
                return; //  no hay nada que hacer
            }
            
            var partida = os.CreateObject<Partida>();
            partida.Concepto = modelo.Concepto;
            partida.Tipo = modelo.Tipo;
            using IObjectSpace osDetalle = os.CreateNestedObjectSpace();
            foreach(var item in modelo.Detalles)
            {
                Type tipoBO = item.TipoBO;
                object valor = ObjectSpace.Evaluate(tipoBO.GetType(), CriteriaOperator.Parse(item.Formula), CriteriaOperator.Parse(item.Criteria, parameters));
                if (valor == null || Convert.ToDecimal(valor) == 0)
                    continue;
                var detallePtda = osDetalle.CreateObject<PartidaDetalle>();
                detallePtda.Cuenta = item.Cuenta;
                detallePtda.Concepto = item.Concepto;
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

        private void CreateOpcionesModelo()
        {
            IQueryable<PartidaModelo> queryModelo  = ObjectSpace.GetObjectsQuery<PartidaModelo>();
            //queryModelo.Where(x => x.Detalles.)
        }
    }
}
