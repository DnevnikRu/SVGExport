using System.Web;

namespace SVGExport
{
    public class HttpHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            Export.ProcessExportRequest(context);
        }
    }
}