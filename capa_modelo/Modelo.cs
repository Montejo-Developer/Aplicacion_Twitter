using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace capa_modelo
{
    public class Modelo
    {
    }

    /*MiTweet, esta clase tendrá los siguientes atributos, basados en los atributos
        originales de la API de Twitter. Esta clase la usaremos para almacenar
        localmente los Twwets descargados a nuestra aplicación.*/
    public class MiTweet : IComparable
    {
        public long id { get; set; } //original.Id
        public string text { get; set; } //original.Text.Replace(",", "")
        public string time { get; set; } //original.CreatedAt
        public int retweets { get; set; }//original.RetweetCount
        public int favourites { get; set; }//original.FavouriteCount
        public int quote { get; set; }//original.quote_count
        public string nombre_usuario { get; set; }//original.User.Id
        public string imagen_usuario { get; set; }

        public string imagen_tweet { get; set; }


        public MiTweet() { }
        public MiTweet(long id, string text, string time, int retweets,
            int favourites, int quote, string nombre_usuario, string imagen_usuario)
        {
            this.id = id;
            this.text = text;
            this.time = time;
            this.retweets = retweets;
            this.favourites = favourites;
            this.quote = quote;
            this.nombre_usuario = nombre_usuario;
            this.imagen_usuario = imagen_usuario;
        }

        public int CompareTo(object obj)
        {
            MiTweet p = (MiTweet)obj;
            int resultado = this.id.CompareTo(p.id);
            if (resultado == 0)
            {
                return 0;
            }
            else
            {
                return resultado;
            }
        }
    }

    public class TweetProgramado
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string usuario { get; set; }
        [NotNull]
        public string titulo { get; set; } //supongo que es mas bien el texto
        [NotNull]
        public bool programado { get; set; }
        public DateTime fechaProgramacion { get; set; }  //Tiene que tener el mismo formato que
                                                         //MiTweet.Time
        public string imagen { get; set; }
        public TweetProgramado() { }
        public byte[] arrayBytes { get; set; }


        /*Constructores para modificar sin foto*/
        public TweetProgramado(int id, string usuario, string titulo, bool programado,
            DateTime fechaProgramacion)
        {
            this.Id = id;
            this.usuario = usuario;
            this.titulo = titulo;
            this.programado = programado;
            this.fechaProgramacion = fechaProgramacion;
            this.imagen = "";
        }

        /*Constructores para modificar con foto*/
        public TweetProgramado(int id, string usuario, string titulo, bool programado,
         DateTime fechaProgramacion, string imagen) : this(id, usuario, titulo, programado,
        fechaProgramacion)
        {
            this.imagen = imagen;
        }


        //constructor para tweet sin foto
        public TweetProgramado(string titulo, string usuario,
            bool programado, DateTime fechaProgramacion)
        {
            this.titulo = titulo;
            this.usuario = usuario;
            this.programado = programado;
            this.fechaProgramacion = fechaProgramacion;
            this.imagen = "";
        }


        //constructor para tweet con foto
        public TweetProgramado(string titulo, string usuario,
             bool programado, DateTime fechaProgramacion, string imagen)
            : this(titulo, usuario, programado, fechaProgramacion)
        {
            this.imagen = imagen;
        }


        public int CompareTo(object obj)
        {
            TweetProgramado p = (TweetProgramado)obj;
            int resultado = this.titulo.CompareTo(p.titulo);
            if (resultado == 0)
            {
                return 0;
            }
            else
            {
                return resultado;
            }
        }
    }

    public class UserApp
    {
        [PrimaryKey, AutoIncrement]
        public int IDUsuario { get; set; }
        public string Usuario { get; set; }
        public string Contrasena { get; set; }//la tengo que guarda en md5
        public string Email { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }

        public string Twitter_ID { get; set; } //dejo el campo vacio

        //claves para conectarse a la app
        public string ConsumerKey { get; set; }
        public string ConsumerKeyApi { get; set; }
        public string AccessToken { get; set; }
        public string AccessTokenSecret { get; set; }

        public UserApp() { }

        public UserApp(string user, string contra, string email, string nombre, string apellidos,
            string consumer1, string consumer2,
            string token1, string token2)
        {
            this.Usuario = user;
            this.Contrasena = contra;
            this.Email = email;
            this.Nombre = nombre;
            this.Apellidos = apellidos;
            this.ConsumerKey = consumer1;
            this.ConsumerKeyApi = consumer2;
            this.AccessToken = token1;
            this.AccessTokenSecret = token2;
            var temp1 = token1;
            string[] key = temp1.Split('-');
            this.Twitter_ID = key[0];
        }

        public UserApp(int IDUsuario, string user, string contra, string consumer1,
            string consumer2, string token1, string token2)
        {
            this.IDUsuario = IDUsuario;
            this.Usuario = user;
            this.Contrasena = contra;
            this.ConsumerKey = consumer1;
            this.ConsumerKeyApi = consumer2;
            this.AccessToken = token1;
            this.AccessTokenSecret = token2;
            var temp1 = token1;
            string[] key = temp1.Split('-');
            this.Twitter_ID = key[0];
        }
    }

    /*Estos followers se guardan en la base de datos
     Esta clase se usa en el proyecto de wpf
     y tiene nuevos  atributos*/
    public class FollowersDB
    {


        [PrimaryKey]
        public long id_follower { get; set; }   //campo que identfica al usuario de manera unica
        public String Nombre { get; set; }
        public String Imagen { get; set; }
        public DateTime Fecha_Creacion { get; set; }
        public int ContSeguidores { get; set; }
        public int ContFav { get; set; }
        public string estado { get; set; }
        public int IDUsuario { get; set; }   //responsable del tweet

        public FollowersDB()
        {

        }

        public FollowersDB(long idFollow, String nombre, String Imagen, DateTime fecha_creacion,
            int ContSeguidores)
        {
            this.id_follower = idFollow;
            this.Nombre = nombre;
            this.Imagen = Imagen;
            this.Fecha_Creacion = Fecha_Creacion;
            this.ContSeguidores = ContSeguidores;
        }
    }


    /*Estos son los followeres*/
    public class Followers
    {
        public long id_follower;
        public String Nombre { get; set; }
        public String Imagen { get; set; }
        public DateTime Fecha_Creacion { get; set; }
        public int ContSeguidores { get; set; }
        public int ContFav { get; set; }
        public string estado { get; set; }
        public int IDUsuario { get; set; }
        public bool following { get; set; } //para saber si me esta siguiendo el follower



        public Followers()
        {

        }

        public Followers(String nombre, String Imagen, DateTime fecha_creacion,
            int ContSeguidores)
        {
            this.Nombre = nombre;
            this.Imagen = Imagen;
            this.Fecha_Creacion = Fecha_Creacion;
            this.ContSeguidores = ContSeguidores;
        }
    }

    /*Clase para las menciones en Twitter*/
    public class Menciones { 
    
        [PrimaryKey, AutoIncrement]
        public int idmenciones { get; set; }
        public int IDUsuario { get; set; }
        
        public DateTime fecha { get; set; }
        public String fecha_corta { get { return fecha.ToShortDateString(); } }
        public string foto_usuario { get; set; }
        public string color_fondo { get; set; }
        public string nombre { get; set; }
        public string text { get; set; }
        public long TweetID { get; set; }

        /*referencia para ver si la mencion ha sido favorita,retweetado,contestada*/
        public bool favorito { get; set; }
        public bool retweet { get; set; }
        public bool contestado { get; set; }
        /*Para guardar las imagenes que nos interesan*/
        public string url_megusta;
        public string url_retweet;
        public string url_contestado;

        public Menciones()
        { }
    }

    /*Datos que guardamos para mostrar en la interfaz de UserApp*/
    public class DatosUsuario
    {
        public String imagen { get; set; }
        public DateTime fecha_alta_twitter { get; set; }
        public int ContSeguidores { get; set; }
        public string descripcion { get; set; }
        public int ContAmigos { get; set; }
        public string lengua { get; set; }
        public string location { get; set; }
        public string nombre { get; set; }


        public DatosUsuario(string imagen, DateTime fecha_alta_twitter,
            int ContSeguidores, int ContAmigos, string descripcion,  string lengua,
            string location)
        {
            this.imagen = imagen;
            this.fecha_alta_twitter = fecha_alta_twitter;
            this.ContAmigos = ContAmigos;
            this.ContSeguidores = ContSeguidores;
            this.descripcion = descripcion;
            this.lengua = lengua;
            this.location = location;
        }

    }

    //Clase para la tabla de promociones
    public class Promocion
    {
        [PrimaryKey, AutoIncrement]
        public int IdPromocionPK { get; set; }
        public long IdPromocion { get; set; }
        public string Titulo { get; set; }
        public DateTime fecha_ini { get; set; }
        public String fecha_fin_formato { get { return fecha_fin.ToShortDateString(); }  }
        public DateTime fecha_fin { get; set; }
        public long ganador { get; set; }
        public byte[] imagen { get; set; }


        public Promocion() { }

        public Promocion(String titulo,DateTime fecha_ini,DateTime fecha_fin, byte[]imagen)
        {
            this.Titulo = titulo;
            this.fecha_ini = fecha_ini;
            this.fecha_fin = fecha_fin;
            this.imagen = imagen;
        }



    }



}
