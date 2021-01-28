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

namespace SBT.Apps.Medico.Generico.Module.BusinessObjects
{
    /// <summary>
    /// BO que corresponde a los datos generales de las tablas de percentiles de crecimiento
    /// </summary>
    /// <remarks>
    /// Mas informacion en
    /// https://www.who.int/tools
    /// Para niños
    /// https://apps.who.int/iris/bitstream/handle/10665/43601/9789241595070_BoysGrowth_eng.pdf?sequence=9&isAllowed=y
    /// https://apps.who.int/iris/bitstream/handle/10665/43601/9789241595070_GirlsGrowth_eng.pdf?sequence=10&isAllowed=y
    /// https://spa.kyhistotechs.com/using-lms-method-calculate-z-scores-51993705
    /// </remarks>
    [DefaultClassOptions, ModelDefault("Caption", "Percentil"), NavigationItem(false), CreatableItem(false),
        DefaultProperty(nameof(Nombre)), Persistent(nameof(PercentilTabla))]
    //[ImageName("BO_Contact")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class PercentilTabla : XPObjectBaseBO
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public PercentilTabla(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region Propiedades

        Signo signo;
        string nombre;
        TipoGenero genero;

        [DbType("smallint"), XafDisplayName("Genero"), Index(0)]
        public TipoGenero Genero
        {
            get => genero;
            set => SetPropertyValue(nameof(Genero), ref genero, value);
        }


        [Size(60), DbType("varchar(60)"), XafDisplayName("Nombre"), Index(1)]
        public string Nombre
        {
            get => nombre;
            set => SetPropertyValue(nameof(Nombre), ref nombre, value);
        }

        
        [XafDisplayName("Signo"), Index(2)]
        public Signo Signo
        {
            get => signo;
            set => SetPropertyValue(nameof(Signo), ref signo, value);
        }

        #endregion

        #region Colecciones
        [Association("PercentilTabla-Detalles"), DevExpress.Xpo.Aggregated, XafDisplayName("Detalles"), Index(0)]
        public XPCollection<PercentilTablaDetalle> Detalles
        {
            get
            {
                return GetCollection<PercentilTablaDetalle>(nameof(Detalles));
            }
        }
        #endregion


        #region Metodos
        /// <summary>
        /// Retornar el valor de la columna percentil cuyo nombre se recibe en el parametro ColumnaPercentil y para la Edad del parametro
        /// </summary>
        /// <param name="Edad">Edad en Meses del percentil a retornar</param>
        /// <param name="ColumnaPercentil">Nombre de la columna percentil cuyo valor se a retornar</param>
        /// <returns>El valor de la columna percentil cuyo nombre se recibe en el parametro</returns>
        public decimal ObtenerPercentil(int edadMeses, string columnaPercentil)
        {          
            PercentilTablaDetalle pd = Detalles.FirstOrDefault<PercentilTablaDetalle>(x => x.EdadMes == edadMeses);
            if (pd != null && pd.ClassInfo.FindMember(columnaPercentil) != null)
                return Convert.ToDecimal(pd.GetMemberValue(columnaPercentil));
            else
                return 0;
        }

        /// <summary>
        /// Retornar el valor de un percentil (columna) a partir de los parametros edadMeses y aValor
        /// </summary>
        /// <param name="edadMeses">Edad en meses del percentil a retornar</param>
        /// <param name="aValor">Valor para buscar el tramo o columna de percentil cuyo valor es el mas proximo</param>
        /// <returns>El valor de la columna percentil que se aproxima mas al parametro aValor</returns>
        public decimal ObtenerPercentil(int edadMeses, decimal aValor)
        {
            PercentilTablaDetalle pd = Detalles.FirstOrDefault<PercentilTablaDetalle>(x => x.EdadMes == edadMeses);
            if (pd != null)
                return pd.ObtenerPercentil(aValor);
            else
                return 0;
        }

        /// <summary>
        /// Retornar el BO que corresponde para la tabla de percentiles actual
        /// </summary>
        /// <param name="edadMeses">Edad en meses de la fila de percentil a retornar</param>
        /// <returns>un BO de tipo que corresponde a la fila para la edad en la tabla de percentiles</returns>
        public PercentilTablaDetalle ObtenerPercentilDetalle(int edadMeses)
        {
            return Detalles.FirstOrDefault<PercentilTablaDetalle>(x => x.EdadMes == edadMeses);
        }

        #endregion
        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue(nameof(PersistentProperty), ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}