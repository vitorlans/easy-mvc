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

        public Tarefas SelecionaTarefaId(int id)
        {
            Tarefas tar = new Tarefas();
            DAOUsuario dUser = new DAOUsuario();
            Usuario user = Usuario.VerificaSeOUsuarioEstaLogado();
            Empresas emp = Empresas.RecuperaEmpresaCookie();

            string relacionado = "";

            try
            {
                //SqlCommand sqlExec = new SqlCommand("SELECT * FROM TBTAREFAS WHERE IDTARE = "+id+" AND IDUSER = " + user.IdUser + " AND IDEMPR = " + emp.IdEmpresa + " AND STATUS = 'A' OR IDTARE = "+id+" AND IDUSERDEST = " + user.IdUser + " AND IDEMPR = " + emp.IdEmpresa + " AND STATUS = 'A'", Connection.Conectar());
                SqlCommand sqlExec = new SqlCommand("SELECT * FROM TBTAREFAS WHERE IDTARE = " + id + " AND IDUSER = " + user.IdUser + " AND IDEMPR = " + emp.IdEmpresa + " OR IDTARE = " + id + " AND IDUSERDEST = " + user.IdUser + " AND IDEMPR = " + emp.IdEmpresa + " ", Connection.Conectar());
                SqlDataReader dr = sqlExec.ExecuteReader();
                bool status = true;

                while (dr.Read())
                {
                    Usuario relac = dUser.RecuperaContato(dr["IDUSERDEST"].ToString());

                    if (relac.IdUser != 0)
                    {
                        relacionado = relac.Email.ToString().Trim();
                    }

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
                    tar.DtInicio = Convert.ToDateTime( dr["DT_INICIO"].ToString() ).ToShortDateString();
                    tar.DtFim = dr["DT_FIM"].ToString().Replace("00:00:00", "").Trim();
                    tar.Prioridade = dr["PRIORIDADE"].ToString();
                    tar.Status = dr["Status"].ToString();
                    tar.Criador = new Usuario { IdUser = int.Parse(dr["IDUSER"].ToString()) };
                    tar.Relacionado = relacionado;
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
                            Status = dr["Status"].ToString(),
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

        public List<Tarefas> ListaTarefasAvancada(string status, string prior, string tarefa, string dtinicio, string dtfim)
        {
            DAOUsuario dUser = new DAOUsuario();
            Usuario user = Usuario.VerificaSeOUsuarioEstaLogado();
            Empresas emp = Empresas.RecuperaEmpresaCookie();

            List<Tarefas> list = new List<Tarefas>();
            string sql="";
            bool statusTar = true;
            string data;
            string email = "";

            if(status != "")
            {
                if (status == "Ativas")
                    sql = " STATUS = 'A' ";

                else if (status == "Concluídas")
                    sql = " STATUS = 'C'";

                else if (status == "Canceladas")
                    sql = " STATUS = 'I'";

                else if (status == "Atrasadas")
                    sql = " STATUS = 'A' AND DT_FIM < GETDATE() AND DT_FIM <> '' "; 

                
                if(prior != "")
                {
                    if(prior == "Baixa")
                        sql += " AND PRIORIDADE = 'B' ";

                    else if(prior == "Media")
                        sql += " AND PRIORIDADE = 'M' ";

                    else if(prior == "Alta")
                        sql += " AND PRIORIDADE = 'A' ";
                }

                
                if(dtinicio != "")
                {
                    sql += " AND DT_INICIO >= '"+dtinicio+"' ";
                }

                if(dtfim != "")
                {
                    sql += " AND DT_FIM <= '"+dtfim+"' ";
                }

                if (tarefa == "Própria")
                    sql += " AND IDUSER = " + user.IdUser + " AND IDUSERDEST IS NULL";

                else if (tarefa == "Recebida")
                    sql += " AND IDUSERDEST = " + user.IdUser + " ";

                else if (tarefa == "Destinada")
                    sql += " AND IDUSER = " + user.IdUser + " AND IDUSERDEST <> " + user.IdUser + " ";

                else if (tarefa == "Todas")
                    sql += " AND IDUSERDEST = " + user.IdUser + " AND IDEMPR = " + emp.IdEmpresa + " OR " + sql + " ";

                
            }

            try
            {

                SqlCommand sqlBusca = new SqlCommand("SELECT * FROM TBTAREFAS WHERE "+sql+" AND IDUSER = "+user.IdUser+" AND IDEMPR = "+emp.IdEmpresa+" ", Connection.Conectar());
                SqlDataReader dr = sqlBusca.ExecuteReader();

                while (dr.Read())
                {
                    Usuario relac = dUser.RecuperaUsuario(dr["IDUSERDEST"].ToString());

                    if (dr["Status"].ToString() == "A")
                    {
                        statusTar = true;
                    }
                    else if (dr["Status"].ToString() == "I" || dr["Status"].ToString() == "C")
                    {
                        statusTar = false;
                    }

                    if (dr["Dt_Fim"].ToString() != "")
                        data = Convert.ToDateTime(dr["Dt_Fim"].ToString()).ToShortDateString();
                    else
                        data = dr["Dt_Fim"].ToString();

                    if (relac.IdUser != 0)
                    {
                        email = relac.Email.ToString().Trim();
                    }

                    list.Add
                      (
                          new Tarefas
                          {
                              IdTarefa = int.Parse(dr["IdTare"].ToString()),
                              Descricao = dr["Descricao"].ToString(),
                              DtInicio = Convert.ToDateTime(dr["Dt_Inicio"].ToString()).ToShortDateString(),
                              DtFim = data,
                              Prioridade = dr["Prioridade"].ToString(),
                              Status = dr["Status"].ToString(),
                              Criador = new Usuario { IdUser = int.Parse(dr["IDUSER"].ToString()) },
                              Relacionado = email
                          }
                      );
                }
            }
            catch(SqlException exsql)
            {
            }

            Connection.Desconectar();
            return list;
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
               /* if (tar.Status)
                {
                    status = "A";
                }
                else if (tar.Status == false)
                {
                    status = "I";
                }*/

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
                    sqlExec.Parameters.AddWithValue("status", "A");

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

        public List<String> ListaContatosTarefa()
        {
            Usuario us = Usuario.VerificaSeOUsuarioEstaLogado();
            Empresas emp = Empresas.RecuperaEmpresaCookie();

            List<String> listUs = new List<String>();

            SqlCommand sqlBusca = new SqlCommand
            (
               " SELECT U.EMAIL FROM TBUSUARIOS U"+
                " INNER JOIN VUSUACONT VU ON VU.IDUSER2 = U.IDUSER "+
                " AND VU.IDUSER = "+us.IdUser+" AND VU.IDEMPR = "+emp.IdEmpresa+" ", Connection.Conectar()
            );

            SqlDataReader dr = sqlBusca.ExecuteReader();

            while (dr.Read())
            {
                listUs.Add(dr["Email"].ToString());
            }

            Connection.Desconectar();

            return listUs;
        }

        public void AtivaDesativaTarefa(int id, string tipo)
        {
            string status = "A";

            if (tipo == "Cancelar")
            {
                status = "I";
            }

            try
            {
                SqlCommand sqlUpdate = new SqlCommand("UPDATE TBTAREFAS SET STATUS = '" + status + "' WHERE IDTARE = " + id + " ", Connection.Conectar());
                sqlUpdate.ExecuteNonQuery();

            }catch(SqlException eSql)
            {
            }

            Connection.Desconectar();
        }

        public List<string> ListaDadosInicial() //ALTERAR IDUSERS E IDEMPRS
        {
            Usuario user = Usuario.VerificaSeOUsuarioEstaLogado();
            Empresas emp = Empresas.RecuperaEmpresaCookie();

            List<string> listS = new List<string>();

            if (user != null && emp != null)
            {
                SqlCommand sqlBusca = new SqlCommand("SELECT Total1 = (SELECT count(*)Total1 FROM TBTAREFAS WHERE STATUS = 'A' AND IDUSER = " + user.IdUser + " AND IDEMPR = " + emp.IdEmpresa + " OR STATUS = 'A' AND IDUSERDEST = " + user.IdUser + " AND IDEMPR = " + emp.IdEmpresa + " ), " +
                                                     " Total2 = (SELECT count(*)Total2 FROM TBTAREFAS WHERE DT_FIM < GETDATE() AND DT_FIM <> '' AND STATUS = 'A' AND IDUSER = " + user.IdUser + " AND IDEMPR = " + emp.IdEmpresa + " OR DT_FIM < GETDATE() AND DT_FIM <> '' AND STATUS = 'A' AND IDUSERDEST = " + user.IdUser + " AND IDEMPR = " + emp.IdEmpresa + "), " +
                                                     " Total3 = (SELECT COUNT(*) FROM TBCOMPROMISSOS COMP " +
                                                                               " LEFT JOIN VCOMPUSER VUSER ON COMP.IDCOMP = VUSER.IDCOMP " +
                                                                               "      WHERE  COMP.IDUSER = " + user.IdUser + " AND COMP.STATUS IN ('A', 'P') AND COMP.IDEMPR = " + emp.IdEmpresa + " " +
                                                                               "      OR  VUSER.IDUSER = " + user.IdUser + " AND COMP.STATUS IN ('A', 'P') AND COMP.IDEMPR = " + emp.IdEmpresa + "), " +
                                                     " Total4 = (SELECT COUNT(*) FROM TBCOMPROMISSOS COMP " +
                                                                               " LEFT JOIN VCOMPUSER VUSER ON COMP.IDCOMP = VUSER.IDCOMP " +
                                                                               "     WHERE  COMP.IDUSER = " + user.IdUser + " AND COMP.STATUS = 'T' AND COMP.IDEMPR = " + emp.IdEmpresa + " " +
                                                                               "       OR  VUSER.IDUSER = " + user.IdUser + " AND COMP.STATUS = 'T' AND COMP.IDEMPR = " + emp.IdEmpresa + ")", Connection.Conectar());

                SqlDataReader dr = sqlBusca.ExecuteReader();

                if (dr.Read())
                {
                    listS.Add(dr["Total1"].ToString());
                    listS.Add(dr["Total2"].ToString());
                    listS.Add(dr["Total3"].ToString());
                    listS.Add(dr["Total4"].ToString());
                }
            }

            Connection.Desconectar();

            return listS;
        }
    }
}