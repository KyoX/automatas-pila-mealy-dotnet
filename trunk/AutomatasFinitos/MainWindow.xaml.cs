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

namespace AutomatasFinitos
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool cargarPDA = true;
        public MainWindow()
        {
            InitializeComponent();

            
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            Nullable<bool> result = dlg.ShowDialog();
            bool inicializado = false;
            MealyImplement mealy = new MealyImplement();
            PDAImplement pda = new PDAImplement();
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
    }
}
