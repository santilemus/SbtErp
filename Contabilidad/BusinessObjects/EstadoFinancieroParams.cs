using DevExpress.ExpressApp.DC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBT.Apps.Contabilidad.Module.BusinessObjects
{
    [DomainComponent]
    public class EstadoFinancieroParams
    {
        public DateTime FechaHasta
        {
            get;
            set;
        }
    }


}
