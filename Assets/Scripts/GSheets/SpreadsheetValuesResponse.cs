using System;
using System.Collections.Generic;

namespace GSheets
{
    [Serializable]
    internal sealed class SpreadsheetValuesResponse
    {
        public string Range { get; set; }
        public string MajorDimension { get; set; }
        public List<List<string>> Values { get; set; }
    }
}