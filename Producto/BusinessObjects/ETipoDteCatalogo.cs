using DevExpress.ExpressApp.DC;

namespace SBT.Apps.Producto.Module.BusinessObjects
{
    /// <summary>
    /// Clasificación de los catálogos del sistema de transmisión de Dte.
    /// la enumeración está de acuerdo a la codificación de las tablas descritas en el documento técnico emitido por el MH
    /// </summary>
    public enum ETipoDteCatalogo
    {
        // CAT-002 Tipo de Documento está en Listas, se agrego columna CodigoAlterno
        // CAT-003 Modelo de Facturación es Enum EModeloFacturacion
        // CAT-004 Tipo de Transmisión es ETipoTransmision
        [XafDisplayName("CAT-005 Tipo de Contingencia")]
        TipoContingencia = 5,
        [XafDisplayName("CAT-006 Retención IVA MH")]
        RetencionIva = 6,
        // CAT-007 Tipo de Generación del Documento es ETipoGeneracionDocumento
        [XafDisplayName("CAT-009 Tipo de establecimiento")]
        TipoEstablecimiento = 9,
        [XafDisplayName("CAT-010 Código tipo de Servicio (Médico)")]
        TipoServicioMedico = 10,
        [XafDisplayName("CAT-011 Tipo de ítem")]
        TipoItem = 11,
        // CAT-014 Unidad de Medida, BO UnidadMedida
        // CAT-015 Tributos se agrego columna CodigoDte en la tabla Tributo (BO Tributo). Falta modificar los datos
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
        // CAT-026 Tipo de Donación es ETipoDonacion
        [XafDisplayName("CAT-027 Recinto fiscal")]
        RecintoFiscal = 27,
        // CAT-029 Tipo Persona es enumeracion TipoPersona (que ya existia en el sistema)
        [XafDisplayName("CAT-030 Transporte")]
        Transporte = 30,
        [XafDisplayName("CAT-031 INCOTERMS")]
        Icoterms = 31
        /// CAT-032 Domicilio Fiscal es enum EDomicilioFiscal
    }

    /// <summary>
    /// CAT-007 Tipo de Generación del Documento
    /// </summary>
    public enum ETipoGeneracionDocumento
    {
        [XafDisplayName("Físico")]
        Fisico = 1,
        [XafDisplayName("Electrónico")]
        Electronico = 2
    }

    /// <summary>
    /// CAT-003 Modelo de Facturación del catálogo sistema de transmisión
    /// </summary>
    public enum EModeloFacturacion
    {
        [XafDisplayName("Modelo Facturación previo")]
        Previo = 1,
        [XafDisplayName("Modelo Facturación diferido")]
        Diferido = 2
    }

    public enum ETipoTransmision
    {
        [XafDisplayName("Transmisión normal")]
        Normal = 1,
        [XafDisplayName("Transmisión por contingencia")]
        Contingencia = 2
    }

    public enum ETipoDonacion
    {
        Efectivo = 1,
        Bien = 2,
        Servicio = 3
    }

    /// <summary>
    /// Enumeracion que corresponde al CAT-032 Domicilio Fiscal de Catálogos Sistema de Transmisión
    /// </summary>
    public enum EDomicilioFiscal
    {
        Domiciliado = 1,
        [XafDisplayName("No Domiciliado")]
        NoDomiciliado = 2
    }
}