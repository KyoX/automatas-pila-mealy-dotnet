using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AutomatasFinitos.Beans;
using AutomatasFinitos.Implement;
using System.Text.RegularExpressions;
using AutomatasFinitos.Utils;

namespace AutomatasFinitos.Pantallas
{
    /// <summary>
    /// Lógica de interacción para MooreGenerator.xaml
    /// </summary>
    public partial class MooreGenerator : Window
    {
        public Mealy_Moore mm { get; set; }

        private List<string> estados = new List<string>();
        private List<string> alfaEntrada = new List<string>();
        private List<string> alfaSalida = new List<string>();
        private Dictionary<string, string> funcT = new Dictionary<string, string>();
        private Dictionary<string, string> funcS = new Dictionary<string, string>();
        

        public MooreGenerator()
        {
            InitializeComponent();
            
        }

        private void llenarListas(){
            this.comboBox7.ItemsSource = estados;
            if (estados.Count > 0) { this.comboBox7.SelectedIndex = 0; }
            
            this.comboBox1.ItemsSource = estados;
            this.comboBox2.ItemsSource = alfaEntrada;
            this.comboBox3.ItemsSource = estados;

            this.comboBox4.ItemsSource = estados;
            this.comboBox5.ItemsSource = alfaEntrada;
            this.comboBox6.ItemsSource = alfaSalida;
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            mm = null;
            this.Close();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {

            if (validarEntradas() && this.textBox7.Text.Length > 0 
                && this.textBox8.Text.Length > 0 && this.textBox9.Text.Length > 0 
                && estados.Count > 0 && alfaEntrada.Count > 0
                && alfaSalida.Count > 0 && funcS.Count > 0 && funcT.Count > 0)
            {
                llenarListas();


                mm = new Mealy_Moore();

                mm.estados = estados;
                mm.alfaEntrada = alfaEntrada;
                mm.alfaSalida = alfaSalida;
                mm.estadoInicial = (string)this.comboBox7.SelectedItem;
                mm.funcTransicion = funcT;
                mm.funcSalida = funcS;
                this.Close();
            }
            else
            {
                MessageBox.Show("Aún faltan campos por definir", "Aviso", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private bool validarEntradas()
        {
            string pattern = @"^\w[,\w]{0,}$";
            string pattern2 = @",{2,}";  // las , no coinciden con # elementos

            Regex rgx = new Regex(pattern);
            Regex comaReg = new Regex(pattern2);
            if (this.textBox7.Text.Length > 0)  // estados
            {
                if (rgx.IsMatch(this.textBox7.Text) && !comaReg.IsMatch(this.textBox7.Text))
                {
                    this.textBox7.Text = this.textBox7.Text.Replace(" ", String.Empty).Trim();
                    if (this.textBox7.Text.EndsWith(",")) { this.textBox7.Text = this.textBox7.Text.Substring(0, this.textBox7.Text.Length - 1); }
                    string[] tempEstados = this.textBox7.Text.Split(',');
                    estados = new List<string>();
                    foreach (string t in tempEstados)
                    {
                        if (!estados.Contains(t)) { estados.Add(t); }
                    }
                    if (estados.Count == 0) { return false; }
                }
                else
                {
                    MessageBox.Show("El formato de " + this.textBox7.Text + " NO es valido.\n"
                        + "Favor revise el campo de estados.", "Formato Invalido",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }
            else { estados = new List<string>(); }


            if (this.textBox8.Text.Length > 0) // alfabeto de entrada
            {
                if (rgx.IsMatch(this.textBox8.Text) && !comaReg.IsMatch(this.textBox8.Text))
                {
                    this.textBox8.Text = this.textBox8.Text.Replace(" ", String.Empty).Trim();
                    if (this.textBox8.Text.EndsWith(",")) { this.textBox8.Text = this.textBox8.Text.Substring(0, this.textBox8.Text.Length - 1); }
                    string[] temp = this.textBox8.Text.Split(',');
                    alfaEntrada = new List<string>();
                    foreach (string t in temp)
                    {
                        if (!alfaEntrada.Contains(t)) { alfaEntrada.Add(t); }
                    }
                    if (alfaEntrada.Count == 0) { return false; }
                }
                else
                {
                    MessageBox.Show("El formato de " + this.textBox8.Text + " NO es valido.\n"
                        + "Favor revise el campo del alfabeto de entrada.", "Formato Invalido",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }
            else { alfaEntrada = new List<string>(); }

            if (this.textBox9.Text.Length > 0)
            {
                if (rgx.IsMatch(this.textBox9.Text) && !comaReg.IsMatch(this.textBox9.Text))
                {
                    this.textBox9.Text = this.textBox9.Text.Replace(" ", String.Empty).Trim();
                    if (this.textBox9.Text.EndsWith(",")) { this.textBox9.Text = this.textBox9.Text.Substring(0, this.textBox9.Text.Length - 1); }
                    string[] temp = this.textBox9.Text.Split(',');
                    alfaSalida = new List<string>();
                    foreach (string t in temp)
                    {
                        if (!alfaSalida.Contains(t)) { alfaSalida.Add(t); }
                    }
                    if (alfaSalida.Count == 0) { return false; }
                }
                else
                {
                    MessageBox.Show("El formato de " + this.textBox9.Text + " NO es valido.\n"
                        + "Favor revise el campo del alfabeto de salida.", "Formato Invalido",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }
            else { alfaSalida = new List<string>(); }

            return true;
        }

        private void textBox7_LostFocus(object sender, RoutedEventArgs e)
        {
            if (validarEntradas()) { llenarListas(); }
        }

        private void textBox8_LostFocus(object sender, RoutedEventArgs e)
        {
            if (validarEntradas()) { llenarListas(); }
        }

        private void textBox9_LostFocus(object sender, RoutedEventArgs e)
        {
            if (validarEntradas()) { llenarListas(); }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (this.comboBox1.SelectedIndex >= 0 &&
                this.comboBox2.SelectedIndex >= 0 &&
                this.comboBox3.SelectedIndex >= 0)
            {
                string key = (string)this.comboBox1.SelectedItem + (string)this.comboBox2.SelectedItem;
                funcT[key] = (string)this.comboBox3.SelectedItem;
            }
            renderizarListaFuncT();
        }

        private void renderizarListaFuncT()
        {
            List<string> listTemp = new List<string>();

            foreach (KeyValuePair<string, string> entry in funcT)
            {
                listTemp.Add("δ(" + entry.Key + ") = " + entry.Value);
            }
            this.listView1.ItemsSource = listTemp;
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            if (this.comboBox4.SelectedIndex >= 0 &&
                this.comboBox5.SelectedIndex >= 0 &&
                this.comboBox6.SelectedIndex >= 0)
            {
                string key = (string)this.comboBox4.SelectedItem + (string)this.comboBox5.SelectedItem;
                funcS[key] = (string)this.comboBox6.SelectedItem;
            }
            renderizarListaFuncS();
        }

        private void renderizarListaFuncS()
        {
            List<string> listTemp = new List<string>();

            foreach (KeyValuePair<string, string> entry in funcS)
            {
                listTemp.Add("λ(" + entry.Key + ") = " + entry.Value);
            }
            this.listView2.ItemsSource = listTemp;
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (this.listView1.SelectedIndex >= 0)
            {
                string key = (string)this.listView1.SelectedItem;
                funcT.Remove(key.Substring(2, key.LastIndexOf(")") - 2));
                renderizarListaFuncT();
            }
        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            if (this.listView2.SelectedIndex >= 0)
            {
                string key = (string)this.listView2.SelectedItem;
                funcS.Remove(key.Substring(2, key.LastIndexOf(")") - 2));
                renderizarListaFuncS();
            }
        }
    }
}
