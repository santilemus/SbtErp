using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SBT.Apps.Medico.Module.Web.Classes
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// https://supportcenter.devexpress.com/ticket/details/t751790/how-to-open-a-picture-in-a-separate-window-when-a-user-clicks-an-action
    /// https://supportcenter.devexpress.com/ticket/details/t751219/show-png-file-or-pdf-file-in-browser-window-in-xaf-web-application
    /// </remarks>
    public class WebDocumentViewer: IHttpHandler
    {
        public byte[] bytes { get; set; }
        public string fileName { get; set; }
        public string contentType { get; set; }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Buffer = true;
            context.Response.Charset = "";
            if (true) // context.Request.QueryString["download"] == "1")  
            {
                context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            }
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.Response.ContentType = contentType;
            context.Response.BinaryWrite(bytes);
            //context.Response.FlushAsync();
            context.Response.Flush();
            context.Response.End();
        }

        public bool IsReusable => false;
    }
}
