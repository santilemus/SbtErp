using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBT.Apps.Contabilidad.Module.BusinessObjects
{
    [DomainComponent]
    [ModelDefault("Caption", "Generar Estado Financiero - Parámetros")]
    public class EstadoFinancieroParams : NonPersistentObjectImpl
    {
        public EstadoFinancieroParams()
        {

        }

        private Moneda moneda;
        private DateTime fechaHasta;
        private IList<Moneda> monedas;

        public DateTime FechaHasta
        {
            get => fechaHasta;
            set => SetPropertyValue(ref fechaHasta, value);
        }

        [DataSourceProperty("Monedas")]
        public Moneda Moneda
        {
            get
            {
                if (moneda == null)
                    moneda = ObjectSpace.GetObjects<Moneda>(new BinaryOperator("Activa", true)).FirstOrDefault();
                return moneda;
            }
            set => SetPropertyValue(ref moneda, value);
        }

        [Browsable(false)]
        public IList<Moneda> Monedas
        {
            get
            {
                if (monedas == null)
                    monedas = ObjectSpace.GetObjects<Moneda>(new BinaryOperator("Activa", true));
                return monedas;
            }
            internal set { monedas = value; }
        }
    }

}
