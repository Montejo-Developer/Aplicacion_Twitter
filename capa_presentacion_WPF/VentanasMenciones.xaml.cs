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
    /// Lógica de interacción para VentanasMenciones.xaml
    /// </summary>
    public partial class VentanasMenciones : Window
    {
        Negocio neg;

        public VentanasMenciones(Negocio neg)
        {
            InitializeComponent();
            this.neg = neg;
            data.ItemsSource = neg.ObtenerMencionesBD();
        }

        /*Prueba para formar el objeto mencion*/
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Usuario_Click(object sender, RoutedEventArgs e)
        {
            data.ItemsSource = neg.ObtenerMencionesPorUsuario();
        }

        private void Palabra_Click(object sender, RoutedEventArgs e)
        {
            string palabra1 = "spam ";
            neg.ObtenerMencionesPorPalabraClave(palabra1);
        }
    }
}
