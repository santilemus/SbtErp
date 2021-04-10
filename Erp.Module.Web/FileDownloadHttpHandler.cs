using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace SBT.Apps.Erp.Module.Web
{
    /// <summary>
    /// Clase para implementar descarga de archivos
    /// </summary>
    /// <remarks>
    /// Info en: https://supportcenter.devexpress.com/ticket/details/t152062/can-t-download-file-from-server-to-client-with-webclient#
    /// Como usar en un action en la misma pagina, ver siguiente fragmento de codigo
    /// void action_Execute(object sender, SimpleActionExecuteEventArgs e) {  
    ///    string nombre = "LogoImage.png";
    ///    string url = FileDownloadHttpHandler.GetResourceUrl(nombre);
    ///    string clientScript = string.Format(@"window.location = ""{0}"";", url);
    ///    WebWindow.CurrentRequestWindow.RegisterStartupScript("DownloadFile", clientScript);  
    ///}
    /// </remarks>
public class FileDownloadHttpHandler : IHttpHandler, IRequiresSessionState
    {
        private readonly object lockObject = new object();
        public static string GetResourceUrl(string name)
        {
            return string.Format("FileDownloadResource.axd?name={0}", name);
        }
        #region IHttpHandler Members  
        public bool IsReusable
        {
            get { return true; }
        }
        public void ProcessRequest(HttpContext context)
        {
            lock (lockObject)
            {
                string name = context.Request.QueryString["name"];
                string patch = "~/Content/Temporales/" + name;
                System.IO.FileInfo toDownload = new System.IO.FileInfo(HttpContext.Current.Server.MapPath(patch));
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=\"" + HttpUtility.UrlEncode(toDownload.Name) + "\"");
                HttpContext.Current.Response.AddHeader("Content-Length", toDownload.Length.ToString());
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.TransmitFile(patch);
                if (HttpContext.Current.Response.Filter != null)
                {
                    HttpContext.Current.Response.Filter = null;
                }
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }
        #endregion
    }
}
