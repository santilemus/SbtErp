//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
namespace visible.medico.Expediente.Module.BusinessObjects
{
  [DefaultClassOptions]
  public partial class Vacuna : DevExpress.Xpo.XPObject
  {
    private System.Boolean _activa;
    private System.String _comentario;
    private System.String _nombre;
    public Vacuna(DevExpress.Xpo.Session session)
      : base(session)
    {
    }
    public System.String Nombre
    {
      get
      {
        return _nombre;
      }
      set
      {
        SetPropertyValue("Nombre", ref _nombre, value);
      }
    }
    [DevExpress.Xpo.SizeAttribute(500)]
    public System.String Comentario
    {
      get
      {
        return _comentario;
      }
      set
      {
        SetPropertyValue("Comentario", ref _comentario, value);
      }
    }
    public System.Boolean Activa
    {
      get
      {
        return _activa;
      }
      set
      {
        SetPropertyValue("Activa", ref _activa, value);
      }
    }
    [DevExpress.Xpo.AssociationAttribute("Dosis-Vacuna")]
    [DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false)]
    public XPCollection<visible.medico.Expediente.Module.BusinessObjects.VacunaDosis> Dosis
    {
      get
      {
        return GetCollection<visible.medico.Expediente.Module.BusinessObjects.VacunaDosis>("Dosis");
      }
    }
  }
}
