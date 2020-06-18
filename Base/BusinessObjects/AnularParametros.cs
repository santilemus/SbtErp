﻿using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    [DomainComponent]
    [DefaultClassOptions]
    [ModelDefault("Caption", "Parámetros <Anulación>"), NavigationItem(false)]
    //[ImageName("BO_Unknown")]
    //[DefaultProperty("SampleProperty")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class AnularParametros
    {
        public AnularParametros()
        {

        }

        [Size(250)]
        public string Comentario { get; set; }
    }
}