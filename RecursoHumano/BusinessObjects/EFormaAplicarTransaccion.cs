using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.DC;

namespace SBT.Apps.RecursoHumano.Module.BusinessObjects
{
    public enum EFormaAplicarTransaccion
    {
        [XafDisplayName("Primera Quincena")]
        PrimeraQuincena = 1,
        [XafDisplayName("Segunda Quincena")]
        SegundaQuincena = 2,
        [XafDisplayName("Ambas")]
        Ambas = 3
    }
}
