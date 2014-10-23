using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace Easy.Models
{
    public class DAOTarefas
    {
        public List<Tarefas> ListaTarefas()
        {
            List<Tarefas> lsTaf;

            lsTaf = new List<Tarefas>();

            //DEVERÁ SER APLICADA A CLAUSULA WHERE PELO ID DO USUARIO LOGADO

            try
            {
                SqlCommand sqlExec = new SqlCommand("SELECT * FROM TBTAREFAS ORDER BY DT_FIM", Connection.Conectar());
                SqlDataReader dr = sqlExec.ExecuteReader();

                while (dr.Read())
                {
                    lsTaf.Add
                    (
                        new Tarefas
                        {
                            IdTarefa = int.Parse(dr["IdTare"].ToString()),
                            Descricao = dr["Descricao"].ToString(),
                            DtInicio = Convert.ToDateTime(dr["Dt_Inicio"].ToString()).ToShortDateString(),
                            DtFim = Convert.ToDateTime(dr["Dt_Fim"].ToString()).ToShortDateString(),
                            Prioridade = dr["Prioridade"].ToString(),
                            Status = dr["Status"].ToString(),
                            Criador = new Usuario { IdUser = int.Parse(dr["IDUSER"].ToString()) },
                            Relacionado = new Usuario { IdUser = int.Parse(dr["IDUSER"].ToString()) }
                        }
                    );
                }

            }
            catch (SqlException sqlE)
            {
            }
            catch (Exception e)
            {
            }

            Connection.Desconectar();
            return lsTaf;
        }

        public void AddTarefa(Tarefas tar)
        {
            if (tar != null)
            {
                try
                {
                    SqlCommand sqlExec = new SqlCommand("INSERT INTO TBTAREFAS VALUES("+
                                                                                        "'"+tar.Descricao+"',"+
                                                                                        "'"+Convert.ToDateTime(tar.DtInicio)+"',"+
                                                                                        "'"+Convert.ToDateTime(tar.DtFim)+"'," +
                                                                                        "'"+tar.Prioridade+"',"+
                                                                                        "'A',"+
                                                                                        "1,"+
                                                                                        "2, null)", Connection.Conectar());
                    sqlExec.ExecuteNonQuery();
                }
                catch (SqlException sqlEx)
                {
                }
            }

        }
    }
}