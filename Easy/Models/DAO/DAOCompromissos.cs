using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Easy.Models
{
    public class DAOCompromissos
    {
        //Status será inserido como (T)erminado, (C)ancelado ou em (A)ndamento
        public void TerminarCompromisso(Compromissos Compromisso, Usuario User)
        {
            //Somente o criador do compromisso poderá alterar o status do mesmo
            if (Compromisso.Usuario.IdUser == User.IdUser)
            {
                try
                {
                    SqlCommand sqlTerminarCompromisso = new SqlCommand("UPDATE TBCOMPROMISSOS SET STATUS = 'T' where idcomp = @IDCOMP", Connection.Conectar());
                    sqlTerminarCompromisso.Parameters.AddWithValue("IDCOMP", Compromisso.IdComp);
                    sqlTerminarCompromisso.ExecuteNonQuery();
                }
                catch (SqlException sqlExcp)
                {
                }
                catch(Exception Erro)
                {
                }
                Connection.Desconectar();
            }
        }
        public void AlterarStatusComp(Compromissos Compromisso, Usuario User)
        {
            //Somente o criador do compromisso poderá alterar o status do mesmo
            if (Compromisso.Usuario.IdUser == User.IdUser)
            {
                try
                {
                    SqlCommand sqlComando = new SqlCommand();
                    sqlComando.Connection = Connection.Conectar();
                    
                    if(Compromisso.Status == "A")
                        sqlComando.CommandText = "UPDATE TBCOMPROMISSOS SET STATUS = 'C' WHERE IDCOMP = @IDCOMP";
                    else
                        sqlComando.CommandText = "UPDATE TBCOMPROMISSOS SET STATUS = 'A' WHERE IDCOMP = @IDCOMP";

                    sqlComando.Parameters.AddWithValue("IDCOMP", Compromisso.IdComp);
                    sqlComando.ExecuteNonQuery();
                }
                catch (SqlException sqlExcp)
                {
                }
                catch (Exception Erro)
                {
                }
                Connection.Desconectar();
            }
        }
        public List<Compromissos> ListarCompromissosData()
        {
            DAOUsuario DUser = new DAOUsuario();
            DAOEmpresas DEmpresa = new DAOEmpresas();
            List<Compromissos> ListaComp;
            ListaComp = new List<Compromissos>();
            Usuario User = Usuario.VerificaSeOUsuarioEstaLogado();

            try
            {
                int y;
                int x = 0;
                for (y = 1; y <= 2; y++)
                {
                    string strSelect;
                    if (y == 1)
                    {
                        strSelect = "SELECT * FROM TBCOMPROMISSOS CO, VCOMPUSER UC, TBUSUARIOS US WHERE CO.IDCOMP = UC.IDCOMP " +
                            "AND UC.IDUSER = US.IDUSER AND UC.IDUSER = " + User.IdUser;
                    }
                    else
                    {
                        strSelect = "SELECT * FROM TBCOMPROMISSOS WHERE IDUSER = " + User.IdUser;
                    }
                    SqlCommand sqlComando = new SqlCommand(strSelect, Connection.Conectar());
                    SqlDataReader dr = sqlComando.ExecuteReader();

                    while (dr.Read())
                    {
                        ListaComp.Add(
                            new Compromissos
                            {
                                IdComp = int.Parse(dr["IDCOMP"].ToString()),
                                Titulo = dr["TITULO"].ToString(),
                                Descricao = dr["DESCRICAO"].ToString(),
                                DataInicio = dr["DT_INICIO"].ToString(),
                                DataTermino = dr["DT_FIM"].ToString(),
                                Status = (dr["STATUS"].ToString()),
                                Usuario = DUser.RecuperaUsuario(dr["IDUSER"].ToString()),
                                Empresa = DEmpresa.RecuperarEmpresaId(dr["IDEMPR"].ToString())
                            }
                            );

                        string dtIni = Convert.ToDateTime(ListaComp[x].DataInicio).ToShortDateString();
                        string dtFim = Convert.ToDateTime(ListaComp[x].DataTermino).ToShortDateString();
                        string hrIni = Convert.ToDateTime(ListaComp[x].DataInicio).ToShortTimeString();
                        string hrFim = Convert.ToDateTime(ListaComp[x].DataTermino).ToShortTimeString();



                        if (dtIni == dtFim)
                        {
                            ListaComp[x].DataInicio = Convert.ToDateTime(ListaComp[x].DataInicio.ToString()).ToLongDateString() + " às " + hrIni.ToString() + " até ";
                            ListaComp[x].DataInicio = Compromissos.FormataTexto(ListaComp[x].DataInicio);
                            ListaComp[x].DataTermino = hrFim;
                        }
                        else
                        {
                            ListaComp[x].DataInicio = Convert.ToDateTime(ListaComp[x].DataInicio.ToString()).ToLongDateString() + " às " + hrIni.ToString() + " até ";
                            ListaComp[x].DataInicio = Compromissos.FormataTexto(ListaComp[x].DataInicio);
                            ListaComp[x].DataTermino = Convert.ToDateTime(ListaComp[x].DataTermino.ToString()).ToLongDateString() + " às " + hrIni.ToString();
                            ListaComp[x].DataTermino = Compromissos.FormataTexto(ListaComp[x].DataTermino);
                        }

                        x++;
                    }
                }
            }
            catch (SqlException sqlErro)
            {
            }
            catch (Exception erro)
            {
            }
            Connection.Desconectar();
            return ListaComp;
        }
        public List<Compromissos> ListarCompromissosTodos()
        {
            DAOUsuario DUser = new DAOUsuario();
            DAOEmpresas DEmpresa = new DAOEmpresas();
            List<Compromissos> ListaComp;
            ListaComp = new List<Compromissos>();

            try
            {
                SqlCommand sqlComando = new SqlCommand("SELECT * FROM TBCOMPROMISSOS", Connection.Conectar());
                SqlDataReader dr = sqlComando.ExecuteReader();

                while (dr.Read())
                {
                    ListaComp.Add(
                        new Compromissos
                        {
                            IdComp = int.Parse(dr["IDCOMP"].ToString()),
                            Titulo = dr["TITULO"].ToString(),
                            Descricao = dr["DESCRICAO"].ToString(),
                            DataInicio = dr["DT_INICIO"].ToString(),
                            DataTermino = dr["DT_FIM"].ToString(),
                            Status = (dr["STATUS"].ToString()),
                            Usuario = DUser.RecuperaUsuario(dr["IDUSER"].ToString()),
                            Empresa = DEmpresa.RecuperarEmpresaId(dr["IDEMPR"].ToString())
                        }
                        );
                } 
            }
            catch(SqlException sqlErro)
            {
            }
            catch(Exception erro)
            {
            }
            Connection.Desconectar();
            return ListaComp;
        }
        public void AddCompromisso(Compromissos Compromisso)
        {
            Compromisso.Status = "P";
            try
            {
                SqlCommand sqlExec = new SqlCommand("INSERT INTO TBCOMPROMISSOS VALUES (@TITULO, @DESCRICAO, @DT_INICIO, @DT_FIM, @STATUS, @IDUSER, @IDEMPR)", Connection.Conectar());

                //sqlExec.Parameters.AddWithValue("IDCOMP", Compromisso.IdComp);
                sqlExec.Parameters.AddWithValue("TITULO", Compromisso.Titulo);
                sqlExec.Parameters.AddWithValue("DESCRICAO", Compromisso.Descricao);
                sqlExec.Parameters.AddWithValue("DT_INICIO", Convert.ToDateTime(Compromisso.DataInicio));
                sqlExec.Parameters.AddWithValue("DT_FIM", Convert.ToDateTime(Compromisso.DataTermino));
                sqlExec.Parameters.AddWithValue("STATUS", Compromisso.Status);
                sqlExec.Parameters.AddWithValue("IDUSER", Compromisso.Usuario.IdUser);
                sqlExec.Parameters.AddWithValue("IDEMPR", (object)Compromisso.Empresa.IdEmpresa ?? DBNull.Value);

                sqlExec.ExecuteNonQuery();
            }
            catch (SqlException sqlEx)
            {
            }
            Connection.Desconectar();
        }
        public void EditarCompromisso(Compromissos Compromisso, Usuario User)
        {
            if (Compromisso.Usuario.IdUser == User.IdUser)
            {
                try
                {
                    SqlCommand sqlExec = new SqlCommand("UPDATE TBCOMPROMISSOS SET TITULO = @TITULO, DESCRICAO = @DESCRICAO, DT_INICIO = @DT_INICIO, DT_FIM = @DT_FIM WHERE IDCOMP = @IDCOMP", Connection.Conectar());

                    sqlExec.Parameters.AddWithValue("IDCOMP", Compromisso.IdComp);
                    sqlExec.Parameters.AddWithValue("TITULO", Compromisso.Titulo);
                    sqlExec.Parameters.AddWithValue("DESCRICAO", Compromisso.Descricao);
                    sqlExec.Parameters.AddWithValue("DT_INICIO", Convert.ToDateTime(Compromisso.DataInicio));
                    sqlExec.Parameters.AddWithValue("DT_FIM", Convert.ToDateTime(Compromisso.DataTermino));

                    sqlExec.ExecuteNonQuery();
                }
                catch (SqlException sqlEx)
                {
                }
                Connection.Desconectar();
            }

        }
        public Compromissos SelecionarCompromissoId(int id)
        {
            Compromissos Compromisso = new Compromissos();
            DAOEmpresas DEmpresa = new DAOEmpresas();
            DAOUsuario DUser = new DAOUsuario();
            try
            {
                SqlCommand sqlComando = new SqlCommand("SELECT * FROM TBCOMPROMISSOS WHERE IDCOMP = "+ id,Connection.Conectar());
                SqlDataReader dr = sqlComando.ExecuteReader();

                if (dr.Read())
                {
                    Compromisso.IdComp = int.Parse(dr["IDCOMP"].ToString());
                    Compromisso.Titulo = dr["TITULO"].ToString();
                    Compromisso.Descricao = dr["DESCRICAO"].ToString();
                    Compromisso.DataInicio = dr["DT_INICIO"].ToString();
                    Compromisso.DataTermino = dr["DT_FIM"].ToString();
                    Compromisso.Status = (dr["STATUS"].ToString());
                    Compromisso.Usuario = DUser.RecuperaUsuario(dr["IDUSER"].ToString());
                    Compromisso.Empresa = DEmpresa.RecuperarEmpresaId(dr["IDEMPR"].ToString());
                }
            }
            catch(SqlException e){}
            Connection.Desconectar();
            return Compromisso;
        }
        public void AddParticipantesCompromisso(Compromissos Comp, Usuario Part)
        {
            try
            {
                SqlCommand sqlExec = new SqlCommand("INSERT INTO VCOMPUSER VALUES (@IDCOMP, @IDUSER)", Connection.Conectar());

                //sqlExec.Parameters.AddWithValue("IDCOMP", Compromisso.IdComp);
                sqlExec.Parameters.AddWithValue("IDCOMP", Comp.IdComp);
                sqlExec.Parameters.AddWithValue("IDUSER", Part.IdUser);

                sqlExec.ExecuteNonQuery();
            }
            catch (SqlException sqlEx)
            {
            }
            Connection.Desconectar();
        }
        public int RecuperaIdUltimoCompromisso(int IdUser)
        {
            int IdComp = 0;
            try
            {
                SqlCommand sqlComando = new SqlCommand("SELECT IDCOMP FROM TBCOMPROMISSOS WHERE IDUSER = " + IdUser + "ORDER BY IDCOMP DESC", Connection.Conectar());
                SqlDataReader dr = sqlComando.ExecuteReader();

                if (dr.Read())
                {
                    IdComp = Int32.Parse(dr["IDCOMP"].ToString());
                }
            }
            catch (SqlException e) { }
            Connection.Desconectar();
            return IdComp;
        }
    }
}