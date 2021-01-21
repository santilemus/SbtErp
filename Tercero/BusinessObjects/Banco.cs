using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Validation;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.Linq;

namespace SBT.Apps.Tercero.Module.BusinessObjects
{
    [DefaultClassOptions, CreatableItem(false)]
    [XafDefaultProperty("Nombre"), NavigationItem("Catalogos"), ModelDefault("Caption", "Bancos")]
    [ImageName("Bank")]
    [DevExpress.Xpo.MapInheritance(DevExpress.Xpo.MapInheritanceType.ParentTable)]    
    public class Banco : Tercero
    {
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Comision = 0.0m;
        }

        public Banco(DevExpress.Xpo.Session session)  : base(session)
        {
        }

        private System.Decimal comision;

        [DevExpress.Persistent.Base.ToolTipAttribute("Porcentaje comisión tarjetas")]
        [DevExpress.ExpressApp.DC.XafDisplayNameAttribute("Porcentaje Comisión")]
        [DevExpress.Persistent.Base.VisibleInListViewAttribute(false)]
        public System.Decimal Comision
        {
            get => comision;
            set => SetPropertyValue(nameof(Comision), ref comision, value);
        }

    }
}
