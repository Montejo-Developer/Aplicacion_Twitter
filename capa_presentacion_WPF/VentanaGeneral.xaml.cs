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
using System.Windows.Shapes;
using capa_modelo;
using capa_negocios;

namespace capa_presentacion_WPF
{
    /// <summary>
    /// Lógica de interacción para VentanaGeneral.xaml
    /// </summary>
    public partial class VentanaGeneral : Window
    {
        Negocio neg;
        MainWindow padre;
        DatosUsuario user;
        public VentanaGeneral(Negocio neg,MainWindow padre)
        {
            InitializeComponent();
            this.neg = neg;
            user = neg.DarDatosUsuario();
            Welcome.Content = "Bienvenido "+user.nombre;
            this.padre = padre;
            mostrarDatosUsuario();
        }

        private void mostrarDatosUsuario()
        {
            nombre_txt.Content = user.nombre;
            id_follower_txt.Content = user.location;
            fecha_creacion_txt.Content = user.fecha_alta_twitter;
            seguidores_txt.Content = user.ContSeguidores;
            Imagen.ImageSource = new BitmapImage(new Uri(user.imagen, UriKind.Absolute));
            descripcion_txt.Text = user.descripcion;
        }

        private void mTodos_Click(object sender, RoutedEventArgs e)
        {
            Ventana_TodosFollowers t = new Ventana_TodosFollowers(neg);
            t.Show();
        }

        private void mNoSigo_Click(object sender, RoutedEventArgs e)
        {
            Ventana_No_Sigo_Followers t = new Ventana_No_Sigo_Followers(neg);
            t.Show();

        }

        private void mRecibirMenciones_Click(object sender, RoutedEventArgs e)
        {
            VentanasMenciones t = new VentanasMenciones(neg);
            t.Show();
        }

        private void mGestionMenciones_Click(object sender, RoutedEventArgs e)
        {
            Ventana_Mencion_Gestion t = new Ventana_Mencion_Gestion(neg);
            t.Show();
        }

        private void mEliminarMenciones_Click(object sender, RoutedEventArgs e)
        {
          
        }

        private void mAltaPromociones_Click(object sender, RoutedEventArgs e)
        {
            Ventana_Alta_Promociones t = new Ventana_Alta_Promociones(neg);
            t.Show();
        }

        private void mNoMeSiguen_Click(object sender, RoutedEventArgs e)
        {
           Ventana_Antiguos_Followers t = new Ventana_Antiguos_Followers(neg);
            t.Show();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
        }


        private void Salir_Click(object sender, RoutedEventArgs e)
        {

            string messageBoxText = "¿Está seguro que quiere salir de la sesion?";
            string caption = "Saliendo de twitter";
            MessageBoxButton button = MessageBoxButton.OKCancel;
            MessageBoxImage icon = MessageBoxImage.Question;
            MessageBoxResult respuesta = MessageBox.Show(messageBoxText, caption, button, icon);
            if (respuesta == MessageBoxResult.OK)
            {
                padre.Show();
                this.Close();
            }
            else
            {
                VentanaGeneral t = new VentanaGeneral(neg, padre);
                t.Show();
            }

           
        }

        private void mPromocionesGestion_Click(object sender, RoutedEventArgs e)
        {
            Ventana_Promocion_Gestion t = new Ventana_Promocion_Gestion(neg);
            t.Show();
        }

        private void TweetApp_Closed(object sender, EventArgs e)
        {

            string messageBoxText = "¿Está seguro que quiere salir de la sesion?";
            string caption = "Saliendo de twitter";
            MessageBoxButton button = MessageBoxButton.OKCancel;
            MessageBoxImage icon = MessageBoxImage.Question;
            MessageBoxResult respuesta = MessageBox.Show(messageBoxText, caption, button, icon);
            if (respuesta == MessageBoxResult.OK)
            {
                padre.Show();
                this.Close();
            }
            else
            {
                VentanaGeneral t = new VentanaGeneral(neg,padre);
                t.Show();
            }
        }
    }
}
