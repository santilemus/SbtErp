using System;
using System.Collections.Generic;
using DevExpress.ExpressApp.DC;

namespace SBT.Apps.Medico.Generico.Module.BusinessObjects
{
    public enum ETablaPercentilCdo
    {
        [XafDisplayName("Estatura Hembras 2 - 20 Años")]
        StatureFemale = 1,
        [XafDisplayName("Estatura Varones 2 - 20 Años")]
        StatureMale = 2,
        [XafDisplayName("Indice Masa Muscular Hembras 2 - 20 Años")]
        BMI_Female = 3,
        [XafDisplayName("Indice Masa Muscular 2 - 20 Años")]
        BMI_Male = 4
    }
}
