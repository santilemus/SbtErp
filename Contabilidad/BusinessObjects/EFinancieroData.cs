using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBT.Apps.Contabilidad.Module.BusinessObjects
{
    [DomainComponent]
    [DefaultClassOptions]
    [VisibleInReports]
    [ModelDefault("Caption", "EstadoFinanciero"), NavigationItem(false)]
    public class EFinancieroData: NonPersistentEntityObject
    {

        decimal valor2;
        int nivel2;
        string nombre2;
        decimal valor1;
        int nivel1;
        string nombre1;
        int oid;
        [Key(false)]
        public int Oid
        {
            get => oid;
            set => SetPropertyValue(ref oid, value);
        }

        [Size(150)]
        public string Nombre1
        {
            get => nombre1;
            set => SetPropertyValue(ref nombre1, value);
        }

        public int Nivel1
        {
            get => nivel1;
            set => SetPropertyValue(ref nivel1, value);
        }
       
        public decimal Valor1
        {
            get => valor1;
            set => SetPropertyValue(ref valor1, value);
        }

        [Size(150)]
        public string Nombre2
        {
            get => nombre2;
            set => SetPropertyValue(ref nombre2, value);
        }

        public int Nivel2
        {
            get => nivel2;
            set => SetPropertyValue(ref nivel2, value);
        }

        public decimal Valor2
        {
            get => valor2;
            set => SetPropertyValue(ref valor2, value);
        }

        public override void OnSaving()
        {
            base.OnSaving();
        }

    }
}
