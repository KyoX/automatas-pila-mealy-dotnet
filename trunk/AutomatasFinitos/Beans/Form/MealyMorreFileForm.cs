using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomatasFinitos.Form
{
    class MealyMorreFileForm
    {
        public string estados { get; set; }
        public string alfabetoEntrada { get; set; }
        public string alfabetosalida { get; set; }
        public string estadoinicial { get; set; }
        public string funcionesTransición { get; set; }
        public string funcionesSalida { get; set; }
    }
}
