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
using System.Collections.ObjectModel;
using System.IO;


namespace capa_presentacion_WPF
{
    /// <summary>
    /// Lógica de interacción para Ventana_Promocion_Gestion.xaml
    /// </summary>
    public partial class Ventana_Promocion_Gestion : Window
    {
        private Negocio neg;
        private ObservableCollection<Promocion> l_usuarios;
        private CollectionViewSource MiVista;

        public Ventana_Promocion_Gestion(Negocio neg)
        {
            this.neg = neg;
            InitializeComponent();
            MiVista = (System.Windows.Data.CollectionViewSource)
                this.Resources["lista_usuarios"];
            l_usuarios = neg.getListPromociones();
            MiVista.Source = l_usuarios;
           

        }

        private void data_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid data = (DataGrid)sender;
            Promocion tmp_Promocion = (Promocion)data.SelectedItem;
            if (tmp_Promocion != null )
            {
                List<Followers> elegidos = neg.gestionPromocion(tmp_Promocion);
                indicadorNombre.Content = tmp_Promocion.ganador == 0 ? "Participantes" : "Ganador";
                data_concursantes.ItemsSource = null;
                data_concursantes.ItemsSource = elegidos;
                indicadorConcurso.Content = tmp_Promocion.Titulo;
                ImagePromocion.Source = ToImage(tmp_Promocion.imagen);
            }
        }

        public BitmapImage ToImage(byte[] array)
        {
            using (var ms = new System.IO.MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad; // here
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }



        private void onClick_PromocionTerminada(object sender, RoutedEventArgs e)
        {
            MiVista.Filter += new FilterEventHandler(FiltrarPromocionTerminada);
        }


        private void OnClick_PorPalabra(object sender, RoutedEventArgs e)
        {
            MiVista.Filter += new FilterEventHandler(FiltrarPromocionPalabra);

        }
      
        private void FiltrarPromocionPalabra(object sender, FilterEventArgs e)
        {
            Promocion usu = (Promocion)e.Item;

            if (usu != null)
            {
                if ((usu.Titulo.Contains(txt_palabra.Text) ) || (txt_palabra.Text == ""))
                {
                    e.Accepted = true;
                }
                else
                {
                    e.Accepted = false;
                }
            }
        }

        private void FiltrarPromocionTerminada(object sender, FilterEventArgs e)
        {
            Promocion usu = (Promocion)e.Item;
            DateTime time = DateTime.Now;
            if (usu != null)
            {
                int result = DateTime.Compare(time, usu.fecha_fin);
                if (result > 0)
                {
                    e.Accepted = true;
                }
                else
                {
                    e.Accepted = false;
                }
            }
        }
    }
}
