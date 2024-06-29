using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBT.eFactura.Dte.Poco
{
    /// <summary>
    /// Detalle de los pagos aplicados al Dte
    /// </summary>
    public class Pagos
    {
        /// <summary>
        /// Código de la forma de pago
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("codigo")]
        [Length(2, 2)]
        [Required]
        public string? Codigo { get; set; }

        /// <summary>
        /// Monto por forma de pago
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("montoPago")]
        public decimal MontoPago { get; set; }

        /// <summary>
        /// Referencia de modalidad de pago
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("referencia")]
        [Length(1, 50)]
        public string? Referencia { get; set; }

        /// <summary>
        /// Plazo pactado para el pago cuando son operaciones al crédito
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("plazo")]
        [Length(2, 2)]
        public string? Plazo { get; set; }

        /// <summary>
        /// Período del plazo pactado para el pago cuando son operaciones al crédito
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("periodo")]
        public int Periodo { get; set; }
    }
}
