using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutomatasFinitos.Beans;
using System.Runtime.Serialization;

namespace AutomatasFinitos.Implement
{
    class MealyImplement
    {
       private Mealy_Moore mm;

       public bool initFromFile(string fileName)
       {
           System.IO.StreamReader file = null;
           try
           {
               file = new System.IO.StreamReader(fileName);
               string linea;
               if (file != null)
               {
                   mm = new Mealy_Moore();

                   // Estados del automata
                   linea = file.ReadLine();
                   if (linea != null && linea.Length > 0 && linea.IndexOf(",") > 0)
                   {
                       linea = linea.Replace(" ", string.Empty);
                       mm.estados = new List<string>(linea.Split(','));
                   }
                   else { return false; }

                   // Alfabeto de entrada
                   linea = file.ReadLine();
                   if (linea != null && linea.Length > 0 && linea.IndexOf(",") > 0)
                   {
                       linea = linea.Replace(" ", string.Empty);
                       mm.alfaEntrada = new List<string>(linea.Split(','));
                   }
                   else { return false; }

                   // alfabeto de salida
                   linea = file.ReadLine();
                   if (linea != null && linea.Length > 0 && linea.IndexOf(",") > 0)
                   {
                       linea = linea.Replace(" ", string.Empty);
                       mm.alfaSalida = new List<string>(linea.Split(','));
                   }
                   else { return false; }


                   // estado inicial
                   linea = file.ReadLine();
                   if (linea != null && linea.Length > 0 && mm.estados.Contains(linea) && linea.IndexOf(",") < 0)
                   {
                       linea = linea.Replace(" ", string.Empty);
                       mm.estadoInicial = linea;
                   }
                   else { return false; }

                   //función de transición
                   linea = file.ReadLine();
                   Dictionary<string, string> dicTemp = new Dictionary<string, string>();
                   if (linea != null && linea.Length > 0 && linea.IndexOf(",") > 0)
                   {
                       string[] temp = linea.Split(',');
                       foreach (string t in temp)
                       {
                           if (!t.Contains(":")) { return false; }
                           string[] key = t.Split(':');
                           dicTemp.Add(key[0].Replace(" ", string.Empty), key[1].Replace(" ", string.Empty));
                       }
                       if (dicTemp.Count > 0)
                       {
                           mm.funcTransicion = dicTemp;
                       }
                   }

                   //función de salida
                   linea = file.ReadLine();
                   dicTemp = new Dictionary<string, string>();
                   if (linea != null && linea.Length > 0 && linea.IndexOf(",") > 0)
                   {
                       string[] temp = linea.Split(',');
                       foreach (string t in temp)
                       {
                           if (!t.Contains(":")) { return false; }
                           string[] key = t.Split(':');
                           dicTemp.Add(key[0].Replace(" ", string.Empty), key[1].Replace(" ", string.Empty));
                       }
                       if (dicTemp.Count > 0)
                       {
                           mm.funcSalida = dicTemp;
                       }
                   }

               }
               else { return false; }

           }
           catch (Exception e)
           {
               Console.Write(e);
               return false;
           }
           finally
           {
               file.Close();
           }
           
           return true;
       }
    }
}
