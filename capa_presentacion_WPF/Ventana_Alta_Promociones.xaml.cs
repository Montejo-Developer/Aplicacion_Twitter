using System;
using System.Collections.Generic;
using System.Drawing;
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
using Microsoft.Win32;

namespace capa_presentacion_WPF
{
    /// <summary>
    /// Lógica de interacción para Ventana_Alta_Promociones.xaml
    /// </summary>
    public partial class Ventana_Alta_Promociones : Window
    {
        byte[] arrayfoto;
        private Negocio neg;



        public Ventana_Alta_Promociones(Negocio neg)
        {
            InitializeComponent();
            this.neg = neg;
            //asi evito que sean nulos
            txt_fecha_inicio.SelectedDate = DateTime.Now.Date;
            txt_fecha_fin.SelectedDate = DateTime.Now.Date;
            arrayfoto = null;
        }

        private void btnFoto_Click(object sender, RoutedEventArgs e)
        {
            if (imgFoto.Source == null)
            {

                OpenFileDialog openFile = new OpenFileDialog();
                BitmapImage b = new BitmapImage();
                openFile.Title = "Seleccione la Imagen a Mostrar";
                openFile.Filter = "Todos(*.*) | *.*| Imagenes | *.jpg; *.gif; *.png; *.bmp";

                if (openFile.ShowDialog() == true)
                {
                    //direccion de la imagen
                    String url_imagen = openFile.FileName;
                    b.BeginInit();
                    b.UriSource = new Uri(openFile.FileName);
                    b.EndInit();
                    imgFoto.Stretch = Stretch.Fill;
                    imgFoto.Source = b;

                    ImageConverter _imageConverter = new ImageConverter();

                    //convertimos la foto en un array de bytes que sera lo que guardemos
                    // en la base de datos
                    arrayfoto = (byte[])_imageConverter.ConvertTo(
                        System.Drawing.Image.FromFile(url_imagen), typeof(byte[]));

                    btnFoto.Content = "Quitar Foto";
                }
            }

            else
            {
                imgFoto.Source = null;
                arrayfoto = null;
                btnFoto.Content = "Agregar Foto";
            }
        }

        private void btn_CrearMencion(object sender, RoutedEventArgs e)
        {
            DateTime fecha1 = txt_fecha_inicio.SelectedDate.Value.Date;
            DateTime fecha2 = txt_fecha_fin.SelectedDate.Value.Date;
            String titulo = txt_titulo.Text;
            byte[] foto = arrayfoto;
            if (foto == null || titulo.Equals(""))
            {
                infoText.Foreground = new SolidColorBrush(Colors.Red);
                infoText.Text = "No dejes ningun campo vacio";
            }
            else
            {
                Promocion tmp = new Promocion(titulo, fecha1, fecha2, foto);
                int number = neg.InsertarPromocion(tmp);
                string mensaje;
                if (number == 1)
                {
                    //no se ha podido llevar a cabo la promocion
                    mensaje = "No se podido llevar a cabo la introduccion de la promocion";
                    infoText.Foreground= new SolidColorBrush(Colors.Red);
                }
                else
                {
                    //se ha introducido correctamente la promocion
                    mensaje = "Promocion subida correctamente";
                    infoText.Foreground = new SolidColorBrush(Colors.Blue);
                }
                infoText.Text = mensaje;
            }
        }
    }
}
