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
using AutomatasFinitos.Utils;



namespace AutomatasFinitos
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MealyImplement mealy;
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

            mealy = null;
            pda = null;
            if (nombre.Contains("Mealy")) 
            {
                if (mG.ShowDialog().GetValueOrDefault())
                {
                    mealy = new MealyImplement(mG.mm);
                    this.label4.Content = "Tipo Automata : Mealy";
                }
                else {
                    mealy = null;
                    pda = null;
                    this.label4.Content = "Tipo Automata : ---"; 
                }
            }
            else {
                if (pila.ShowDialog().GetValueOrDefault())
                {
                    pda = new PDAImplement(pila.pda);
                    this.label4.Content = "Tipo Automata : PDA";
                }
                else {
                    mealy = null;
                    pda = null;
                    this.label4.Content = "Tipo Automata : ---"; 
                }

            }
        }

        // para ayuda
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            MenuItem source = (MenuItem)e.Source;
            string nombre = (string)source.Header;
            if (nombre.Contains("Acerca")) { new About().ShowDialog(); }
          
        }

        // ejecuta la maquina de Mealy o el automata de pila de acuerdo a la definición cargada
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            //limpiar();
            string[] cintaEntradas = this.textBox1.Text.Split(',');
            this.textBox2.Text = "";
            this.textBox3.Text = "";
            this.textBox4.Text = "";

            string temp;
            if (mealy != null)  // se quiere que se ejecute un autómata de Mealy
            {
                this.label3.Content = "Estado: Realizando transiciones";
                mealy.inicializar();  // setea los valores necesarios para inicializar el automata de Mealy

                this.textBox4.Text = this.textBox4.Text
                            + "\n\t\tFunción de salida\t\t|\t\tFunción de transición\n";

                foreach (string entrada in cintaEntradas)
                {
                    this.textBox4.Text = this.textBox4.Text
                            + "\t\t-----------------------------------------------------------------------\n";
                    esperar1Seg();
                    if (mealy.validateTransition(entrada))
                    {
                        temp = mealy.generateOutputVal(entrada);
                        
                        this.textBox3.Text = this.textBox3.Text + temp;

                        String eAnt = mealy.lastState;
                        this.textBox4.Text = this.textBox4.Text
                            + "\t\tλ("
                            + eAnt + "," + entrada
                            + ") := "
                            + temp
                            + "\t\t|\t\t";
                      
                        temp = mealy.generateTransition(entrada);

                        this.textBox4.Text = this.textBox4.Text
                            + "\t\tδ("
                            + eAnt + "," + entrada
                            + ") := "
                            + temp
                            +"\n";
                       
                        this.textBox2.Text = mealy.stack;
                        System.Windows.Forms.Application.DoEvents();
                   }
                    else
                    {
                        MessageBox.Show("El autómata no esta definido correctamente\n"
                            + "Falta la definición para δ(" + mealy.lastState + "," + entrada + ")",
                            "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        break;
                    }
                }
                this.label3.Content = "Estado: listo";
            }
            else
            {
                if (pda != null)
                {
                    bool terminaPDA = true;
                    this.label3.Content = "Estado: Realizando transiciones";
                    pda.inicializar();
                    string[] data;

                    this.textBox4.Text = this.textBox4.Text
                            + "\n\t\tStack\t\t|\t\tFunción de transición\n";

                    foreach (string entrada in cintaEntradas)
                    {
                        esperar1Seg();
                        this.textBox4.Text = this.textBox4.Text
                           + "\t\t-----------------------------------------------------------------------\n";
                        string tempStack = pda.popStack();
                        if (pda.validateTransition(entrada, tempStack))
                        {

                            String eAnt = pda.lastState;

                            temp = pda.generateTransition(entrada, tempStack);
                            data = temp.Split(',');

                            this.textBox4.Text = this.textBox4.Text
                                + "\t\t"
                                +tempStack
                                + " -> "
                                + data[1]
                                + "\t\t|\t\t";

                            this.textBox4.Text = this.textBox4.Text
                                + "\t\tδ("
                                + eAnt + "," + entrada
                                + ") := "
                                + data[0]
                                + "\n";

                            pda.lastState = data[0];
                            pda.putInStack(data[1]);

                            this.textBox2.Text = pda.stackToString();
                            System.Windows.Forms.Application.DoEvents();
                        }
                        else 
                        {
                            pda.putInStack(tempStack);
                            MessageBox.Show("El autómata no esta definido correctamente\n"
                           + "Falta la definición para δ(" + pda.lastState + "," + entrada + "," + tempStack + ")",
                           "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            terminaPDA = false;
                            break;
                        }
                    }


                    bool aceptado = pda.esAceptado();
                    
                    if (terminaPDA && aceptado) {
                        this.textBox3.Text = "Aceptado";
                        this.textBox4.Text = this.textBox4.Text
                            + "\n\t\tLa palabra: "
                            + this.textBox1.Text.Replace(",", String.Empty)
                            + " Es aceptada por el PDA";
                        
                    }
                    else {
                        this.textBox3.Text = "Rechazado";
                        this.textBox4.Text = this.textBox4.Text
                            + "\n\t\tLa palabra: "
                            + this.textBox1.Text.Replace(",", String.Empty)
                            + " Es rechazada por el PDA";
                    }
                    this.label3.Content = "Estado: listo";
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

        /*
         * 
         * Primitiva que obliga al programa a esperar 1 segundo antes de seguir ejecutandose
         * 
         * */
        private void esperar1Seg()
        {
            long t0 = CurrentMillis.Millis;
            long t1;
            long t;
            do
            {
                t1 = CurrentMillis.Millis;
                t = t1 - t0;
            } while (t < 1000);

        }

        // pone en blanco los campos y stack/salida, tambien la definición de los automatas
        private void button3_Click(object sender, RoutedEventArgs e)
        {
              mealy = null;
              pda = null;
              limpiar();
        }

        private void limpiar(){
            
            this.textBox1.Text = "";
            this.textBox2.Text = "";
            this.textBox3.Text = "";
            this.textBox4.Text = "";
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