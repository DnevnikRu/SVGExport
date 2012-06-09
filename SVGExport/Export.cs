using System.Web;
using Newtonsoft.Json;

namespace SVGExport
{
    internal static class Export
    {
        internal static void ProcessExportRequest(HttpContext context)
        {
            if (context != null && context.Request.HttpMethod.Equals("POST"))
            {
                var request = context.Request;

                var model = JsonConvert.DeserializeObject<DataModel>(request.Form["json"]);
                
                if (model.IsValid)
                {
                    var exporter = new Exporter(model);

                    exporter.WriteToHttpResponse(context.Response);

                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    context.Response.End();
                }
            }
        }
    }
}