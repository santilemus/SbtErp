using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using SBT.Apps.Base.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBT.Apps.Facturacion.Module.Controllers
{
    public class DocumentosAnuladosListViewController: ViewController<ListView>
    {
        public DocumentosAnuladosListViewController(): base()
        {
            TargetObjectType = typeof(SBT.Apps.Facturacion.Module.BusinessObjects.DocumentosAnulados);
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            if (View.ObjectTypeInfo.FindMember("Empresa") != null && !(View.CollectionSource.Criteria.ContainsKey("Empresa Actual")) && SecuritySystem.CurrentUser != null)
                View.CollectionSource.Criteria["Empresa Actual"] = CriteriaOperator.Parse("[Empresa.Oid] = ?", ((Usuario)SecuritySystem.CurrentUser).Empresa.Oid);
        }
    }
}
