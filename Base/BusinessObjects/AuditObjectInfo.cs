using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using System;
using System.ComponentModel;
using System.Linq;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    [DomainComponent]
    [DefaultClassOptions]
    [ModelDefault("Caption", "Auditoría del Registro"), NavigationItem(false)]
    //[ImageName("BO_Unknown")]
    //[DefaultProperty("SampleProperty")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class AuditObjectInfo
    {
        public AuditObjectInfo(XPCustomBaseBO BO)
        {
            fechaCrea = BO.FechaCrea;
            usuarioCrea = BO.UsuarioCrea;
            fechaMod = BO.FechaMod;
            usuarioMod = BO.UsuarioMod;
        }

        public AuditObjectInfo(XPObjectBaseBO BO)
        {
            fechaCrea = BO.FechaCrea;
            usuarioCrea = BO.UsuarioCrea;
            fechaMod = BO.FechaMod;
            usuarioMod = BO.UsuarioMod;
        }

        DateTime ? fechaCrea;
        string usuarioCrea;
        DateTime ? fechaMod;
        string usuarioMod;

        [XafDisplayName("Fecha de Creación:"), ModelDefault("DisplayFormat", "{0:G}")]
        public DateTime? FechaCrea => fechaCrea;

        [XafDisplayName("Usuario Creó"), FieldSize(25)]
        public string UsuarioCrea => usuarioCrea;

        [XafDisplayName("Fecha Última Modificación:"), ModelDefault("DisplayFormat", "{0:G}")]
        public DateTime? FechaMod => fechaMod;

        [XafDisplayName("Usuario Ultima Modificación"), FieldSize(25)]
        public string UsuarioMod => usuarioMod;




    }
}