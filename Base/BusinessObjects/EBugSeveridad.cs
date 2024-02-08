using DevExpress.ExpressApp.DC;

namespace SBT.Apps.Base.Module.BusinessObjects
{
    public enum EBugSeveridad
    {
        [XafDisplayName("Crítico")]
        Critico = 0,
        Alto = 1,
        Medio = 2,
        Bajo = 3,
        Cosmetico = 4
    }
}