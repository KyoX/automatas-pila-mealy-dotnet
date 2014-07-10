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
using AutomatasFinitos.Beans;

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
            //limpiar();
            string[] cintaEntradas = this.textBox1.Text.Split(',');
            this.textBox2.Text = "";
            this.textBox3.Text = "";
            String cadAct = "";
            String pila = "";
            System.Threading.ManualResetEvent temporizador = new System.Threading.ManualResetEvent(false);

            string temp;
            if (moore != null)  // se quiere que se ejecute un autómata de moore
            {
                moore.inicializar(this.slider1.Value);  // setea los valores necesarios para inicializar el automata de moore
                prepararCanvaMoore(); // dibuja los estados 

                foreach (string entrada in cintaEntradas)
                { 
                    Console.WriteLine("---");
                    temporizador.WaitOne(500);
                    Console.WriteLine("---");
                    if (moore.validateTransition(entrada))
                    {
                        temp = moore.generateOutputVal(entrada);
                        Console.WriteLine("Salida: " + temp);
                        this.textBox3.Text = this.textBox3.Text + temp;

                        String eAnt = moore.lastState;
                      
                        temp = moore.generateTransition(entrada);
                        Console.WriteLine("Transición: " + moore.lastState + "," + entrada
                            + " -> " + temp);

                        dibujarTransicionMoore(eAnt,moore.lastState);
                        
                        this.textBox2.Text = moore.stack;
                        eActual.Content = moore.lastState;
                        cadActual.Content = cadAct + entrada;
                        pila = pila + moore.stack;
                        CadPila.Content = pila + moore.stack;
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
                    prepararCanvaPDA(); // dibuja los estados 

                    this.PDAq.Visibility = Visibility.Visible;
                    this.PDAres.Visibility = Visibility.Visible;
                    
                    string[] data;
                    foreach (string entrada in cintaEntradas)
                    {
                        Console.WriteLine("---");
                        temporizador.WaitOne(500);
                        Console.WriteLine("---");
                        string tempStack = pda.popStack();
                        if (pda.validateTransition(entrada, tempStack))
                        {

                            temp = pda.generateTransition(entrada, tempStack);
                            data = temp.Split(',');

                            Console.WriteLine("Estados: " + pda.lastState + " -> " + data[0]);
                            Console.WriteLine("Al stack: " + data[1]);

                            String eAnt = pda.lastState;

                            pda.lastState = data[0];
                            pda.putInStack(data[1]);

                            dibujarTransicionPDA(eAnt, pda.lastState);
                        
                            cadActual.Content = cadAct + pda.stackToString(); 
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

                    this.PDAq.Visibility = Visibility.Visible;
                    this.PDAres.Visibility = Visibility.Visible;

                    bool aceptado = pda.esAceptado();
                    Console.WriteLine("Es aceptado? : " + aceptado);
                    if (aceptado) {
                        this.PDAres.Content = "Aceptado";
                        this.textBox3.Text = "Aceptado";
                    }
                    else {
                        this.PDAres.Content = "Rechazado";
                        this.textBox3.Text = "Rechazado"; }
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
              limpiar();
        }

        private void limpiar(){
            
            this.textBox1.Text = "";
            this.textBox2.Text = "";
            this.textBox3.Text = "";
            this.cadActual.Content = "";
            this.PDAres.Content = "";
            this.label4.Content = "Tipo Automata : ---";
            this.canva1.Visibility = Visibility.Hidden;
            this.eli1.Visibility = Visibility.Hidden; this.le1.Content = "";
            this.eli1.Fill = new SolidColorBrush(Colors.Blue);
            this.eli2.Visibility = Visibility.Hidden; this.le1.Content = "";
            this.eli2.Fill = new SolidColorBrush(Colors.Blue);
            this.eli3.Visibility = Visibility.Hidden; this.le1.Content = "";
            this.eli3.Fill = new SolidColorBrush(Colors.Blue);
            this.eli4.Visibility = Visibility.Hidden; this.le1.Content = "";
            this.eli4.Fill = new SolidColorBrush(Colors.Blue);
            this.eli5.Visibility = Visibility.Hidden; this.le1.Content = "";
            this.eli5.Fill = new SolidColorBrush(Colors.Blue);
            this.eli6.Visibility = Visibility.Hidden; this.le1.Content = "";
            this.eli6.Fill = new SolidColorBrush(Colors.Blue);
            this.eli7.Visibility = Visibility.Hidden; this.le1.Content = "";
            this.eli7.Fill = new SolidColorBrush(Colors.Blue);
            eActual.Content = "";
            cadActual.Content = "";
            CadPila.Content = "";        
        }

        private void prepararCanvaMoore(){
            List<string> temp = new List<string>(); 
            //Colorear el estado inicial
           temp = moore.obtenerListaEstados();
           int ini = temp.IndexOf(moore.lastState);

          switch (ini)
           {
               case 0:
                   this.eli1.Fill = new SolidColorBrush(Colors.Yellow);
                   break;
               case 1:
                   this.eli2.Fill = new SolidColorBrush(Colors.Yellow);
                   break;
               case 2:
                   this.eli3.Fill = new SolidColorBrush(Colors.Yellow);
                   break;
               case 3:
                   this.eli4.Fill = new SolidColorBrush(Colors.Yellow);
                   break;
               case 4:
                   this.eli5.Fill = new SolidColorBrush(Colors.Yellow);
                   break;
               case 5:
                   this.eli6.Fill = new SolidColorBrush(Colors.Yellow);
                   break;
               case 6:
                   this.eli7.Fill = new SolidColorBrush(Colors.Yellow);
                   break;
           }
           

            int a = temp.Count;
                                  
         for (int i = 0 ; i < a; i++)  
            {
                switch (i) {
                   case 0:
                        this.eli1.Visibility = Visibility.Visible;
                        this.le1.Content = temp[i];
                        break;
                    case 1:
                        this.eli2.Visibility = Visibility.Visible;
                        this.le2.Content = temp[i];
                        break;
                    case 2:
                        this.eli3.Visibility = Visibility.Visible;
                        this.le3.Content = temp[i];
                        break;
                    case 3:
                        this.eli4.Visibility = Visibility.Visible;
                        this.le4.Content = temp[i];
                        break;
                    case 4:
                        this.eli5.Visibility = Visibility.Visible;
                        this.le5.Content = temp[i];
                        break;
                    case 5:
                        this.eli6.Visibility = Visibility.Visible;
                        this.le6.Content = temp[i];
                        break;
                    case 6:
                        this.eli7.Visibility = Visibility.Visible;
                        this.le7.Content = temp[i];
                        break;                                  
                }
            }
        
         this.canva1.Visibility = Visibility.Visible;
        }

        private void prepararCanvaPDA()
        {
            List<string> temp = new List<string>();
            //Colorear el estado inicial
            temp = pda.obtenerListaEstados();
            int ini = temp.IndexOf(pda.lastState);

            switch (ini)
            {
                case 0:
                    this.eli1.Fill = new SolidColorBrush(Colors.Yellow);
                    break;
                case 1:
                    this.eli2.Fill = new SolidColorBrush(Colors.Yellow);
                    break;
                case 2:
                    this.eli3.Fill = new SolidColorBrush(Colors.Yellow);
                    break;
                case 3:
                    this.eli4.Fill = new SolidColorBrush(Colors.Yellow);
                    break;
                case 4:
                    this.eli5.Fill = new SolidColorBrush(Colors.Yellow);
                    break;
                case 5:
                    this.eli6.Fill = new SolidColorBrush(Colors.Yellow);
                    break;
                case 6:
                    this.eli7.Fill = new SolidColorBrush(Colors.Yellow);
                    break;
            }
            
            int a = temp.Count;

            for (int i = 0; i < a; i++)
            {
                switch (i)
                {
                    case 0:
                        this.eli1.Visibility = Visibility.Visible;
                        this.le1.Content = temp[i];
                        break;
                    case 1:
                        this.eli2.Visibility = Visibility.Visible;
                        this.le2.Content = temp[i];
                        break;
                    case 2:
                        this.eli3.Visibility = Visibility.Visible;
                        this.le3.Content = temp[i];
                        break;
                    case 3:
                        this.eli4.Visibility = Visibility.Visible;
                        this.le4.Content = temp[i];
                        break;
                    case 4:
                        this.eli5.Visibility = Visibility.Visible;
                        this.le5.Content = temp[i];
                        break;
                    case 5:
                        this.eli6.Visibility = Visibility.Visible;
                        this.le6.Content = temp[i];
                        break;
                    case 6:
                        this.eli7.Visibility = Visibility.Visible;
                        this.le7.Content = temp[i];
                        break;
                }
            }

            this.canva1.Visibility = Visibility.Visible;
        }

        private void dibujarTransicionMoore(String act, String sig)
        {
            List<string> temp = new List<string>();
            temp = moore.obtenerListaEstados();

            int a = temp.IndexOf(act);
            int b = temp.IndexOf(sig);

           switch (a)
            {
                case 0:
                    this.eli1.Fill = new SolidColorBrush(Colors.Blue);
                    break;
                case 1:
                    this.eli2.Fill = new SolidColorBrush(Colors.Blue);
                    break;
                case 2:
                    this.eli3.Fill = new SolidColorBrush(Colors.Blue);
                    break;
                case 3:
                    this.eli4.Fill = new SolidColorBrush(Colors.Blue);
                    break;
                case 4:
                    this.eli5.Fill = new SolidColorBrush(Colors.Blue);
                    break;
                case 5:
                    this.eli6.Fill = new SolidColorBrush(Colors.Blue);
                    break;
                case 6:
                    this.eli7.Fill = new SolidColorBrush(Colors.Blue);
                    break;
            }

           switch (b)
           {
               case 0:
                   this.eli1.Fill = new SolidColorBrush(Colors.Yellow);
                   break;
               case 1:
                   this.eli2.Fill = new SolidColorBrush(Colors.Yellow);
                   break;
               case 2:
                   this.eli3.Fill = new SolidColorBrush(Colors.Yellow);
                   break;
               case 3:
                   this.eli4.Fill = new SolidColorBrush(Colors.Yellow);
                   break;
               case 4:
                   this.eli5.Fill = new SolidColorBrush(Colors.Yellow);
                   break;
               case 5:
                   this.eli6.Fill = new SolidColorBrush(Colors.Yellow);
                   break;
               case 6:
                   this.eli7.Fill = new SolidColorBrush(Colors.Yellow);
                   break;
           }
        }

        private void dibujarTransicionPDA(String act, String sig)
        {
            List<string> temp = new List<string>();
            temp = pda.obtenerListaEstados();

            int a = temp.IndexOf(act);
            int b = temp.IndexOf(sig);

            switch (a)
            {
                case 0:
                    this.eli1.Fill = new SolidColorBrush(Colors.Blue);
                    break;
                case 1:
                    this.eli2.Fill = new SolidColorBrush(Colors.Blue);
                    break;
                case 2:
                    this.eli3.Fill = new SolidColorBrush(Colors.Blue);
                    break;
                case 3:
                    this.eli4.Fill = new SolidColorBrush(Colors.Blue);
                    break;
                case 4:
                    this.eli5.Fill = new SolidColorBrush(Colors.Blue);
                    break;
                case 5:
                    this.eli6.Fill = new SolidColorBrush(Colors.Blue);
                    break;
                case 6:
                    this.eli7.Fill = new SolidColorBrush(Colors.Blue);
                    break;
            }

            switch (b)
            {
                case 0:
                    this.eli1.Fill = new SolidColorBrush(Colors.Yellow);
                    break;
                case 1:
                    this.eli2.Fill = new SolidColorBrush(Colors.Yellow);
                    break;
                case 2:
                    this.eli3.Fill = new SolidColorBrush(Colors.Yellow);
                    break;
                case 3:
                    this.eli4.Fill = new SolidColorBrush(Colors.Yellow);
                    break;
                case 4:
                    this.eli5.Fill = new SolidColorBrush(Colors.Yellow);
                    break;
                case 5:
                    this.eli6.Fill = new SolidColorBrush(Colors.Yellow);
                    break;
                case 6:
                    this.eli7.Fill = new SolidColorBrush(Colors.Yellow);
                    break;
            }
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