
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace SBT.Apps.Medico.Module.Web.Classes
{
    /// <summary>
    /// Viewer de las imagenes provenientes de PacienteFileData
    /// </summary>
    /// <remarks>
    /// Mas informacion en: https://supportcenter.devexpress.com/ticket/details/t751790/how-to-open-a-picture-in-a-separate-window-when-a-user-clicks-an-action
    /// LA IMPLEMENTACION DEL SIGUIENTE ENLACE TENEMOS QUE HACER Y PROBAR PORQUE ESTA YA NO FUNCIONA
    /// https://docs.devexpress.com/eXpressAppFramework/113601/shape-export-print-data/reports/task-based-help/how-to-print-a-report-without-displaying-a-preview
    /// </remarks>
    public class ImageViewer : IHttpHandler, IReadOnlySessionState
    {
        public byte[] bytes { get; set; }
        public string fileName { get; set; }
        public string contentType { get; set; }

        public void ProcessRequest(HttpContext context)
        {
            if (bytes == null)
                return;
            try
            {
                context.Response.Buffer = true;
                context.Response.Charset = "";
                context.Response.AddHeader("Content-Disposition", "inline; filename=" + fileName);
                context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                //context.Response.ContentType = "application/octectstream";
                context.Response.ContentType = contentType;
                context.Response.BinaryWrite(bytes);             
                context.Response.Flush();
                context.Response.End();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsReusable => false;

    }

}
