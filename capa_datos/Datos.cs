using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using capa_modelo;
using System.Collections;
using SQLite;
using System.Collections.ObjectModel;

namespace capa_datos
{
    public class Datos
    {
        private SQLiteConnection _bd;

        public Datos() 
        {         
            _bd = new SQLiteConnection("TweetApp.db");    
            _bd.CreateTable<UserApp>();
            _bd.CreateTable<TweetProgramado>();
            _bd.CreateTable<FollowersDB>(); 
            _bd.CreateTable<Menciones>();
            _bd.CreateTable<Promocion>();
        }

        public int AgregarUsuario(UserApp p)
        {
            try
            {
                _bd.Insert(p);
                return 1;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public int AgregarTweetProgramado(TweetProgramado p)
        {
            try
            {
                _bd.Insert(p);
                return 0;
            }
            catch (Exception e)
            {
                return 1;
            }
        }

        /*
         * Aqui solo puedo obtener los mensajes
         * del usuario que ha iniciado la sesion
         * de otro modo me saldran todos 
         */
        public List<TweetProgramado> CargarTweetsProgramado(UserApp user)
        {
            string usuario = user.Usuario;
            int i;
            List<TweetProgramado> c = new List<TweetProgramado>(); ;

            c = _bd.Query<TweetProgramado>
                ("select "
             + "Id, usuario,titulo,programado,fechaProgramacion,imagen,arrayBytes"
             + " from TweetProgramado where usuario  like '" + usuario + "'" );

            return c;
        }

        public List<MiTweet> ObtenerTimeLine()
        {
            int i;
            List<MiTweet> c = new List<MiTweet>(); ;

            TableQuery<MiTweet> aux = _bd.Table<MiTweet>();

            for (i = 0; i < aux.Count(); i++)
            {
                c.Add(aux.ElementAt(i));
            }

            return c;
        }

        public UserApp ObtenerUsuario(string nombre,string contra)
        {
            List<UserApp> peep = new List<UserApp>();
            peep = _bd.Query<UserApp>
                ("select "
                + "IDUsuario, Usuario,Contrasena,Email,Nombre,Apellidos,ConsumerKey,ConsumerKeyApi,AccessToken,AccessTokenSecret,Twitter_ID"
                + " from UserApp where Usuario like '" +nombre + "'" +
                                    " and Contrasena like '"+contra+"'");
            if (peep.Count == 0)
                return null;
            else
            {
                return peep[0]; //devolvemos el primero
            }

        }

        public int EliminarTweetProgramado(string id)
        {
            // Borro la pelicula de la base de datos.
            try
            {
                _bd.Execute("Delete from TweetProgramado where id =" +  id );
            }
            catch (Exception e)
            {
                return 1;
            }

            return 0;
        }

        public int ModificarTweetProgramado(TweetProgramado tmp)
        {
            try
            {
                _bd.Update(tmp);
            }
            catch (Exception e)
            {
                return 1;
            }
            return 0;
        }


        public int CambiarClave(string clave, UserApp user)
        {
            try
            {
                _bd.Execute("Update UserApp set Contrasena = '" + clave + "'" +
                             " where IDUsuario = " +
                             user.IDUsuario + "");
            }
            catch (Exception e)
            {
                return 1;
            }

            return 0;
        }

        /*Agreaga mis tweets descargados de la Twitter*/
        public int AgregarMisTweets(List<MiTweet> misTweets)
        {
            int number = 1;
            foreach (MiTweet p in misTweets)
            {
                try
                {
                    _bd.Insert(p);
                    number = 0;
                }
                catch (Exception e)
                {
                    number =  1;
                }
            }
            return number;
        }

        /*Borra todas los followers que hay en la base de datos*/
        public int BorrarFollowers()
        {
            try
            {
                _bd.Execute("DELETE FROM FollowersDB");
                return 0;
            }
            catch (Exception e)
            {
                return 1;
            }

        }

        public int AgregarMisFollowers(List<FollowersDB> misFollowers)
        {
            int number = 1;
            if (BorrarFollowers() == 0)
            {
                foreach (FollowersDB p in misFollowers)
                {
                    try
                    {
                        _bd.Insert(p);
                        number = 0;
                    }
                    catch (Exception e)
                    {
                        number = 1;
                    }
                }
            }
            return number;
        }

        public List<FollowersDB> ObtenerFollowersDB(UserApp p)
        {
            List<FollowersDB> aux = new List<FollowersDB>(); 
            aux = _bd.Query<FollowersDB> ("select * from FollowersDB where IDUsuario ="+p.IDUsuario);
            return aux;
        }

        public int AgregarMisMenciones(List<Menciones> misMenciones,UserApp user)
        {
            int number = 1;
            if (BorrarMisMenciones(user) == 0)
            {
                foreach (Menciones p in misMenciones)
                {
                    try
                    {
                        _bd.Insert(p);
                        number = 0;
                    }
                    catch (Exception e)
                    {
                        number = 1;
                    }
                }
            }
            return number;
        }

        public List<Menciones> ObtenerMenciones(UserApp p)
        {
            List<Menciones> aux = new List<Menciones>();
            aux = _bd.Query<Menciones>("select * from Menciones where IDUsuario =" + p.IDUsuario);
            return aux;
        }


        private int BorrarMisMenciones(UserApp user)
        {
            try
            {
                _bd.Execute("DELETE FROM Menciones where IDUsuario = "+"'"+user.IDUsuario+"'");
                return 0;
            }
            catch (Exception e)
            {
                return 1;
            }
        }

        public int BorrarMencion(Menciones tmp)
        {
            try
            {
                _bd.Execute("DELETE FROM Menciones where idmenciones = " + "'" +tmp.idmenciones+ "'");
                return 0;
            }
            catch (Exception e)
            {
                return 1;
            }
        }


        public int InsertacionPromocion(Promocion tmp)
        {
            int number;
            try
            {
                _bd.Insert(tmp);
                number = 0;
            }
            catch (Exception e)
            {
                number = 1;
            }

            return number;
        }
        public int ActualizarPromocion(Promocion tmp)
        {
            int number;
            try
            {
                _bd.Update(tmp);
                number = 0;
            }
            catch (Exception e)
            {
                number = 1;
            }

            return number;
        }

        public ObservableCollection<Promocion> ObtenerPromociones()
        {
            int i;
            ObservableCollection<Promocion> c = new ObservableCollection<Promocion>(); ;

            TableQuery<Promocion> aux = _bd.Table<Promocion>();

            for (i = 0; i < aux.Count(); i++)
            {
                c.Add(aux.ElementAt(i));
            }

            return c;
        }










    }

}
