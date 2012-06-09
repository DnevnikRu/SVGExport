using System.Collections.Generic;

namespace SVGExport
{
    public class DataModel
    {
        #region Properties

        public string Filename { get; set; }
        public string ContentType { get; set; }
        public string Title { get; set; }
        public int Count { get; set; }
        public float Resolution { get; set; }
        public List<string> Data { get; set; }

        #endregion

        #region Constructors

        public DataModel()
        {
            Filename = "file";
            ContentType = "application/pdf";
            Title = string.Empty;
            Count = 1;
            Resolution = 96.0f;
        }

        #endregion

        #region Methods

        public bool IsValid
        {
            get
            {
                return 
                    !string.IsNullOrEmpty(Filename) &&
                    !string.IsNullOrEmpty(ContentType) && 
                    Count > 0 &&
                    Resolution >= 48.0f &&
                    Data.Count > 0;
            }
        }

        #endregion
    }
}