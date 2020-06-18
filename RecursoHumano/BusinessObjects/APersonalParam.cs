using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Xpo;
using DevExpress.Persistent.Validation;

namespace SBT.Apps.RecursoHumano.Module.BusinessObjects
{ 

    [DomainComponent]
    [DefaultClassOptions]
    [ModelDefault("Caption", "Aprobación o Rechazo"), NavigationItem(false)]
    //[ImageName("BO_Unknown")]
    public class APersonalParam
    {

        public APersonalParam()
        {

        }

        [Size(150), XafDisplayName("Comentario")]
        public String Comentario { get; set; }

    }
}