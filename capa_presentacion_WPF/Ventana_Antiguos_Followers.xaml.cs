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
    /// Lógica de interacción para Ventana_Antiguos_Followers.xaml
    /// </summary>
    public partial class Ventana_Antiguos_Followers : Window
    {
        
        Negocio neg;
        List<FollowersDB> misFollowersDB;
        public Ventana_Antiguos_Followers(Negocio neg)
        {
            InitializeComponent();
            this.neg = neg;
            misFollowersDB = neg.ObtenerListada_Sin_Followers();
            data.ItemsSource = misFollowersDB;

        }



        private void mostrarDetalles(FollowersDB temp)
        {
            nombre_txt.Content = temp.Nombre;
            id_follower_txt.Text = temp.estado;
            fecha_creacion_txt.Content = temp.Fecha_Creacion.ToShortDateString();
            seguidores_txt.Content = temp.ContSeguidores;
            Imagen.ImageSource = new BitmapImage(new Uri(temp.Imagen, UriKind.Absolute));
        }


        private void data_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid data = (DataGrid)sender;


            FollowersDB temp = (FollowersDB)data.SelectedItem;
            
            mostrarDetalles(temp);
        }

    }
}
