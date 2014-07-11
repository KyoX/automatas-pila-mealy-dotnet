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
using System.Text.RegularExpressions;

namespace AutomatasFinitos.Pantallas
{
    /// <summary>
    /// Lógica de interacción para PDAGenerator.xaml
    /// </summary>
    public partial class PDAGenerator : Window
    {
        public PDA pda { get; set; }

        private List<string> estados = new List<string>();
        private List<string> alfaEntrada = new List<string>();
        private List<string> alfaPila = new List<string>();
        private List<string> estadosFinales = new List<string>();
        private List<string> estadosFinalesDisp = new List<string>();
        private Dictionary<string, string> funcT = new Dictionary<string, string>();
        
        public PDAGenerator()
        {
            InitializeComponent();

            List<string> temp = new List<string>();
            this.comboBox9.ItemsSource = temp;

            for (int i = 2; i <= 15; i++)
            {
                temp.Add("" + i);
            }

            this.comboBox9.ItemsSource = temp;
            this.comboBox9.SelectedIndex = 0;
            if (validarEntradas()) { llenarListas(); }
        }

        private bool validarEntradas()
        {
            string pattern = @"^\w[,\w]{0,}$";
            string pattern2 = @",{2,}";  // las , no coinciden con # elementos

            Regex rgx = new Regex(pattern);
            Regex comaReg = new Regex(pattern2);

            string valor = (string)this.comboBox9.SelectedValue;
            estados = new List<string>();
            int cantEstados = Convert.ToInt32(valor);
            estadosFinalesDisp = new List<string>();
            for (int i = 0; i < cantEstados; i++)
            {
                estados.Add("q" + i);
                estadosFinalesDisp.Add("q" + i);
            }




            if (this.textBox2.Text.Length > 0)  // alfabeto de entrada
            {
                if (rgx.IsMatch(this.textBox2.Text) && !comaReg.IsMatch(this.textBox2.Text))
                {
                    this.textBox2.Text = this.textBox2.Text.Replace(" ", String.Empty).Trim();
                    if (this.textBox2.Text.EndsWith(",")) { this.textBox2.Text = this.textBox2.Text.Substring(0, this.textBox2.Text.Length - 1); }
                    string[] tempEstados = this.textBox2.Text.Split(',');
                    alfaEntrada = new List<string>();
                    foreach (string t in tempEstados)
                    {
                        if (!alfaEntrada.Contains(t)) { alfaEntrada.Add(t); }
                    }
                    if (alfaEntrada.Count == 0) { return false; }
                }
                else
                {
                    MessageBox.Show("El formato de " + this.textBox2.Text + " NO es valido.\n"
                        + "Favor revise el campo de estados.", "Formato Invalido",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }

            if (this.textBox3.Text.Length > 0)  // alfabeto de pila
            {
                if (rgx.IsMatch(this.textBox3.Text) && !comaReg.IsMatch(this.textBox3.Text))
                {
                    this.textBox3.Text = this.textBox3.Text.Replace(" ", String.Empty).Trim();
                    if (this.textBox3.Text.EndsWith(",")) { this.textBox3.Text = this.textBox3.Text.Substring(0, this.textBox3.Text.Length - 1); }
                    string[] tempEstados = this.textBox3.Text.Split(',');
                    alfaPila = new List<string>();
                    foreach (string t in tempEstados)
                    {
                        if (!alfaPila.Contains(t)) { alfaPila.Add(t); }
                    }
                    if (alfaPila.Count == 0) { return false; }
                }
                else
                {
                    MessageBox.Show("El formato de " + this.textBox3.Text + " NO es valido.\n"
                        + "Favor revise el campo de estados.", "Formato Invalido",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }


            return true;
        }

        private void llenarListas()
        {
            this.comboBox1.ItemsSource = estados;
            this.comboBox3.ItemsSource = estados;
            this.comboBox5.ItemsSource = estados;
            if (estados.Count > 0) { this.comboBox1.SelectedIndex = 0; }

            this.comboBox6.Items.Clear();
            foreach (string temp in estadosFinalesDisp) { 
                if (!estadosFinales.Contains(temp)) { this.comboBox6.Items.Add(temp); }
            }

            this.comboBox4.ItemsSource = alfaEntrada;

            if (!alfaPila.Contains("&")) { alfaPila.Add("&"); }
            this.comboBox2.ItemsSource = alfaPila;
            this.comboBox2.SelectedIndex = 0;
            this.comboBox7.ItemsSource = alfaPila;
            this.comboBox8.ItemsSource = alfaPila;

        }

        private void textBox1_LostFocus(object sender, RoutedEventArgs e)
        {
            if (validarEntradas()) { llenarListas(); }
        }

        private void textBox2_LostFocus(object sender, RoutedEventArgs e)
        {
            if (validarEntradas()) { llenarListas(); }
        }

        private void textBox3_LostFocus(object sender, RoutedEventArgs e)
        {
            if (validarEntradas()) { llenarListas(); }
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            if (this.comboBox6.SelectedIndex >= 0)
            {
                string temp = (string)this.comboBox6.SelectedItem;

                if (!estadosFinales.Contains(temp)) { estadosFinales.Add(temp); }
                this.listView2.Items.Add(temp);
                this.comboBox6.Items.RemoveAt(this.comboBox6.SelectedIndex);
                estadosFinalesDisp.Remove(temp);

            }
        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            if (this.listView2.SelectedIndex >= 0)
            {
                string temp = (string)this.listView2.SelectedItem;

                if (!estadosFinalesDisp.Contains(temp)) { estadosFinalesDisp.Add(temp); }
                this.comboBox6.Items.Add(temp);
                this.listView2.Items.RemoveAt(this.listView2.SelectedIndex);
                estadosFinales.Remove(temp);
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            string temp = (string) this.listView1.SelectedItem;

            funcT.Remove(temp);
            funcT.Remove(temp.Substring(2, temp.LastIndexOf(")") - 2));
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

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (this.comboBox3.SelectedIndex >= 0 &&
                this.comboBox4.SelectedIndex >= 0 &&
                this.comboBox5.SelectedIndex >= 0 &&
                this.comboBox7.SelectedIndex >= 0 &&
                this.comboBox8.SelectedIndex >= 0 )
            {
                string key = (string)this.comboBox3.SelectedItem 
                            + (string)this.comboBox4.SelectedItem 
                            + (string)this.comboBox7.SelectedItem;

                funcT[key] = (string)this.comboBox5.SelectedItem + "," + (string)this.comboBox8.SelectedItem;
            }
            renderizarListaFuncT();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if (estados.Count > 0 && estadosFinales.Count > 0
                && alfaEntrada.Count > 0 && alfaPila.Count > 0
                && funcT.Count > 0)
            {
                llenarListas();

                pda = new PDA();
                pda.alfaEntrada = alfaEntrada;
                pda.alfaPila = alfaPila;
                pda.estadoInicial = (string)this.comboBox1.SelectedItem;
                pda.estados = estados;
                pda.estadosFinales = estadosFinales;
                pda.funcTransicion = funcT;
                pda.simboloInicialPila = (string)this.comboBox2.SelectedItem;
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Aún faltan campos por definir", "Aviso", MessageBoxButton.OK, MessageBoxImage.Stop);
            }   
        }

        private void comboBox9_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (validarEntradas()) { llenarListas(); }
        }
    }
}
