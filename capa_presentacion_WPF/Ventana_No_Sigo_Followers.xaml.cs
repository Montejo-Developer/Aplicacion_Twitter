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
    /// Lógica de interacción para Ventana_No_Sigo_Followers.xaml
    /// </summary>
    public partial class Ventana_No_Sigo_Followers : Window
    {
        Negocio neg;
        List<Followers> misFollowersDB;  
        public Ventana_No_Sigo_Followers(Negocio neg)
        {
           
            InitializeComponent();
            misFollowersDB = neg.Obtener_Lista_followers_QUE_NO_SIGO();
            data.ItemsSource = misFollowersDB;

            this.neg = neg;
        }

        private void data_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid data = (DataGrid)sender;
            Followers fila = (Followers)data.SelectedItem;
            mostrarDetalles(fila);
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            // execute some code
            DataGridRow fila = (DataGridRow)sender;
            int index = fila.GetIndex();
            Followers temp = misFollowersDB[index];
            mostrarDetalles(temp);
        }

        private void mostrarDetalles(Followers temp)
        {
            nombre_txt.Content = temp.Nombre;
            id_follower_txt.Text = temp.estado;
            fecha_creacion_txt.Content = temp.Fecha_Creacion.ToShortDateString();
            seguidores_txt.Content = temp.ContSeguidores;
            Imagen.ImageSource = new BitmapImage(new Uri(temp.Imagen, UriKind.Absolute));

        }
    }
}
