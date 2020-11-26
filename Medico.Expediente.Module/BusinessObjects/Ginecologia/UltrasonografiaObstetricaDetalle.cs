using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using SBT.Apps.Base.Module.BusinessObjects;

namespace SBT.Apps.Medico.Expediente.Module.BusinessObjects.Ginecologia
{
    [DefaultClassOptions, ModelDefault("Caption", "Ultrasonografía Obstetrica - Detalle"), 
        NavigationItem(false), CreatableItem(false), Persistent(nameof(UltrasonografiaObstetricaDetalle))]
    //[ImageName("BO_Contact")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class UltrasonografiaObstetricaDetalle : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public UltrasonografiaObstetricaDetalle(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        decimal pF;
        decimal espesorPlacenta;
        int fcf;
        string pbf;
        DateTime fpp;
        decimal reaccionDesidual;
        decimal ila;
        int eg;
        SBT.Apps.Base.Module.BusinessObjects.TipoGenero genero;
        decimal vv;
        decimal lf;
        decimal cc;
        int dms;
        decimal ac;
        decimal dbp;
        decimal crl;
        EGradoPlacenta gradoPlacenta;
        EFormaPlacentaPrevia formaPlacentaPrevia;
        EPlacentraFetoUltra placenta;
        EDorsoFetoUltra dorso;
        EPosicionFetoUltra posición;
        EPresentacionFetoUltra presentación;
        ESituacionFetoUltra situacion;
        UltraSonografiaObstetrica ultrasonografia;

        [Association("UltrasonografiaObstetrica-Detalles")]
        public UltraSonografiaObstetrica Ultrasonografia
        {
            get => ultrasonografia;
            set => SetPropertyValue(nameof(Ultrasonografia), ref ultrasonografia, value);
        }

        [DbType("smallint"), XafDisplayName("Situación")]
        public ESituacionFetoUltra Situacion
        {
            get => situacion;
            set => SetPropertyValue(nameof(Situacion), ref situacion, value);
        }

        [DbType("smallint"), XafDisplayName("Presentación")]
        public EPresentacionFetoUltra Presentación
        {
            get => presentación;
            set => SetPropertyValue(nameof(Presentación), ref presentación, value);
        }

        [DbType("smallint"), XafDisplayName("Posición")]
        public EPosicionFetoUltra Posición
        {
            get => posición;
            set => SetPropertyValue(nameof(Posición), ref posición, value);
        }

        [DbType("smallint"), XafDisplayName("Dorso")]
        public EDorsoFetoUltra Dorso
        {
            get => dorso;
            set => SetPropertyValue(nameof(Dorso), ref dorso, value);
        }

        [DbType("smallint"), XafDisplayName("Placenta")]
        public EPlacentraFetoUltra Placenta
        {
            get => placenta;
            set => SetPropertyValue(nameof(Placenta), ref placenta, value);
        }

        [DbType("smallint"), XafDisplayName("Forma Placenta Previa")]
        public EFormaPlacentaPrevia FormaPlacentaPrevia
        {
            get => formaPlacentaPrevia;
            set => SetPropertyValue(nameof(FormaPlacentaPrevia), ref formaPlacentaPrevia, value);
        }

        [DbType("smallint"), XafDisplayName("Grado Placenta")]
        public EGradoPlacenta GradoPlacenta
        {
            get => gradoPlacenta;
            set => SetPropertyValue(nameof(GradoPlacenta), ref gradoPlacenta, value);
        }

        [DbType("numeric(10,2)"), XafDisplayName("Espesor Placentario (mm)"),
            ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal EspesorPlacenta
        {
            get => espesorPlacenta;
            set => SetPropertyValue(nameof(EspesorPlacenta), ref espesorPlacenta, value);
        }

        #region Biometria
        /// <summary>
        /// Distancia de coronilla a coxis (es lo mismo que LCN)
        /// </summary>
        [DbType("numeric(10,2)"), XafDisplayName("CRL (mm)"), VisibleInListView(false), 
            ToolTip("Distancia de coronilla a coxis (es lo mismo que LCN)")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal Crl
        {
            get => crl;
            set => SetPropertyValue(nameof(Crl), ref crl, value);
        }

        /// <summary>
        /// Diámetro biparietal, de un lado a otro de la cabeza, expresado en milímetros
        /// </summary>
        [DbType("numeric(10,2)"), XafDisplayName("DBP (mm)"), VisibleInListView(false),
            ToolTip("Diámetro biparietal, de un lado a otro de la cabeza")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal Dbp
        {
            get => dbp;
            set => SetPropertyValue(nameof(Dbp), ref dbp, value);
        }

        /// <summary>
        /// perímetro o circunferencia abdominal. Es lo mismo que CA en español
        /// </summary>
        [DbType("numeric(10,2)"), XafDisplayName("AC (mm)"), VisibleInListView(false)]
        [ToolTip("Perímetro o circunferencia abdominal. Es lo mismo que CA en español")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal Ac
        {
            get => ac;
            set => SetPropertyValue(nameof(Ac), ref ac, value);
        }

        /// <summary>
        /// Diametro medio del saco (DMS), también se conoce como SG (Saco Gestacional)
        /// </summary>
        [DbType("numeric(10,2)"), XafDisplayName("SG (mm)"), VisibleInListView(false)]
        [ToolTip("Diametro medio del saco (DMS), también se conoce como SG (Saco Gestacional)")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public int Dms
        {
            get => dms;
            set => SetPropertyValue(nameof(Dms), ref dms, value);
        }

        /// <summary>
        /// Frecuencia Cardíaca Fetal
        /// </summary>
        [DbType("smallint"), XafDisplayName("Frecuencia Cardíaca (x min.)"), VisibleInListView(false)]
        public int Fcf
        {
            get => fcf;
            set => SetPropertyValue(nameof(Fcf), ref fcf, value);
        }

        /// <summary>
        /// Corresponde al perímetro de la cabeza, siempre en milímetros. Es lo mismo que HC en Ingles
        /// </summary>
        [DbType("numeric(10,2)"), XafDisplayName("HC (mm)"), VisibleInListView(false)]
        [ToolTip("Corresponde al perímetro de la cabeza, siempre en milímetros. Es lo mismo que HC en Ingles")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal Cc
        {
            get => cc;
            set => SetPropertyValue(nameof(Cc), ref cc, value);
        }

        /// <summary>
        /// longitud del fémur expresada en milímetros. Es mismo que FL en Inglés
        /// </summary>
        [DbType("Longitud del fémur expresada en milímetros. Es mismo que FL en Inglés")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal Lf
        {
            get => lf;
            set => SetPropertyValue(nameof(Lf), ref lf, value);
        }

        /// <summary>
        /// Vena umbilical
        /// </summary>
        [DbType("numeric(10,2)"), XafDisplayName("Vena Umbilical (mm)")]
        [ToolTip("Vena umbilical")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal Vv
        {
            get => vv;
            set => SetPropertyValue(nameof(Vv), ref vv, value);
        }

        [DbType("numeric(10,2)"), XafDisplayName("Reacción Desidual")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal ReaccionDesidual
        {
            get => reaccionDesidual;
            set => SetPropertyValue(nameof(ReaccionDesidual), ref reaccionDesidual, value);
        }

        #endregion
        /// <summary>
        /// Indice de liquido amniótico
        /// </summary>
        [DbType("numeric(10,2)"), XafDisplayName("Índice Líquido Amniótico (mm)"), VisibleInListView(false)]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal Ila
        {
            get => ila;
            set => SetPropertyValue(nameof(Ila), ref ila, value);
        }

        /// <summary>
        /// Edad Gestacional en semanas
        /// </summary>
        [DbType("smallint"), XafDisplayName("Edad Gestacional (semanas)"), VisibleInListView(false)]
        public int Eg
        {
            get => eg;
            set => SetPropertyValue(nameof(Eg), ref eg, value);
        }

        /// <summary>
        /// Tipo de genero del bebe o nonato
        /// </summary>
        [DbType("smallint"), XafDisplayName("Genero"), VisibleInDetailView(false)]
        public TipoGenero Genero
        {
            get => genero;
            set => SetPropertyValue(nameof(Genero), ref genero, value);
        }

        /// <summary>
        /// Fecha Probable de Parto
        /// </summary>
        [DbType("datetime2"), XafDisplayName("Fecha Probable de Parto"), VisibleInListView(false)]
        public DateTime Fpp
        {
            get => fpp;
            set => SetPropertyValue(nameof(Fpp), ref fpp, value);
        }

        /// <summary>
        /// Ponderado Fetal
        /// PF = (AU - n) x 155 +- 100 gramos
        /// AU = Altura Uterina
        /// n = 11 si la presentacion esta encajada y n = 12 si la presentacion aun no esta encajada
        /// La altura uterina corresponde a la distancia entre la sínfisis pubiana (una de las articulaciones de la pelvis, 
        /// situada a la altura del vello púbico) y el fondo uterino (la parte más alta del útero), ambos determinados por 
        /// palpación y se usa una cinta metrica
        /// </summary>
        [DbType("numeric(10,2)"), XafDisplayName("Ponderado Fetal"), ToolTip("Ponderado Fetal")]
        [ModelDefault("DisplayFormat", "{0:N2}"), ModelDefault("EditMask", "n2")]
        public decimal PF
        {
            get => pF;
            set => SetPropertyValue(nameof(PF), ref pF, value);
        }

        /// <summary>
        /// Perfil Biofísico Fetal
        /// El análisis combina el monitoreo de la frecuencia cardíaca fetal (prueba sin esfuerzo) y la ecografía fetal 
        /// para evaluar la frecuencia cardíaca, la respiración, los movimientos, el tono muscular y el nivel de líquido 
        /// amniótico del bebé
        /// </summary>
        [Size(50), DbType("varchar(50)"), XafDisplayName("Perfil Biofísico Fetal"), VisibleInListView(false)]
        public string Pbf
        {
            get => pbf;
            set => SetPropertyValue(nameof(Pbf), ref pbf, value);
        }

        #endregion

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}


    }

    public enum ESituacionFetoUltra
    {
        Longitudinal = 0,
        Transverso = 1,
        Oblicuo = 2
    }

    public enum EPresentacionFetoUltra
    {
        Cefalico = 0,
        Podalico = 1

    }

    public enum EPosicionFetoUltra
    {
        Izquierda = 0,
        Derecha = 1
    }

    public enum EDorsoFetoUltra
    {
        Lateral = 0,
        Anterior = 1,
        Posterior = 2
    }

    public enum EPlacentraFetoUltra
    {
        Anterior = 0,
        Posterior = 2,
        Previa = 3,
        Fundica = 4
    }

    public enum EFormaPlacentaPrevia
    {
        Marginal = 0,
        Parcial = 1,
        Completa = 2
    }

    public enum EGradoPlacenta
    {
        I = 0,
        II = 1,
        III = 2
    }

}