using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomatasFinitos.Beans
{
    public class Mealy_Moore
    {
        public List<string> estados { get; set; }                  // estados del automata
        public List<string> alfaEntrada { get; set; }             // alfabeto de entrada
        public List<string> alfaSalida { get; set; }                            // alfabeto de salida del automata
        public string estadoInicial { get; set; }                               // estado con el que incia el automata
        public Dictionary<string, string> funcTransicion { get; set; }    // representación de todas las funciones de transición
        public Dictionary<string, string> funcSalida { get; set; }        // representación de todas las funciones de salida
    }
}
