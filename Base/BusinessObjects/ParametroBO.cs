using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;


namespace SBT.Apps.Base.Module.BusinessObjects
{
    /// <summary>
    /// Implementar clase cuyas propiedades se construyen dinamicamente para leer los parametros de consultas a ejecutar
    /// </summary>
    [MemberDesignTimeVisibility(false), ModelDefault("Caption", "Parámetros"), DomainComponent]
    public class ParametroBO
    {
        //public ParametroBO(Session session, XPClassInfo classInfo) : base(session, classInfo)
        //{

        //}
    }
}
