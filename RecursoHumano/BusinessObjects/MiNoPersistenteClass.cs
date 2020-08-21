using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBT.Apps.RecursoHumano.Module.BusinessObjects
{
    [DomainComponent, ModelDefault("Caption", "Mi Clase No Persistente")]
    public class MiNoPersistenteClass
    {

        DateTime fechaDesde;

        public DateTime Test
        {
            get => fechaDesde;
            set => fechaDesde = value;
        }

        public DateTime FechaHasta { get; set; }
        public string Cuenta { get; set; }

        [Browsable(false)]
        public SBT.Apps.Contabilidad.BusinessObjects.Catalogo ListaCuentas { get; set; }

        
    }
}
