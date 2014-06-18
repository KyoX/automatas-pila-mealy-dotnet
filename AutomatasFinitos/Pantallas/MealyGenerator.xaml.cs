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

namespace AutomatasFinitos.Pantallas
{
    /// <summary>
    /// Lógica de interacción para MealyGenerator.xaml
    /// </summary>
    public partial class MealyGenerator : Window
    {
        private string funcionTrans = "";
        private string funcionSal = "";

        public MealyGenerator()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if(validarEntradas())
            {
                MealyMorreFileForm mmForm = new MealyMorreFileForm();
                MealyImplement mImpl = new MealyImplement();
                mmForm.estados = this.textBox7.Text;
                mmForm.alfabetoEntrada = this.textBox8.Text;
                mmForm.alfabetosalida = this.textBox9.Text;
                mmForm.estadoinicial = this.textBox10.Text;
                mmForm.funcionesTransición = this.funcionTrans;
                mmForm.funcionesSalida = this.funcionSal;

                if (mImpl.generateFile(mmForm)) { MessageBox.Show("Archivo creado exitosamente", "Mealy", MessageBoxButton.OK, MessageBoxImage.Information); }
                else { MessageBox.Show("No se pudo generar el archivo de definiciones","Mealy", MessageBoxButton.OK,MessageBoxImage.Warning); }
            }
        }

        private bool validarEntradas()
        {
            return true;
        }
    }
}
