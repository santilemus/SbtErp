using System;
using DevExpress.Xpo;
using SBT.Apps.Medico.Generico.Module.BusinessObjects;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using SBT.Apps.Base.Module.BusinessObjects;

namespace SBT.Apps.Medico.Generico.Module.BusinessObjects
{
    [DefaultClassOptions]
    //[DevExpress.Persistent.Base.ImageNameAttribute("vacunas")]
    [NavigationItem("Salud"), Persistent("TerminologiaAnatomica"),
        XafDefaultProperty("TerminoAnatomico"), ModelDefault("Caption", "Terminología Anatómica")]
    public class TerminologiaAnatomica : XPObjectBaseBO
    {

        public TerminologiaAnatomica(Session session) : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.
        }

        #region Propiedades

        MedicoLista categoria;
        string terminoAnatomico;
        string codigoFMA;
        string codigoTA;

        [Size(12), DbType("varchar(12)"), Indexed(Name = "idxTerminologiaAnatomica_CodigoTA", Unique = true), XafDisplayName("Código TA"),
            ModelDefault("DisplayFormat", "L00.0.00.000"), ModelDefault("EditMask", "L00.0.00.000"),
            RuleRequiredField("TerminologiaAnatomica.CodigoTA_Requerido", "Save"),
            RuleUniqueValue("TerminologiaAnatomica.CodigoTA_Unico", DefaultContexts.Save, SkipNullOrEmptyValues = true)]
        public string CodigoTA
        {
            get => codigoTA;
            set => SetPropertyValue(nameof(CodigoTA), ref codigoTA, value);
        }


        [Size(10), XafDisplayName("Código FMA"), DbType("varchar(10)"), VisibleInLookupListView(true)]
        public string CodigoFMA
        {
            get => codigoFMA;
            set => SetPropertyValue(nameof(CodigoFMA), ref codigoFMA, value);
        }


        [Size(150), DbType("varchar(150)"), XafDisplayName("Término Anatómico"),
            RuleRequiredField("TerminologiaAnatomica.TerminoAnatomico_Requerido", DefaultContexts.Save)]
        public string TerminoAnatomico
        {
            get => terminoAnatomico;
            set => SetPropertyValue(nameof(TerminoAnatomico), ref terminoAnatomico, value);
        }


        [Size(10), DbType("varchar(10)"), XafDisplayName("Categoría"),
            RuleRequiredField("TerminologiaAnatomica.Categoria_Requerida", "Save")]       
        [Association("MedicoListas-TerminologiaAnatomicas")]
        [DataSourceCriteria("[Categoria] == 1")]
        public MedicoLista Categoria
        {
            get => categoria;
            set => SetPropertyValue(nameof(Categoria), ref categoria, value);
        }


        #endregion
    }

}