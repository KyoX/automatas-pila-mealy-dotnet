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
using System.Windows.Navigation;
using System.Windows.Shapes;
using AutomatasFinitos.Implement;
using AutomatasFinitos.Pantallas;

namespace AutomatasFinitos
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool cargarPDA = true;
        bool inicializado = false;
        MealyImplement mealy = new MealyImplement();
        PDAImplement pda = new PDAImplement();


        public MainWindow()
        {
            InitializeComponent();

            
        }

        // carga el archivo de texto con las definiciones de la maquina de Mealy o el automata de pila
        // dependiendo del radio button seleccionado
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                if (cargarPDA)
                {
                    inicializado = pda.initFromFile(dlg.FileName);
                }
                else
                {
                    inicializado = mealy.initFromFile(dlg.FileName);
                }
            }
        }

        private void radioButton1_Checked(object sender, RoutedEventArgs e)
        {
            cargarPDA = true;
        }

        private void radioButton2_Checked(object sender, RoutedEventArgs e)
        {
            cargarPDA = false;
        }

        // para acciones
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem source = (MenuItem)e.Source;

            string nombre = (string)source.Header;
            switch (nombre)
            {
                case "Salir":
                    Application.Current.Shutdown();
                    break;
            }

        }

        // para generar
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            MenuItem source = (MenuItem)e.Source;
            string nombre = (string)source.Header;

            if (nombre.Contains("Mealy")) { new MealyGenerator().ShowDialog(); }
            else { new PDAGenerator().ShowDialog(); }
        }

        // para ayuda
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            MenuItem source = (MenuItem)e.Source;
            string nombre = (string)source.Header;
            if (nombre.Contains("Acerca")) { new About().ShowDialog(); }
            else { new HowTo().ShowDialog(); }
        }

        // ejecuta la maquina de Mealy o el automata de pila de acuerdo a la definición cargada
        private void button2_Click(object sender, RoutedEventArgs e)
        {

        }

        // pone en blanco los campos y stack/salida, tambien la definición de los automatas
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            mealy = new MealyImplement();
            pda = new PDAImplement();
            inicializado = false;
        }
    }
}
