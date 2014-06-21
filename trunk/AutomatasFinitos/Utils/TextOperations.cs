using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomatasFinitos.Utils
{
    public static class TextOperations
    {
        public static string concatCsv_L(List<string> L)
        {
            string linea = "";
            foreach (string temp in L) { linea = linea + temp + ","; }
            if(linea.Length>0) { linea = linea.Substring(0, linea.Length - 1); }
            return linea;
        }

        public static string concatCsv_D(Dictionary <string, string> D, char separator)
        {
            string linea = "";
            foreach (KeyValuePair<string, string> entry in D)
            {
                linea = linea + entry.Key + separator + entry.Value + ",";
            }
            if (linea.Length > 0) { linea = linea.Substring(0, linea.Length - 1); }
            return linea;
        }

    }
}
