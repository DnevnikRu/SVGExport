using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Web;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using Svg;

namespace SVGExport
{
    internal class Exporter
    {
        #region Fileds

        private readonly DataModel model;

        #endregion

        #region Properties

        public string ContentDisposition { get; private set; }

        #endregion

        #region Constructor

        internal Exporter(DataModel model)
        {
            this.model = model;

            string extension;

            switch (model.ContentType)
            {
                case "application/pdf":
                    extension = "pdf";
                    break;
                default:
                    throw new ArgumentException(string.Format("Invalid content type specified: {0}", model.ContentType));
            }

            model.Filename = string.Format("{0}.{1}", model.Filename, extension);

            ContentDisposition = string.Format("attachment; filename={0}", model.Filename);
        }

        #endregion

        #region Methods

        private IEnumerable<SvgDocument> CreateListOfSvgDocument()
        {
            var result = new List<SvgDocument>();

            foreach (var item in model.Data)
            {
                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(item)))
                {
                    result.Add(SvgDocument.Open(stream));
                }
            }

            return result;
        }

        internal void WriteToHttpResponse(HttpResponse httpResponse)
        {
            httpResponse.ClearContent();
            httpResponse.ClearHeaders();
            httpResponse.ContentType = model.ContentType;
            httpResponse.AddHeader("Content-Disposition", ContentDisposition);
            WriteToStream(httpResponse.OutputStream);
        }

        internal void WriteToStream(Stream outputStream)
        {
            switch (model.ContentType)
            {
                case "application/pdf":
                    {
                        using (var seekableStream = new MemoryStream())
                        {
                            var svgList = CreateListOfSvgDocument();
                            var pdfDocument = new PdfDocument();
                            var pdfPage = pdfDocument.AddPage();
                            var gfx = XGraphics.FromPdfPage(pdfPage);
                            pdfPage.DrawTitle(gfx, model.Title);

                            foreach (var svg in svgList.Select((v, i) => new {Value = v, Index = i}))
                            {
                                var number = svg.Index % model.Count;
                                if (number == 0 && svg.Index != 0)
                                {
                                    gfx.Restore(gfx.Save());
                                    pdfDocument.Save(seekableStream, false);
                                    pdfPage = pdfDocument.AddPage();
                                    gfx = XGraphics.FromPdfPage(pdfPage);
                                    pdfPage.DrawTitle(gfx, model.Title);
                                }

                                var image = svg.Value.Draw();
                                image.SetResolution(model.Resolution, model.Resolution);
                                gfx.DrawImage(image, new XPoint(30.0f, (90.0f + 250.0f * number)));
                            }
                            gfx.Restore(gfx.Save());
                            pdfDocument.Save(seekableStream, false);
                            seekableStream.WriteTo(outputStream);
                        }
                    }
                    break;
            }

            outputStream.Flush();
        }

        #endregion
    }
}