using DevExpress.ExpressApp.DC;

namespace SBT.Apps.Producto.Module.BusinessObjects
{
    /// <summary>
    /// Clasificación de los catálogos del sistema de transmisión de Dte.
    /// la enumeración está de acuerdo a la codificación de las tablas descritas en el documento técnico emitido por el MH
    /// </summary>
    public enum ETipoDteCatalogo
    {
        [XafDisplayName("CAT-001 Ambiente de destino")]
        AmbienteDestino = 1,
        [XafDisplayName("CAT-002 Tipo de Documento")]
        TipoDeDocumento = 2,
        [XafDisplayName("CAT-003 Modelo de Facturación")]
        ModeloFacturacion = 3,
        [XafDisplayName("CAT-004 Tipo de Transmisión")]
        TipoTransmision = 4,
        [XafDisplayName("CAT-005 Tipo de Contingencia")]
        TipoContingencia = 5,
        [XafDisplayName("CAT-006 Retención IVA MH")]
        RetencionIva = 6,
        [XafDisplayName("CAT-007 Tipo de Generación del Documento")]
        TipoGeneracionDoc = 7,
        [XafDisplayName("CAT-009 Tipo de establecimiento")]
        TipoEstablecimiento = 9,
        [XafDisplayName("CAT-010 Código tipo de Servicio (Médico)")]
        TipoServicioMedico = 10,
        [XafDisplayName("CAT-011 Tipo de ítem")]
        TipoItem = 11,
        [XafDisplayName("CAT-016 Condición de la Operación")]
        CondicionOperacion = 16,
        [XafDisplayName("CAT-017 Forma de Pago")]
        FormaPago = 17,
        [XafDisplayName("CAT-018 Plazo")]
        Plazo = 18,
        [XafDisplayName("CAT-021 Otros Documentos Asociados")]
        OtrosDocumentos = 21,
        [XafDisplayName("CAT-022 Tipo de documento de identificación del Receptor")]
        TipoDocumentoReceptor = 22,
        [XafDisplayName("CAT-023 Tipo de Documento en Contingencia")]
        TipoDocumentoContingencia = 23,
        [XafDisplayName("CAT-024 Tipo de Invalidación")]
        TipoInvalidacion = 24,
        [XafDisplayName("CAT-025 Título a que se remiten los bienes")]
        TituloRemisionBienes = 25,
        [XafDisplayName("CAT-026 Tipo de Donación")]
        TipoDonacion = 26,
        [XafDisplayName("CAT-027 Recinto fiscal")]
        RecintoFiscal = 27,
        [XafDisplayName("CAT-029 Tipo de persona ")]
        TipoPersona = 29,
        [XafDisplayName("CAT-030 Transporte")]
        Transporte = 30,
        [XafDisplayName("CAT-031 INCOTERMS")]
        Icoterms = 31,
        [XafDisplayName("CAT-032 Domicilio Fiscal")]
        DomicilioFiscal = 32
    }
}