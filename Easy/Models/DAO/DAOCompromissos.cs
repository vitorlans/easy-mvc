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
            Compromissos Compr = new Compromissos();
            try
            {
                SqlCommand sqlComando = new SqlCommand("SELECT * FROM TBCOMPROMISSOS WHERE DT_FIM > getdate() order by dt_inicio", Connection.Conectar());
                SqlDataReader dr = sqlComando.ExecuteReader();

                while(dr.Read())
                {
                    Compr.IdComp = int.Parse(dr["IDCOMP"].ToString());
                    Compr.Titulo = dr["TITULO"].ToString();
                    Compr.Descricao = dr["DESCRICAO"].ToString();
                    Compr.DataInicio = Convert.ToDateTime(dr["DT_INICIO"].ToString()).ToShortDateString();
                    Compr.DataTermino = dr["DT_FIM"].ToString();
                    Compr.Status = (dr["STATUS"].ToString());
                    Compr.Usuario = DUser.RecuperaUsuario(dr["IDUSER"].ToString());
                    Compr.Empresa = DEmpresa.RecuperarEmpresaId(dr["IDEMPR"].ToString();
                    


                    ListaComp.Add(Compr);
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
        public List<Compromissos> ListarCompromissosTodos()
        {
            DAOUsuario DUser = new DAOUsuario();
            DAOEmpresas DEmpresa = new DAOEmpresas();
            List<Compromissos> ListaComp;
            ListaComp = new List<Compromissos>();
            Compromissos Compr = new Compromissos();
            try
            {
                SqlCommand sqlComando = new SqlCommand("SELECT * FROM TBCOMPROMISSOS", Connection.Conectar());
                SqlDataReader dr = sqlComando.ExecuteReader();

                while(dr.Read())
                {
                    Compr.IdComp = int.Parse(dr["IDCOMP"].ToString());
                    Compr.Titulo = dr["TITULO"].ToString();
                    Compr.Descricao = dr["DESCRICAO"].ToString();
                    Compr.DataInicio = Convert.ToDateTime(dr["DT_INICIO"].ToString()).ToShortDateString();
                    Compr.DataTermino = dr["DT_FIM"].ToString();
                    Compr.Status = (dr["STATUS"].ToString());
                    Compr.Usuario = DUser.RecuperaUsuario(dr["IDUSER"].ToString());
                    Compr.Empresa = DEmpresa.RecuperarEmpresaId(dr["IDEMPR"].ToString();
                    ListaComp.Add(Compr);
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
    }
}