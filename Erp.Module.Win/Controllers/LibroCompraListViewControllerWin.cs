using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SBT.Apps.Base.Module.BusinessObjects;
using SBT.Apps.Iva.Module.BusinessObjects;
using SBT.Apps.Iva.Module.Controllers;
using DevExpress.XtraEditors;

namespace SBT.Apps.Erp.Module.Win.Controllers
{
    public class LibroCompraListViewControllerWin: LibroCompraListViewController
    {
        public LibroCompraListViewControllerWin(): base()
        {

        }

        protected override void OnActivated()
        {
            base.OnActivated();
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }

        protected override void DoExecuteExport(RangoFechaParams parametros)
        {
            base.DoExecuteExport(parametros);
            var cvs = ToCsvStream(parametros.FechaInicio, parametros.FechaFin);
            System.IO.Stream stream;
            
            XtraSaveFileDialog dlg = new XtraSaveFileDialog();
            dlg.Title = "Exportar Libro de Compras";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if ((stream = dlg.OpenFile()) != null)
                {
                    cvs.Position = 0;
                    cvs.WriteTo(stream);
                    
                    cvs.Close();
                }
                stream.Close();
                stream.Dispose();
            }
            cvs.Close();
            cvs.Dispose();
        }
    }
}
