using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutomatasFinitos.Beans;

namespace AutomatasFinitos.Implement
{
    class MealyImplement
    {
        private Mealy_Moore mm;
        public int delay { get; set; }
        public string stack { get; set;}
        public string lastState { get; set; } // estado en que ha quedado el automata


        public MealyImplement(Mealy_Moore moore) { 
            this.mm = moore; 
        }

        public MealyImplement() { }
        /**
         *
         * valido solo la función de transición porque en caso de no existir no 
         * se puede hacer nada más, y termina la ejecución. En caso de que no exista
         * la función de salida, se generará la salida por defecto, la cual es: 
         * & (cadena vacía) por eso no se valida
         * 
         **/
        public bool validateTransition(string entrada) {
            return mm.funcTransicion.ContainsKey(lastState+entrada); // 
        }

        public string generateTransition(string entrada)
        {
            string temp = mm.funcTransicion[lastState + entrada];
            lastState = temp;                                   // actualizo al ultimo estado
            return temp;
        }

        /**
         * 
         * Llamar a esta función antes de generateTransition, así no afecta su salida
         * 
         * */
        public string generateOutputVal(string entrada)
        {
            if (mm.funcSalida.ContainsKey(lastState + entrada))
            {
                stack = mm.funcSalida[lastState + entrada];
            }
            else
            {
                stack = "&";
            }
            return stack;
        }

        public void inicializar(double delay)
        {
            stack = "&";
            lastState = mm.estadoInicial;
            this.delay = (int)delay * 100;
        }

        public List<string> obtenerListaEstados()
        {      
            if (this.mm != null)// si se creo 
            {
                if (this.mm.estados.Count > 0) // si tiene estados
                {
                    return this.mm.estados;
                }                              
            }
            return new List<string>();
        }
    }
}
