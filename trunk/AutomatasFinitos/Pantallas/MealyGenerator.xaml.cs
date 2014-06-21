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
using AutomatasFinitos.Form;
using System.Text.RegularExpressions;
using AutomatasFinitos.Utils;

namespace AutomatasFinitos.Pantallas
{
    /// <summary>
    /// Lógica de interacción para MealyGenerator.xaml
    /// </summary>
    public partial class MealyGenerator : Window
    {
        private List<string> estados = new List<string>();
        private List<string> alfaEntrada = new List<string>();
        private List<string> alfaSalida = new List<string>();
        Dictionary<string, string> funcT = new Dictionary<string, string>();
        Dictionary<string, string> funcS = new Dictionary<string, string>();

        public MealyGenerator()
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
            this.Close();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            
            
            if(validarEntradas())
            {
                llenarListas();
               
                MealyMorreFileForm mmForm = new MealyMorreFileForm();
                MealyImplement mImpl = new MealyImplement();


                mmForm.estados = TextOperations.concatCsv_L(estados);
                mmForm.alfabetoEntrada = TextOperations.concatCsv_L(alfaEntrada);
                mmForm.alfabetosalida = TextOperations.concatCsv_L(alfaSalida);
                mmForm.estadoinicial = (string)this.comboBox7.SelectedItem;
                mmForm.funcionesTransición = TextOperations.concatCsv_D(funcT, ':');
                mmForm.funcionesSalida = TextOperations.concatCsv_D(funcS, ':');

                if (mImpl.generateFile(mmForm)) { MessageBox.Show("Archivo creado exitosamente", "Mealy", MessageBoxButton.OK, MessageBoxImage.Information); }
                else { MessageBox.Show("No se pudo generar el archivo de definiciones","Mealy", MessageBoxButton.OK,MessageBoxImage.Warning); }
                
                

            }
        }

        private bool validarEntradas()
        {
            string pattern = @"^\w[,\w]{0,}$";
            string pattern2 = @",{2,}";  // las , no coinciden con # elementos

            Regex rgx = new Regex(pattern);
            Regex comaReg = new Regex(pattern2);
            if (this.textBox7.Text.Length > 0)
            {
                if (rgx.IsMatch(this.textBox7.Text) && !comaReg.IsMatch(this.textBox7.Text))
                {
                    this.textBox7.Text = this.textBox7.Text.Replace(" ", String.Empty).Trim();
                    if (this.textBox7.Text.EndsWith(",")) { this.textBox7.Text = this.textBox7.Text.Substring(0, this.textBox7.Text.Length - 1); }
                    estados = new List<string>(this.textBox7.Text.Split(','));
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


            if (this.textBox8.Text.Length > 0)
            {
                if (rgx.IsMatch(this.textBox8.Text) && !comaReg.IsMatch(this.textBox8.Text))
                {
                    this.textBox8.Text = this.textBox8.Text.Replace(" ", String.Empty).Trim();
                    if (this.textBox8.Text.EndsWith(",")) { this.textBox8.Text = this.textBox8.Text.Substring(0, this.textBox8.Text.Length - 1); }
                    alfaEntrada = new List<string>(this.textBox8.Text.Split(','));
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
                    alfaSalida = new List<string>(this.textBox9.Text.Split(','));
                }
                else
                {
                    MessageBox.Show("El formato de " + this.textBox9.Text + " NO es valido.\n"
                        + "Favor revise el campo dl alfabeto de salida.", "Formato Invalido",
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
