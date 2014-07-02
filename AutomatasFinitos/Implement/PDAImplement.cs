﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutomatasFinitos.Beans;

namespace AutomatasFinitos.Implement
{
    class PDAImplement
    {
        public PDA pda { get; set; }
        public int delay { get; set; }
        public List<string> stack { get; set;}
        public string lastState { get; set; } // estado en que ha quedado el automata

        public PDAImplement(PDA pda)
        {
            // TODO: Complete member initialization
            this.pda = pda;
        }

        public PDAImplement() {}

        /**
         * 
         * No basta el comportamiento habitual de una lista, necesito simular una pila
         * por eso creo las funciones put y pop
         * 
         * */
        public void putInStack(string data){
            stack.Insert(stack.Count, data);
        }

        /**
         * 
         * Favor no validar si tiene elementos aquí, se supone que es primitiva
         * la validación se tiene que hacer antes de llamarla, por eso el stack
         * esta como publico
         * 
         * */
        public string popStack()
        {
            string temp = stack.Last();
            stack.RemoveAt(stack.Count - 1);
            return temp;
        }
         
        /**
         * 
         * Inicializa el pda con los datos necesarios para la ejecución
         * 
         * */
        public void inicializar(double delay)
        {
            this.delay = (int) delay;
            stack = new List<string>();
            this.lastState = pda.estadoInicial;
        }

        /**
         *
         * valido solo la función de transición porque en caso de no existir no 
         * se puede hacer nada más, y termina la ejecución. En caso de que no exista
         * la función de salida, se generará la salida por defecto, la cual es: 
         * & (cadena vacía) por eso no se valida
         * 
         **/
        public bool validateTransition(string entrada, string elementoPila)
        {
            return pda.funcTransicion.ContainsKey(lastState + entrada + elementoPila); 
        }

        public string generateTransition(string entrada, string elementoPila)
        {
            string temp = pda.funcTransicion[lastState + entrada + elementoPila];
            lastState = temp;                                   // actualizo al ultimo estado
            return temp;
        }

    }
}
