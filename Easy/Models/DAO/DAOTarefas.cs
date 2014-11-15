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
            DAOUsuario dUser = new DAOUsuario();
            Usuario user = Usuario.VerificaSeOUsuarioEstaLogado();
            Empresas emp = Empresas.RecuperaEmpresaCookie();

            try
            {
                SqlCommand sqlExec = new SqlCommand("SELECT * FROM TBTAREFAS WHERE IDTARE = "+id+" AND IDUSER = " + user.IdUser + " AND IDEMPR = " + emp.IdEmpresa + " AND STATUS = 'A' OR IDTARE = "+id+" AND IDUSERDEST = " + user.IdUser + " AND IDEMPR = " + emp.IdEmpresa + " AND STATUS = 'A'", Connection.Conectar());
                SqlDataReader dr = sqlExec.ExecuteReader();
                bool status = true;

                while (dr.Read())
                {
                    Usuario relac = dUser.RecuperaUsuario(dr["IDUSERDEST"].ToString());

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
                    tar.Relacionado = relac.Email.ToString().Trim();
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
            DAOUsuario dUser = new DAOUsuario();
            Usuario user = Usuario.VerificaSeOUsuarioEstaLogado();
            Empresas emp = Empresas.RecuperaEmpresaCookie();

            List<Tarefas> lsTaf;
            lsTaf = new List<Tarefas>();

            //DEVERÁ SER APLICADA A CLAUSULA WHERE PELO ID DO USUARIO LOGADO

            try
            {
                SqlCommand sqlExec = new SqlCommand("SELECT * FROM TBTAREFAS WHERE IDUSER = " + user.IdUser + " AND IDEMPR = " + emp.IdEmpresa + " AND STATUS = 'A' OR IDUSERDEST = " + user.IdUser + " AND IDEMPR = " + emp.IdEmpresa + " AND STATUS = 'A'", Connection.Conectar());
                SqlDataReader dr = sqlExec.ExecuteReader();
                bool status = true;
                string data;
                string email = "";

                while (dr.Read())
                {
                    Usuario relac = dUser.RecuperaUsuario(dr["IDUSERDEST"].ToString());

                    if (dr["Status"].ToString() == "A")
                    {
                        status = true;
                    }
                    else if (dr["Status"].ToString() == "I")
                    {
                        status = false;
                    }

                    if (dr["Dt_Fim"].ToString() != "")
                        data = Convert.ToDateTime(dr["Dt_Fim"].ToString()).ToShortDateString();
                    else
                        data = dr["Dt_Fim"].ToString();

                    if (relac.IdUser != 0)
                    {
                        email = relac.Email.ToString().Trim();
                    }

                    lsTaf.Add
                    (
                        new Tarefas
                        {
                            IdTarefa = int.Parse(dr["IdTare"].ToString()),
                            Descricao = dr["Descricao"].ToString(),
                            DtInicio = Convert.ToDateTime(dr["Dt_Inicio"].ToString()).ToShortDateString(),
                            DtFim = data,
                            Prioridade = dr["Prioridade"].ToString(),
                            Status = status,
                            Criador = new Usuario { IdUser = int.Parse(dr["IDUSER"].ToString()) },
                            Relacionado = email
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
                    string data = null;
                    string sql = "";

                    DAOUsuario dUser = new DAOUsuario();
                    Usuario idRelac = new Usuario();

                    if (tar.Relacionado != null)
                    {
                        idRelac = dUser.RecuperaUsuarioEmail(tar.Relacionado.ToString().Trim());
                    }

                    Usuario user = Usuario.VerificaSeOUsuarioEstaLogado();
                    Empresas emp = Empresas.RecuperaEmpresaCookie();


                    if (tar.DtFim != null)
                        data = Convert.ToDateTime(tar.DtFim).ToString();

                    if (idRelac.IdUser != 0)
                    {
                        sql = "INSERT INTO TBTAREFAS VALUES(" +
                                                    "'" + tar.Descricao + "'," +
                                                    "'" + Convert.ToDateTime(tar.DtInicio) + "'," +
                                                    "'" + data + "'," +
                                                    "'" + tar.Prioridade + "'," +
                                                    "'A'," +
                                                    "" + user.IdUser + "," +
                                                    "" + idRelac.IdUser + "," + emp.IdEmpresa + " )";
                    }
                    else
                    {
                        sql = "INSERT INTO TBTAREFAS VALUES(" +
                                                    "'" + tar.Descricao + "'," +
                                                    "'" + Convert.ToDateTime(tar.DtInicio) + "'," +
                                                    "'" + data + "'," +
                                                    "'" + tar.Prioridade + "'," +
                                                    "'A'," +
                                                    "" + user.IdUser + "," +
                                                    "NULL," + emp.IdEmpresa + " )";
                    }
                    
                    SqlCommand sqlExec= new SqlCommand(sql, Connection.Conectar());

                    sqlExec.ExecuteNonQuery();
                }
                catch (SqlException sqlEx)
                {
                }
            }

        }

        public void AtualizaTarefa(Tarefas tar)
        {
            DAOUsuario duser = new DAOUsuario();

            string email = (string)tar.Relacionado ?? "";

            Usuario user = duser.RecuperaUsuarioEmail(email.Trim());

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
                                             +"IDUSERDEST = @relac "
                                             +"WHERE IDTARE = @id", Connection.Conectar()
                    );

                    sqlExec.Parameters.AddWithValue("desc", tar.Descricao);
                    sqlExec.Parameters.AddWithValue("ini", tar.DtInicio);
                    sqlExec.Parameters.AddWithValue("fim", (object)tar.DtFim ?? DBNull.Value);
                    sqlExec.Parameters.AddWithValue("prior", tar.Prioridade);
                    sqlExec.Parameters.AddWithValue("status", status);

                    if(user.IdUser != 0)
                        sqlExec.Parameters.AddWithValue("relac", user.IdUser);
                    else
                        sqlExec.Parameters.AddWithValue("relac", DBNull.Value);

                    sqlExec.Parameters.AddWithValue("id", tar.IdTarefa);

                    sqlExec.ExecuteNonQuery();

                    Connection.Desconectar();
                }
                catch (SqlException sqlEx)
                {
                }
            }
        }

        public void ConcluirTarefa(int id)
        {
            try
            {
                SqlCommand sqlExec = new SqlCommand("UPDATE TBTAREFAS SET STATUS = 'C' WHERE IDTARE = " + id + "", Connection.Conectar());

                sqlExec.ExecuteNonQuery();
            }
            catch (SqlException sqlE)
            {
            }

            Connection.Desconectar();
        }

        //TESTE SOBRE RELACÇÃO DE USUARIOS NA INSERÇÃO DE TAREFAS

        public List<String> ListaContatoTarefa()
        {
            Usuario us = Usuario.VerificaSeOUsuarioEstaLogado();
            List<String> listUs = new List<String>();

            SqlCommand sqlBusca = new SqlCommand
            (
               " SELECT IDUSER, NOME, EMAIL FROM TBUSUARIOS WHERE IDUSER IN "+ 
                "("+
	                " SELECT vuser1.IDUSER2 FROM TBUSUARIOS tuser1"+
		                " INNER JOIN VUSUACONT vuser1 ON vuser1.IDUSER = tuser1.IDUSER"+
	                " WHERE tuser1.IDUSER = 1 "+
                ")", Connection.Conectar()
            );

            SqlDataReader dr = sqlBusca.ExecuteReader();

            while (dr.Read())
            {
                listUs.Add(dr["IDUSER"].ToString());
                listUs.Add(dr["NOME"].ToString());
                listUs.Add(dr["EMAIL"].ToString());
            }

            Connection.Desconectar();

            return listUs;
        }
    }
}