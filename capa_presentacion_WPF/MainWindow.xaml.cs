using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using capa_modelo;  //por ahora no se usa
using capa_negocios;

namespace capa_presentacion_WPF
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Negocio neg;
        
        private bool validado;
        private int contador;

        public MainWindow()
        {
            InitializeComponent();
            neg = new Negocio();

            UserApp user = new UserApp("XXXXX","XXXXX","XXXXX",
                "XXXXX", "XXXXX", "XXXXX",
                "XXXXX",
                "XXXXX-XXXXX",
               "XXXXX");

            neg.AgregarUsuario(user);
            contador = 3;
        }

        private void crearUsuario_click(object sender, RoutedEventArgs e)
        {

            string usuario = txt_nombre.Text.ToString();
            string pass = txt_contra.Password.ToString();
           
            //comprobar en el base de datos que el usuario exista
            int resutl = neg.ComprobarUsuario(usuario, pass);
            if (resutl == 0) { 
                IniciarApp();
            }
            else
            {
                MostrarError();
            }
        }

        private void MostrarError()
        {
            contador--;


            info.Text = "Error de autenticacion Fallos restantes:" + contador;
            if(contador<0)
            {
                App.Current.Shutdown();
            }
        }

        private void IniciarApp()
        {
            VentanaGeneral miApp = new VentanaGeneral(neg, this);
            miApp.Show();
            // this.Close();
            this.Hide();
            Reiniciar();
        }

        private void Reiniciar()
        {
            txt_nombre.Text="";
            txt_contra.Clear();
            contador = 3;

        }
    }
}
