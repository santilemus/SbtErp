using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBT.eFactura.Dte.Poco
{
    /// <summary>
    /// Resumen con los totales de la factura electrónica de sujeto excluído. Dte
    /// </summary>
    public class ResumenFSE
    {
        /// <summary>
        /// Total de Operaciones
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("totalCompra")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public decimal TotalCompra { get; set; }

        /// <summary>
        /// Monto global de Descuento, Bonificación, Rebajas y otros al total de operaciones.
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("descu")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public decimal Descuento { get; set; }

        /// <summary>
        /// Total del monto de Descuento, Bonificación, Rebajas
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("totalDescu")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public decimal TotalDescuento { get; set; }

        /// <summary>
        /// Subtotal
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("subTotal")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public decimal SubTotal { get; set; }

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
        /// Condición de la operación
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("condicionOperacion")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public int CondicionOperacion { get; set; }

        /// <summary>
        /// Pagos del Dte
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("pagos")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        public System.Collections.Generic.ICollection<Pago>? Pagos { get; set; }

        /// <summary>
        /// Observaciones
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("observaciones")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
        [MaxLength(3000)]
        public string? Observaciones { get; set; }


    }
}
