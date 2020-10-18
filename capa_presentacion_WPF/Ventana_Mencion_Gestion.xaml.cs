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
using capa_negocios;
using capa_modelo;

namespace capa_presentacion_WPF
{
    /// <summary>
    /// Lógica de interacción para Ventana_Mencion_Gestion.xaml
    /// </summary>
    public partial class Ventana_Mencion_Gestion : Window
    {
        Negocio neg;
        List<Menciones> misMenciones;
        Menciones elegida;
        bool salida = true;
        public Ventana_Mencion_Gestion(Negocio neg)
        {
            InitializeComponent();
            this.neg = neg;
            IniciarLista();
            salida = true;
            respuesta.Foreground= new SolidColorBrush(Colors.Black); 
        }
        //metodo que descarga la lista de negocio y la introduce en el data correcto
        public void IniciarLista()
        {
            misMenciones = neg.ObtenerMencionesPorUsuario();
            for (int i = 0; i < misMenciones.Count; i++)
            {
                misMenciones[i].url_megusta = misMenciones[i].favorito == false ?
                  "imagen/like_vacio.png" : "imagen/like_lleno.png";
                misMenciones[i].url_retweet = misMenciones[i].retweet == false ?
                 "imagen/retweet_vacio.png" : "imagen/retweet_lleno.png";
            }
            data.ItemsSource = null;
            data.ItemsSource = misMenciones;
        }


        private void onClick_Favorito(object sender, RoutedEventArgs e)
        {
            //no se ha sido posible la ejecucion 
            if ( elegida == null )
            {
                respuesta.Text = "No ha sido posible darle megusta";
            }
            else
            {
                bool resp=neg.HacerFavorito(elegida);
                if (resp)
                {
                    respuesta.Text = "Ahora te gusta este tweet";
                    imag_fav.Source = new BitmapImage(new Uri("imagen/like_lleno.png", UriKind.Relative));

                }
                else
                {
                    respuesta.Text = "Ahora te gusta este tweet";
                }
            }
        }

        private void onClick_Retweet(object sender, RoutedEventArgs e)
        {
            if (elegida != null)
            {
                neg.HacerRetweet(elegida);
               imag_rt.Source = new BitmapImage(new Uri("imagen/retweet_lleno.png", UriKind.Relative));
                respuesta.Text = "Has hecho retweet a  este tweet";

            }
        }

        private void OnClick_Reply(object sender, RoutedEventArgs e)
        {
            String texto = text.Text;
            if (elegida != null)
            {
                neg.ContestarTweet(elegida, texto);
                respuesta.Text = "Has enviado tu respuesta!!";
            }
        }
        
        private void onClick_Delete(object sender, RoutedEventArgs e)
        {
            if (elegida != null)
            {
                int number=neg.BorrarMencion(elegida);
                if (number == 0)
                {
                    respuesta.Text = "Has borrado la mencion!!";
                    misMenciones = null;
                    misMenciones = neg.ObtenerMencionesBD();
                    data.ItemsSource = null;
                    data.ItemsSource = misMenciones;
                    if (misMenciones.Count == 0)
                        mostrarDatos(null);
                    else
                    {
                        elegida = misMenciones[0];
                        mostrarDatos(elegida);
                    }
                }
                else
                {
                    respuesta.Text = "No se ha borrado la mencion";
                }
            }
        }

        private void mostrarDatos(Menciones fila)
        {
            Imagen.ImageSource = new BitmapImage(new Uri(elegida.foto_usuario, UriKind.Absolute));
            infoTweet.Text = fila.text;

            if (fila.retweet == true)
            {
                imag_rt.Source = new BitmapImage(new Uri("imagen/retweet_lleno.png", UriKind.Relative));
            }
            else
            {
                imag_rt.Source = new BitmapImage(new Uri("imagen/retweet_vacio.png", UriKind.Relative));
            }
            if (fila.favorito == true)
            {
                imag_fav.Source = new BitmapImage(new Uri("imagen/like_lleno.png", UriKind.Relative));
            }
            else
            {
                imag_fav.Source = new BitmapImage(new Uri("imagen/like_vacio.png", UriKind.Relative));
            }
        }


        private void data_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid data = (DataGrid)sender;
            Menciones fila = (Menciones)data.SelectedItem;
            respuesta.Text = "";
            elegida = fila;
            if (fila == null)
            {
                respuesta.Text = "No ha menciones disponibles";
            }
            else
            {
                mostrarDatos(fila);
            }

        }
        private void OnClick_Mostrar(object sender, RoutedEventArgs e)
        {
            salida = salida == true?false:true;
            if (salida == true)
            {
                infoTweet.Visibility = Visibility.Collapsed;
                infoReply.Visibility = Visibility.Visible;
                btn_reply.Content = "Ver tweet";
            }
            else
            {
                infoTweet.Visibility = Visibility.Visible;
                infoReply.Visibility = Visibility.Collapsed;
                btn_reply.Content = "Contestar";
            }
        }
    }
}
