using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomatasFinitos.Beans
{
    public class PDA
    {
        public List<string> estados { get; set; }                  // estados del automata
        public List<string> alfaEntrada { get; set; }             // alfabeto de entrada
        public List<string> alfaPila { get; set; }                            // alfabeto del stack
        public string estadoInicial { get; set; }                               // estado con el que incia el automata
        public string simboloInicialPila { get; set; }                          // simbolo con el que comienza la pila
        public List<string> estadosFinales { get; set; }                        // Estados de aceptación del automata
        public Dictionary<string, string> funcTransicion { get; set; }    // representación de todas las funciones de transición        
    }
}
