using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using capa_datos;
using capa_modelo;
using System.Security.Cryptography;
using System.Collections;
using System.Collections.ObjectModel;

namespace capa_negocios
{

    public class Negocio
    {
        private Datos datos1;
        UserApp usuario;       //guardamos el usuario con el que inicio sesion
        Twitter mitwitter;
        private List<TweetProgramado> misTweetsProgramados;
        private List<MiTweet> misTweets;
        //guardamos la lista de followers que tenemos en nuestra base de datos
        private List<FollowersDB> misFollowersDB;
        private List<Followers> misFollowersActual;

        //guardamos la lista de menciones a mi usuario
        private List<Menciones> misMenciones;

        //guardamos la lista de promociones que hay en la base de datos
        private ObservableCollection<Promocion> misPromociones;

        




        public Negocio()
        {
            datos1 = new Datos();

        }

        /*Una vez que se que el usuario existe ya solo queda cargar los 
         tweets que nos interesan*/
        public void CargarProgramados()
        {
            //   mitwitter = new Twitter(usuario);
            misTweetsProgramados = datos1.CargarTweetsProgramado(usuario);
        }


        public List<TweetProgramado> getMisTweetsProgramados()
        {
            return misTweetsProgramados;
        }

        public List<MiTweet> getMisTweets()
        {
            return misTweets;
        }

        /*Agregando elementos a las tablas*/
        //agregando usuarios
        public int AgregarUsuario(UserApp pe)
        {
            pe.Contrasena = codifica_MD5(pe.Contrasena);
            int number = datos1.AgregarUsuario(pe);
            return number;
        }

        public void Crearfoto(TweetProgramado pe)
        {

        }

        public int AgregarTweet(TweetProgramado pe)
        {

            //cambiar y hacer la copia del archivo
            int number = datos1.AgregarTweetProgramado(pe);
            if (number == 0)  //se ha llevado a cabo la insercion 
                misTweetsProgramados = datos1.CargarTweetsProgramado(usuario);
            //actualizamos la lista de mensajes
            return number;
        }

        //modificar un tweet Programado
        public int ModificarTweetProgramado(TweetProgramado tmp)
        {

            int datos = datos1.ModificarTweetProgramado(tmp);
            if (datos == 0)
            {
                misTweetsProgramados = datos1.CargarTweetsProgramado(usuario);
                //actualizamos la lista de mensajes
            }
            return datos;
        }


        //comprobar que el usuario existe en la base de datos
        public int ComprobarUsuario(string user, string password)
        {

            UserApp temp = datos1.ObtenerUsuario(user, codifica_MD5(password));
            if (temp == null)
                return 1;
            else
            {
                usuario = temp;
                mitwitter = new Twitter(usuario);  //Cuando sabemos que hay 
                                                   //un usuario Creamos el objeto Twitter    
                                                   //funciones encargadas del timeLine
                IniciarTwitter();


                // misTweets = datos1.ObtenerTimeLine();

                string correo_encriptado = codifica_MD5(usuario.Email);
                if (usuario.Contrasena.Equals(correo_encriptado))  //primera vez que usa app
                    return 2;
                else         //ya ha entrado anteriormente
                    return 0;
            }
        }

        /*Una vez que sabemos que existe el usuario iniciamos todos los elementos     --------------------
         que tienen que ver con el twitter*/
        private void IniciarTwitter()
        {
            IniciarTimeLine();
            IniciarListaFollowersDB();
            IniciarFollowers();
            IniciarMisMenciones();
            IniciarPromociones();
        }

        private void IniciarPromociones()
        {
            misPromociones = datos1.ObtenerPromociones();
        }

        public string codifica_MD5(string pas)
        {
            int i;
            MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(pas));

            pas = "";
            for (i = 0; i < data.Length; i++)
            {
                pas = pas + data[i];
            }

            return pas;
        }

        public int CambiarContra(string clave)
        {
            return datos1.CambiarClave(codifica_MD5(clave), usuario);
        }


        public UserApp getUserApp()
        {
            return usuario;
        }

        public void setUserApp(UserApp tmp)
        {
            this.usuario = tmp;
        }

        public int EliminarTweetProgramado(string id)
        {
            int result = datos1.EliminarTweetProgramado(id);
            misTweetsProgramados = datos1.CargarTweetsProgramado(usuario);
            return result;

        }

        /*Funciones encargadas de que hacer operaciones con Twitter*/
        /*Envia un tweet directamente*/
        public void EnviarTweet(string text)
        {
            // mitwitter.Run();
            mitwitter.HacerNuevoTweet(text);
        }

        public void EnviarTweetFoto(string text, string foto)
        {
            mitwitter.HacerNuevoTweetConfoto(text, foto);
        }


        /*Esta funcion lo que hace es pilllar los tweets de twitter
         y meterlos en la base de datos local*/
        public void IniciarTimeLine()
        {
            misTweets = mitwitter.ObtenerTimeLine();
            //datos1.AgregarMisTweets(list);
        }

        /*Obtengo una lista con el nombre de los usuarios*/
        public List<Followers> ObtenerFollowers()
        {

            return misFollowersActual;
        }

        public void IniciarFollowers()
        {
            misFollowersActual = mitwitter.ObtenerFollowers(usuario);
        }


        /*Obtiene todos los tweets y comprueba que son anteriores a al fecha
         actual en otro caso lo publica*/
        public void RevisarTweets()
        {
            for (int i = 0; i < misTweetsProgramados.Count; i++)
            {
                DateTime t1 = DateTime.Now;
                DateTime t2 = misTweetsProgramados[i].fechaProgramacion;

                if (t1.CompareTo(t2) > 0 //t1 es posterior a t2
                    || !misTweetsProgramados[i].programado) //he modificado un tweet 
                                                            // para enviarlo ahora
                {
                    if (misTweetsProgramados[i].imagen.Equals(""))
                    {
                        mitwitter.HacerNuevoTweet(misTweetsProgramados[i].titulo); //enviamos el tweet
                    }
                    else
                    {
                        string url_imagen = misTweetsProgramados[i].imagen;
                        //guardamos el arraydebytes

                        //  mitwitter.HacerNuevoTweetConfoto(misTweetsProgramados[i].titulo,
                        //    url_imagen);
                        mitwitter.HacerNuevoTweetConfoto2(misTweetsProgramados[i].titulo,
                            misTweetsProgramados[i].arrayBytes);

                    }
                    datos1.EliminarTweetProgramado(misTweetsProgramados[i].Id.ToString());
                    misTweetsProgramados = datos1.CargarTweetsProgramado(usuario);//actualizo la lista

                }
            }
        }
        public int ObtenerMensajePorEnviar()
        {
            return misTweetsProgramados.Count();
        }

        public DatosUsuario DarDatosUsuario()
        {
            return mitwitter.DarDatosUsuario();
        }


        public TweetProgramado getTweetProgramdoPorId(int id)
        {
            for (int i = 0; i < misTweetsProgramados.Count; i++)
            {
                if (id == misTweetsProgramados[i].Id)
                {
                    return misTweetsProgramados[i];
                }
            }
            return null;
        }


        /*Esta funcion actualiza la lista de usuario que 
         tenemos en nuestra base de datos descargando un listado de followers
         de Twitter e insertandola en la base de de datos*/
        public int ActualizarListaUsuario()
        {
            List<FollowersDB> tmp = mitwitter.ObtenerFollowersDB();
            int sign = datos1.AgregarMisFollowers(tmp);
            return sign;
        }

        /*
         * Este metodo obtiene la lista de Followers guardados en la 
         * base de datos
         */
        public List<FollowersDB> ObtenerListaFollowersDB()
        {
            return misFollowersDB;
        }

        public void IniciarListaFollowersDB()
        {
            misFollowersDB = datos1.ObtenerFollowersDB(usuario);
        }


        //obtiene el listado del los usuarios que ya no me siguen
        public List<FollowersDB> ObtenerListada_Sin_Followers()
        {
            List<FollowersDB> followersDenegados = new List<FollowersDB>();
            for (int i = 0; i < misFollowersDB.Count; i++)
            {

                FollowersDB encontrado = null;
                for (int j = 0; j < misFollowersActual.Count && encontrado == null; j++)
                {
                    if (misFollowersDB[i].id_follower == misFollowersActual[i].id_follower)
                    {
                        encontrado = misFollowersDB[i];

                    }

                }
                if (encontrado != null)
                {
                    followersDenegados.Add(encontrado);
                }

            }
            return followersDenegados;
        }
        //followers que no sigo yo 
        public List<Followers> Obtener_Lista_followers_QUE_NO_SIGO()
        {
            List<Followers> followersDenegados = new List<Followers>();
            for (int j = 0; j < misFollowersActual.Count; j++)
            {
                if (misFollowersActual[j].following == false)
                {
                    followersDenegados.Add(misFollowersActual[j]);
                }
            }

            return followersDenegados;
        }

        //recupera las menciones de la base de datos
        public void IniciarMisMenciones()
        {
            misMenciones = datos1.ObtenerMenciones(usuario);

        }

        //Borra las menciones que antes existian y las cambia por las nuevas
        public List<Menciones> ObtenerMencionesPorUsuario()
        {
            List<Menciones> temp = mitwitter.ObtenerMencionesPorUsuario();
            datos1.AgregarMisMenciones(temp, usuario);    
            //guardando las menciones en la base de datos
            misMenciones = temp;
            return temp;

        }

        //obtiene un listado de las menciones del usuario en la base de datos
        public List<Menciones> ObtenerMencionesBD()
        {
            return misMenciones;
        }

        public void ObtenerMencionesPorPalabraClave(string clave)
        {
            mitwitter.ObtenerMencionesPorPalabraClave(clave);
        }

        //dar favorito
        public bool HacerFavorito(Menciones temp)
        {
            return mitwitter.HacerFavorito(temp);
        }

        //hacer retweet
        public void HacerRetweet(Menciones temp)
        {
            mitwitter.HacerRetweet(temp);
        }

        //contestar tweet
        public void ContestarTweet(Menciones temp, String texto)
        {
            mitwitter.ContestarTweet(temp, texto);
        }


        public int BorrarMencion(Menciones tmp)
        {
            int datos= datos1.BorrarMencion(tmp);
            if (datos == 0)//exito al borrar
            {
                misMenciones.Remove(tmp);
            }
            return datos;
        }


        public int InsertarPromocion(Promocion tmp)
        {
            tmp = mitwitter.InsertarPromocion(tmp);
            //guardamos en la base de datos;
            int number = datos1.InsertacionPromocion(tmp);
            if (number == 0)
            {
                misPromociones.Add(tmp);
            }
            return number;
        }


        public ObservableCollection<Promocion> getListPromociones()
        {
            return misPromociones;
        }

        /*En este metodo lo que hacemos es comprobar  si la promocion
         ha sido finalizada y tiene ganador: Obtenemos los datos del ganador y lo
         devolvemos.
         En caso contrario, comprobamos su fecha de finalizacion
         si es caduca: Sacamos la lista de concursante hacemos un random 
         y conseguimos el ganador , modificamos la base de datos
         y lo devolvemos*/
        public List<Followers> gestionPromocion(Promocion tmp_Promocion)
        {
            List<Followers> concursantes = new List<Followers>();
           
            if (tmp_Promocion.ganador != 0)
            {
                concursantes.Add(
                    mitwitter.obtenerInformacionUsuario(tmp_Promocion.ganador));
            }
            else
            {
                //obtener la lista de concursantes
                List<Followers> concursante_tmp = mitwitter.usuarioRetweet(tmp_Promocion);

                if (concursante_tmp.Count == 0)
                {
                    return null;
                }
                else
                {
                    DateTime time = DateTime.Now;
                    int result = DateTime.Compare(time, tmp_Promocion.fecha_fin);
                    if (result > 0) // finalizada
                    {
                        Followers tmp_ganador=ObtenerGanadorPromocion(concursante_tmp);
                        //con un ganador actualizamos en la base de datos
                        tmp_Promocion.ganador = tmp_ganador.id_follower;
                        //guardmos en la base datos
                        datos1.ActualizarPromocion(tmp_Promocion);

                        concursantes.Add(tmp_ganador);

                    }
                    else   //no finalizada
                    {
                        //mostramos la lista de concursantes
                        concursantes = concursante_tmp;
                    }
                }
              
            }

            return concursantes;
        }

        /**/
        private Followers ObtenerGanadorPromocion(List<Followers> concursante_tmp)
        {
            Random rnd = new Random();
            int index_ganador = rnd.Next(0, concursante_tmp.Count );
            return concursante_tmp[index_ganador];
        }
    }
}
