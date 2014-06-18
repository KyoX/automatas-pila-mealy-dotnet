using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutomatasFinitos.Beans;

namespace AutomatasFinitos.Implement
{
    class PDAImplement
    {
        private PDA pda;
        public bool initFromFile(string fileName)
        {
            System.IO.StreamReader file = null;
            try
            {
                file = new System.IO.StreamReader(fileName);
                string linea;
                if (file != null)
                {
                    pda = new PDA();

                    // Estados del automata
                    linea = file.ReadLine();
                    if (linea != null && linea.Length > 0 && linea.IndexOf(",") > 0)
                    {
                        linea = linea.Replace(" ", string.Empty);
                        pda.estados = new List<string>(linea.Split(','));
                    }
                    else { return false; }

                    // Alfabeto de entrada
                    linea = file.ReadLine();
                    if (linea != null && linea.Length > 0 && linea.IndexOf(",") > 0)
                    {
                        linea = linea.Replace(" ", string.Empty);
                        pda.alfaEntrada = new List<string>(linea.Split(','));
                    }
                    else { return false; }

                    // Alfabeto del stack/pila
                    linea = file.ReadLine();
                    if (linea != null && linea.Length > 0 && linea.IndexOf(",") > 0)
                    {
                        linea = linea.Replace(" ", string.Empty);
                        pda.alfaPila = new List<string>(linea.Split(','));
                    }
                    else { return false; }

                    //estado inicial del automata
                    linea = file.ReadLine();
                    if (linea != null && linea.Length > 0 && pda.estados.Contains(linea) && linea.IndexOf(",") < 0)
                    {
                        linea = linea.Replace(" ", string.Empty);
                        pda.estadoInicial = linea;
                    }
                    else { return false; }

                    // estado inicial de la pila/stack
                    linea = file.ReadLine();
                    if (linea != null && linea.Length > 0 && pda.alfaPila.Contains(linea) && linea.IndexOf(",") < 0)
                    {
                        linea = linea.Replace(" ", string.Empty);
                        pda.simboloInicialPila = linea;
                    }
                    else { return false; }

                    // estados finales / aceptación
                    linea = file.ReadLine();
                    if (linea != null && linea.Length > 0 && linea.IndexOf(",") > 0)
                    {
                        linea = linea.Replace(" ", string.Empty);
                        pda.estadosFinales = new List<string>(linea.Split(','));
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
                        
                    }
                    if (dicTemp.Count > 0)
                    {
                        pda.funcTransicion = dicTemp;
                    }
                    else { return false; }

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
