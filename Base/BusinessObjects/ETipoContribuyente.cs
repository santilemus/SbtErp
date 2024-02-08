namespace SBT.Apps.Base.Module.BusinessObjects
{
    /// <summary>
    /// Enumeracion con la clasificacion de los tipos de sujetos relacionado al pago de impuestos. 
    /// Esta clasificacion se utiliza para los Terceros
    /// 
    /// </summary>
    public enum ETipoContribuyente
    {
        Gravado = 0,
        /// <summary>
        /// TRANSFERENCIA DE BIENES Y SERVICIOS: Los Jefes de Misiones y/o Representantes de Misiones Diplomáticas y de 
        /// Organismos Internacionales, gozan del beneficio de exención de Impuestos, por la compra de bienes que realicen, 
        /// para materializar el goce del citado beneficio en cada compra que haga dentro del territorio Salvadoreño, 
        /// deberan  acreditar su situación de sujeto exento mediante la exhibición del Carnet de Exención respectivo y 
        /// solicitar factura de consumidor final bajo el concepto de ventas exentas
        /// LOS PROVEEDORES DE BIENES Y SERVICIOS: Que realicen ventas a Personeros de Misiones Diplomáticas y Organismos 
        /// Internacionales debidamente acreditados en el país,que gocen de tal exención, deberan proceder a emitir factura 
        /// de consumidor final bajo el concepto de ventas exentas, conforme a lo establecido en el artículo 114, literal b) 
        /// numeral 3) del Código Tributario
        /// Mas Info: http://catlegal.mh.gob.sv/index.php/consultas/2344-carnet-a-diplomaaacuteticos
        /// Excluido de la calidad de contribuyente Art. 119 CT
        /// Mas Info: https://elsalvador.eregulations.org/media/Codigo%20Tributario.pdf
        /// </summary>
        Exento = 1
    }
}
