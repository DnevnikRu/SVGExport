using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;

namespace SVGExport
{
    public static class Extensions
    {
        public static void DrawTitle(this PdfPage page, XGraphics gfx, string title)
        {
            var rect = new XRect(new XPoint(30.0f, 25.0f), gfx.PageSize);
            var font = new XFont("Tahoma", 11, XFontStyle.Regular);
            var tf = new XTextFormatter(gfx);
            tf.DrawString(title, font, XBrushes.Black, rect, XStringFormats.TopLeft);
        }
    }
}