using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;


namespace SBT.Apps.Base.Module.BusinessObjects
{
    /// <summary>
    /// Implementación de los métodos para el objeto de negocios UnidadMedida, que implementa el mantenimiento correspondiente
    /// </summary>
    [ModelDefault("Caption", "Unidad de Medida"), NavigationItem("Catalogos"), Persistent("UnidadMedida"), XafDefaultProperty("Nombre")]
    [ImageName("UnidadMedida")]
    public class UnidadMedida : XPObject
    {
        public UnidadMedida(Session session) : base(session) { }

        /// <summary>
        /// Evento (delegate) que implementa la inicialización de las propiedades del BO, cuando se crea uno nuevo
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            TipoSistema = TipoSistemaMedida.Internacional;
            TipoUnidad = TipoUnidadMedida.Longitud;
            Magnitud = 1.00m;
            Activo = true;
        }

        public decimal? CelciusToFahrenheit(decimal ACelcius)
        {
            if (this.TipoUnidad == TipoUnidadMedida.Temperatura)
                return ACelcius * 9.00m / 5.00m + 32.00m;
            else
                return null;
        }

        public Decimal? FahrenheitToCelcius(decimal AFahrenheit)
        {
            if (this.TipoUnidad == TipoUnidadMedida.Temperatura)
                return (AFahrenheit - 32.00m) * 5.00m / 9.00m;
            else
                return null;
        }

        public Decimal? CelciusToKelvin(decimal ACelcius)
        {
            if (this.TipoUnidad == TipoUnidadMedida.Temperatura)
                return ACelcius + 273.15m;
            else
                return null;

        }

        public decimal? KelvinToCelcius(decimal AKelvin)
        {
            if (this.TipoUnidad == TipoUnidadMedida.Temperatura)
                return AKelvin - 273.15m;
            else
                return null;
        }

        #region Propiedades
        string codigoDte;
        TipoSistemaMedida fTipoSistema;
        TipoUnidadMedida fTipoUnidad;
        string fNombre;
        decimal fMagnitud;
        string fSimbolo;
        bool fActivo;

        [RuleRequiredField("UnidadMedida.Oid_requerido", DefaultContexts.Save)]
        public string CodigoDte
        {
            get => codigoDte;
            set => SetPropertyValue<string>(nameof(CodigoDte), ref codigoDte, value);
        }

        /// <summary>
        /// Tipo de Sistema de la unidad de medida. Lista de valores puede ser:
        /// 1 = Sistema Internacional
        /// 2 = Sistema Anglosajon
        /// </summary>
        [Persistent(@"TipoSistema")]
        [DbType("Smallint")]
        [DevExpress.Xpo.DisplayName(@"Tipo Sistema")]
        public TipoSistemaMedida TipoSistema
        {
            get => fTipoSistema;
            set => SetPropertyValue(nameof(TipoSistema), ref fTipoSistema, value);
        }


        /// <summary>
        /// Tipo de unidad de medida de acuerdo al sistema
        /// </summary>
        [Persistent(@"TipoUnidad")]
        [DbType("Smallint")]
        [DevExpress.Xpo.DisplayName(@"Tipo Unidad")]
        public TipoUnidadMedida TipoUnidad
        {
            get => fTipoUnidad;
            set => SetPropertyValue(nameof(TipoUnidad), ref fTipoUnidad, value);
        }

        /// <summary>
        /// Nombre de la unidad de medida
        /// </summary>
        [Size(60)]
        [Persistent(@"Nombre")]
        [DbType("varchar(60)")]
        [RuleRequiredField("UnidadMedida.Nombre_requerido", DefaultContexts.Save, "El nombre de la unidad de medida es requerido")]
        public string Nombre
        {
            get { return fNombre; }
            set { SetPropertyValue<string>(nameof(Nombre), ref fNombre, value); }
        }

        /// <summary>
        /// Magnitud de la unidad de medida con respecto a la definida como medida base (tiene valor de 1.0) en cualquiera de los sistemas. Sirve para realizar las conversiones
        /// </summary>
        [Persistent(@"Magnitud")]
        [DbType("numeric(22,11)")]
        [ModelDefault("DisplayFormat", "{0:#,##0.00########;(#,##0.00########)}"),
         RuleRequiredField("UnidadMedida.Magnitud_requerido", DefaultContexts.Save, "La Magnitud es requerida")]
        public decimal Magnitud
        {
            get { return fMagnitud; }
            set { SetPropertyValue<decimal>("Magnitud", ref fMagnitud, value); }
        }

        /// <summary>
        /// Símbolo utilizado para la unidad de medida
        /// </summary>
        [Persistent(@"Simbolo")]
        [DbType("varchar(3)")]
        [DisplayName(@"Símbolo")]
        [RuleRequiredField("UnidadMedida.Simbolo_requerido", DefaultContexts.Save, "El símbolo para la unidad de medida es requerido")]
        public string Simbolo
        {
            get { return fSimbolo; }
            set { SetPropertyValue<string>("Simbolo", ref fSimbolo, value); }
        }

        /// <summary>
        /// Indica sí la unidad de medida se encuentra vigente o activa
        /// </summary>
        [Persistent(@"Activo")]
        [DbType("bit")]
        public bool Activo
        {
            get { return fActivo; }
            set { SetPropertyValue<bool>("Activo", ref fActivo, value); }
        }

        #endregion 
    }

}
