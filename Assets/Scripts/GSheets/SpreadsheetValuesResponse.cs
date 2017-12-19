using System;
using System.Collections.Generic;

namespace GSheets
{
    [Serializable]
    public class SpreadsheetValuesResponse
    {
        public string range { get; set; }
        public string majorDimension { get; set; }
        public List<List<string>> values { get; set; }
    }
}