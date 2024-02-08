using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;

namespace SBT.Apps.RecursoHumano.Module.BusinessObjects
{

    [DomainComponent]
    [DefaultClassOptions]
    [ModelDefault("Caption", "Aprobación o Rechazo"), NavigationItem(false), CreatableItem(false)]
    //[ImageName("BO_Unknown")]
    public class APersonalParam
    {

        public APersonalParam()
        {

        }

        [DevExpress.ExpressApp.DC.FieldSize(150), XafDisplayName("Comentario")]
        public String Comentario { get; set; }

    }
}