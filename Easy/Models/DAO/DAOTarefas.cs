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

        public List<string> BuscaEmails(Tarefas tar)
        {
            List<string> emails = new List<string>();

            SqlCommand sqlQuery = new SqlCommand
                        ("SELECT"+
                         " CRIADOR = (SELECT EMAIL FROM TBUSUARIOS WHERE IDUSER = '"+tar.Criador+"',"+
                         " RELACIONADO = (SELECT EMAIL FROM TBUSUARIOS WHERE IDUSER = '"+tar.Relacionado+"')"
                        );
            SqlDataReader dr = sqlQuery.ExecuteReader();

            while (dr.Read())
            {
                emails.Add(dr["CRIADOR"].ToString());
                emails.Add(dr["RELACIONADO"].ToString());
            }

            return emails;
        }

        public Tarefas SelecionaTarefaId(int id)
        {
            Tarefas tar = new Tarefas();

            try
            {
                SqlCommand sqlExec = new SqlCommand("SELECT * FROM TBTAREFAS WHERE IDTARE = '"+id+"' ", Connection.Conectar());
                SqlDataReader dr = sqlExec.ExecuteReader();
                bool status = true;

                while (dr.Read())
                {
                    if (dr["Status"].ToString() == "A")
                    {
                        status = true;
                    }
                    else if (dr["Status"].ToString() == "I")
                    {
                        status = false;
                    }

                    tar.IdTarefa = int.Parse(dr["IDTARE"].ToString());
                    tar.Descricao = dr["DESCRICAO"].ToString();
                    tar.DtInicio = dr["DT_INICIO"].ToString();
                    tar.DtFim = dr["DT_FIM"].ToString();
                    tar.Prioridade = dr["PRIORIDADE"].ToString();
                    tar.Status = status;
                    tar.Criador = new Usuario { IdUser = int.Parse(dr["IDUSER"].ToString()) };
                    tar.Relacionado = new Usuario { IdUser = int.Parse(dr["IDUSERDEST"].ToString()) };
                    tar.Empresa = new Empresas { IdEmpresa = int.Parse(dr["IDEMPR"].ToString()) };

                }

            }
            catch (SqlException sqlE)
            {
            }
            catch (Exception e)
            {
            }

            Connection.Desconectar();
            return tar;
        }

        public List<Tarefas> ListaTarefas()
        {
            List<Tarefas> lsTaf;

            lsTaf = new List<Tarefas>();

            //DEVERÁ SER APLICADA A CLAUSULA WHERE PELO ID DO USUARIO LOGADO

            try
            {
                SqlCommand sqlExec = new SqlCommand("SELECT * FROM TBTAREFAS WHERE STATUS = 'A' ORDER BY DT_FIM", Connection.Conectar());
                SqlDataReader dr = sqlExec.ExecuteReader();
                bool status = true;

                while (dr.Read())
                {
                    if (dr["Status"].ToString() == "A")
                    {
                        status = true;
                    }
                    else if (dr["Status"].ToString() == "I")
                    {
                        status = false;
                    }

                    lsTaf.Add
                    (
                        new Tarefas
                        {
                            IdTarefa = int.Parse(dr["IdTare"].ToString()),
                            Descricao = dr["Descricao"].ToString(),
                            DtInicio = Convert.ToDateTime(dr["Dt_Inicio"].ToString()).ToShortDateString(),
                            DtFim = Convert.ToDateTime(dr["Dt_Fim"].ToString()).ToShortDateString(),
                            Prioridade = dr["Prioridade"].ToString(),
                            Status = status,
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
                    SqlCommand sqlExec = new SqlCommand
                    (
                        "INSERT INTO TBTAREFAS VALUES("+
                                                    "'"+tar.Descricao+"',"+
                                                    "'"+Convert.ToDateTime(tar.DtInicio)+"',"+
                                                    "'"+Convert.ToDateTime(tar.DtFim)+"'," +
                                                    "'"+tar.Prioridade+"',"+
                                                    "'A',"+
                                                    "1,"+
                                                    "2, null)", Connection.Conectar()
                    );
                    sqlExec.ExecuteNonQuery();
                }
                catch (SqlException sqlEx)
                {
                }
            }

        }

        public void AtualizaTarefa(Tarefas tar)
        {
            string status = "";
            if (tar != null)
            {
                if (tar.Status)
                {
                    status = "A";
                }
                else if (tar.Status == false)
                {
                    status = "I";
                }

                try
                {
                    SqlCommand sqlExec = new SqlCommand
                    (
                        "UPDATE TBTAREFAS SET "
                                             +"DESCRICAO = @desc, "
                                             +"DT_INICIO = @ini, "
                                             +"DT_FIM = @fim, "
                                             +"PRIORIDADE = @prior, "
                                             +"STATUS = @status, "
                                             +"IDUSER = @criador, "
                                             +"IDUSERDEST = @relac, "
                                             +"IDEMPR = @emp "
                                             +"WHERE IDTARE = @id", Connection.Conectar()
                    );

                    sqlExec.Parameters.AddWithValue("desc", tar.Descricao);
                    sqlExec.Parameters.AddWithValue("ini", tar.DtInicio);
                    sqlExec.Parameters.AddWithValue("fim", (object)tar.DtFim ?? DBNull.Value);
                    sqlExec.Parameters.AddWithValue("prior", tar.Prioridade);
                    sqlExec.Parameters.AddWithValue("status", status);
                    sqlExec.Parameters.AddWithValue("criador", 1);
                    sqlExec.Parameters.AddWithValue("relac", 1);
                    sqlExec.Parameters.AddWithValue("emp", 1);

                    sqlExec.Parameters.AddWithValue("id", tar.IdTarefa);

                    sqlExec.ExecuteNonQuery();

                    Connection.Desconectar();
                }
                catch (SqlException sqlEx)
                {
                }
            }
        }
    }
}