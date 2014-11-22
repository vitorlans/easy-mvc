using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Sql;
using System.Data.SqlClient;

namespace Easy.Models
{
    public class DAOComentarios
    {

        public List<Comentarios> ListaComentarios(int id)
        {
            DAOTarefas tar = new DAOTarefas();
            DAOUsuario dUser = new DAOUsuario();

            Usuario user = new Usuario();
            List<Comentarios> list = new List<Comentarios>();

            SqlCommand sqlBusca = new SqlCommand("SELECT * FROM VTARECOMENT WHERE IDTARE = "+id+"", Connection.Conectar());
            SqlDataReader dr = sqlBusca.ExecuteReader();

            while (dr.Read())
            {
                user = dUser.RecuperaUsuario(dr["iduser"].ToString());

                list.Add
                    (
                        new Comentarios
                        {
                            IdComent = int.Parse(dr["IDCOMENT"].ToString()),
                            IdTarefa = tar.SelecionaTarefaId(id),
                            Comentario = dr["COMENTARIO"].ToString(),
                            IdUser = user,
                            DataComentario = Convert.ToDateTime( dr["dtcomentario"].ToString())
                        }
                    );
            }

            return list;
        }

        public void AdicioonarComentario(int idTare, string Comentario)
        {
            DAOUsuario dUser = new DAOUsuario();
            Usuario user = Usuario.VerificaSeOUsuarioEstaLogado();

            try
            {
                SqlCommand sqlExec = new SqlCommand("INSERT INTO VTARECOMENT VALUES("+idTare+", '"+Comentario+"', "+user.IdUser+", GETDATE())", Connection.Conectar());
                sqlExec.ExecuteNonQuery();

            }
            catch(SqlException eSql)
            {
            }

            Connection.Desconectar();
        }
    }
}