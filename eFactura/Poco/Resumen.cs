using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBT.eFactura.Dte.Poco
{
    /// <summary>
    /// Resumen con los totales del Dte
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.8.0.0 (Newtonsoft.Json v13.0.3.0)")]
    public class Resumen
    {
        /// <summary>
        /// Total de Operaciones no sujetas
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("totalNoSuj")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public decimal TotalNoSujeta { get; set; }

        /// <summary>
        /// Total de Operaciones exentas
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("totalExenta")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public decimal TotalExenta { get; set; }

        /// <summary>
        /// Total de Operaciones gravadas
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("totalGravada")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public decimal TotalGravada { get; set; }

        /// <summary>
        /// Suma de operaciones sin impuestos
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("subTotalVentas")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public decimal SubTotalVentas { get; set; }

        /// <summary>
        /// Monto global de Descuento, Bonificación, Rebajas y otros a ventas no sujetas
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("descuNoSuj")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public decimal DescuentoNoSujeta { get; set; }

        /// <summary>
        /// Monto de Descuento, Bonificación, Rebajas y otros a ventas exentas
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("descuExenta")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public decimal DescuentoExenta { get; set; }

        /// <summary>
        /// Monto de Descuento, Bonificación, Rebajas y otros a ventas gravadas
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("descuGravada")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public decimal DescuentoGravada { get; set; }

        /// <summary>
        /// Porcentaje del monto global de Descuento, Bonificación, Rebajas y otros
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("porcentajeDescuento")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public decimal PorcentajeDescuento { get; set; }

        /// <summary>
        /// Total del monto de Descuento, Bonificación, Rebajas
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("totalDescu")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public decimal TotalDescuento { get; set; }

        /// <summary>
        /// Resumen de Tributos
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("tributos")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public System.Collections.Generic.ICollection<ResumenTributos>? Tributos { get; set; }

        /// <summary>
        /// Subtotal
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("subTotal")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public decimal SubTotal { get; set; }

        /// <summary>
        /// Iva Percibido
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("ivaPerci1")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public decimal IvaPercibido { get; set; }

        /// <summary>
        /// Iva Retenido
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("ivaRete1")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public decimal IvaRetenido { get; set; }

        /// <summary>
        /// Retención Renta
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("reteRenta")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public decimal ReteRenta { get; set; }

        /// <summary>
        /// Monto Total de la Operación
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("montoTotalOperacion")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public decimal MontoTotalOperacion { get; set; }

        /// <summary>
        /// Total Cargos/Abonos que no afectan la base imponible
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("totalNoGravado")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public decimal TotalNoGravado { get; set; }

        /// <summary>
        /// Total a pagar
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("totalPagar")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public decimal TotalPagar { get; set; }

        /// <summary>
        /// Valor en letras
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("totalLetras")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public string? TotalLetras { get; set; }

        /// <summary>
        /// Saldo a favor
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("saldoFavor")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public decimal SaldoFavor { get; set; }

        /// <summary>
        /// Condición de la operación
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("condicionOperacion")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public int CondicionOperacion { get; set; }

        /// <summary>
        /// Pagos del Dte
        /// </summary>
        /// <remarks>
        /// Para las notas de credito y otros documentos no va, pero se intentara utilizar esta clase. Si hay problemas se tendra que hacer una
        /// especifica para las notas de credito
        /// </remarks>
        [System.Text.Json.Serialization.JsonPropertyName("pagos")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public System.Collections.Generic.ICollection<Pago>? Pagos { get; set; }

        /// <summary>
        /// Número de Pago Electrónico
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("numPagoElectronico")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [Length(0, 100)]
        public string? NumeroPagoElectronico { get; set; }

    }
}
