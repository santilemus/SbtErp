using DevExpress.ExpressApp.DC;
using System;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    public enum ETipoRoleUnidad
    {
        [XafDisplayName("Unidad Interna")]
        Unidad = 1,
        [XafDisplayName("Agencia")]
        Agencia = 2,
        [XafDisplayName("Bodega")]
        Bodega = 3,
        [XafDisplayName("Unidad Externa")]
        UnidadExterna = 4
    }
}
