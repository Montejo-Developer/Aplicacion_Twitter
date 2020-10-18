using System;
using System.Collections;
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
    /// Lógica de interacción para Ventana_TodosFollowers.xaml
    /// </summary>
    public partial class Ventana_TodosFollowers : Window
    {
        Negocio neg;
        List<FollowersDB> misFollowers;
        public Ventana_TodosFollowers(Negocio neg)
        {
            InitializeComponent();
            misFollowers = neg.ObtenerListaFollowersDB();
            data.ItemsSource = misFollowers;
            this.neg = neg;
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            // execute some code
            DataGridRow fila = (DataGridRow)sender ;
            int index = fila.GetIndex();
            FollowersDB temp =(FollowersDB) misFollowers[index];
            mostrarDetalles(temp);
        }

        private void mostrarDetalles(FollowersDB temp)
        {
            nombre_txt.Content = temp.Nombre;
            id_follower_txt.Text = temp.estado;
            fecha_creacion_txt.Content = temp.Fecha_Creacion.ToShortDateString();
            seguidores_txt.Content = temp.ContSeguidores;
            Imagen.ImageSource = new BitmapImage(new Uri(temp.Imagen, UriKind.Absolute));
        }

        private void Actualizar_BD_followers(object sender, RoutedEventArgs e)
        {
            int number = neg.ActualizarListaUsuario();
        }

        private void data_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid data = (DataGrid)sender;
            FollowersDB fila = (FollowersDB)data.SelectedItem;
            mostrarDetalles(fila);
        }
    }
}
