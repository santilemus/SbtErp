using System;
using System.Collections.Generic;
using DevExpress.ExpressApp.DC;

namespace SBT.Apps.Medico.Generico.Module.BusinessObjects
{
    public enum ETablaPercentilWho
    {
        [XafDisplayName("Estatura Niñas 0 - 2 Años")]
        LengthGirl_0_2Y = 1,
        [XafDisplayName("Estatura Niños 0 - 2 Años")]
        LengthBoy_0_2Y = 2,
        [XafDisplayName("Peso Niñas 0 - 2 Años")]
        WeightGirl_0_2Y = 3,
        [XafDisplayName("Peso Niños 0 - 2 Años")]
        WeightBoy_0_2Y = 4,
        [XafDisplayName("Peso Longitud y Circunferencia Cabeza Niñas")]
        WLHC_Girl = 5,
        [XafDisplayName("Peso Longitud y Circunferencia Cabeza Niñós")]
        WLHC_Boy = 6
    }
}
