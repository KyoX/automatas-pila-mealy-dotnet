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
using System.Threading;

namespace AutomatasFinitos
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MooreImplement moore;
        PDAImplement pda;


        public MainWindow()
        {
            InitializeComponent();

            
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
            MooreGenerator mG = new MooreGenerator();
            PDAGenerator pila = new PDAGenerator();

            moore = null;
            pda = null;

            if (nombre.Contains("Moore")) 
            { 
                mG.ShowDialog();
                moore = new MooreImplement(mG.mm);
                this.label4.Content = "Tipo Automata : Moore";
            }
            else {
                pila.ShowDialog();
                pda = new PDAImplement(pila.pda);
                this.label4.Content = "Tipo Automata : PDA";
            }
        }

        // para ayuda
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            MenuItem source = (MenuItem)e.Source;
            string nombre = (string)source.Header;
            if (nombre.Contains("Acerca")) { new About().ShowDialog(); }
            else { new HowTo().ShowDialog(); }
        }

        // ejecuta la maquina de Moore o el automata de pila de acuerdo a la definición cargada
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            string[] cintaEntradas = this.textBox1.Text.Split(',');
            this.textBox2.Text = "";
            this.textBox3.Text = "";
            System.Threading.ManualResetEvent temporizador = new System.Threading.ManualResetEvent(false);

            string temp;
            if (moore != null)  // se quiere que se ejecute un autómata de moore
            {
                moore.inicializar(this.slider1.Value);  // setea los valores necesarios para inicializar el automata de moore
                foreach (string entrada in cintaEntradas)
                {
                    Console.WriteLine("---");
                    temporizador.WaitOne(2000);
                    Console.WriteLine("---");
                    if (moore.validateTransition(entrada))
                    {
                        temp = moore.generateOutputVal(entrada);
                        Console.WriteLine("Salida: " + temp);
                        this.textBox3.Text = this.textBox3.Text + temp;

                        temp = moore.generateTransition(entrada);
                        Console.WriteLine("Transición: " + moore.lastState + "," + entrada
                            + " -> " + temp);

                        this.textBox2.Text = moore.stack;
                    }
                    else
                    {
                        MessageBox.Show("El autómata no esta definido correctamente\n"
                            + "Falta la definición para δ(" + moore.lastState + "," + entrada + ")",
                            "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        break;
                    }
                }

                // pongo lo que quedó en el stack
                Console.WriteLine("Salida: " + moore.stack);
                this.textBox2.Text = moore.stack;
                this.textBox3.Text = this.textBox3.Text + moore.stack;
            }
            else
            {
                if (pda != null)
                {
                    pda.inicializar(this.slider1.Value);
                    string[] data;
                    foreach (string entrada in cintaEntradas)
                    {
                        Console.WriteLine("---");
                        temporizador.WaitOne(2000);
                        Console.WriteLine("---");
                        string tempStack = pda.popStack();
                        if (pda.validateTransition(entrada, tempStack))
                        {

                            temp = pda.generateTransition(entrada, tempStack);
                            data = temp.Split(',');

                            Console.WriteLine("Estados: " + pda.lastState + " -> " + data[0]);
                            Console.WriteLine("Al stack: " + data[1]);

                            pda.lastState = data[0];
                            pda.putInStack(data[1]);

                            Console.WriteLine(pda.stackToString());
                            this.textBox2.Text = pda.stackToString();

                        }
                        else
                        {
                            pda.putInStack(tempStack);
                            MessageBox.Show("El autómata no esta definido correctamente\n"
                           + "Falta la definición para δ(" + pda.lastState + "," + entrada + "," + tempStack + ")",
                           "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            break;
                        }
                    }

                    bool aceptado = pda.esAceptado();
                    Console.WriteLine("Es aceptado? : " + aceptado);
                    if (aceptado) { this.textBox3.Text = "Aceptado"; } else { this.textBox3.Text = "Rechazado"; }
                }
                else
                {

                    // no ha seleccionado a ningún tipo de automata
                    MessageBox.Show("No ha seleccionado ningún tipo de autómata.\n"
                           + "Dirijase al menú generar y seleccione un tipo para\n"
                           + "definirlo y poder seguir con la ejecución.",
                           "Seleccione un tipo de autómata", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
                
            }
            
        }

        // pone en blanco los campos y stack/salida, tambien la definición de los automatas
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            moore = null;
            pda = null;
            this.textBox1.Text = "";
            this.label4.Content = "Tipo Automata : ---";
        }


        /*
         * 
         * esta esto aquí porque la mierda de .net se negaba a cerrar la aplicación
         * así que hubo que forzar el cierre por las malas
         * 
        */
        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown(); 
        }
    }
}
