using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace Easy.Models
{
    public class DAOCompromissos
    {
        //Status será inserido como (T)erminado, (C)ancelado ou em (A)ndamento
        public void TerminarCompromisso(Compromissos Compromisso, Usuario Usuario)
        {
            //Somente o criador do compromisso poderá alterar o status do mesmo
            if (Compromisso.Usuario.IdUser == Usuario.IdUser)
            {
                try
                {
                    SqlCommand sqlTerminarCompromisso = new SqlCommand("UPDATE TBCOMPROMISSOS SET STATUS = T where idcomp = ?", Connection.Conectar());
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
        public void CancelarCompromisso(Compromissos Compromisso, Usuario Usuario)
        {
            //Somente o criador do compromisso poderá alterar o status do mesmo
            if (Compromisso.Usuario.IdUser == Usuario.IdUser)
            {
                try
                {
                    SqlCommand sqlComando = new SqlCommand("Update TBCompromissos set status = C where idcomp = ?", Connection.Conectar());
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

            try
            {
                SqlCommand sqlComando = new SqlCommand("Select * from TBCompromissos order by dt_inicio", Connection.Conectar());
                SqlDataReader dr = sqlComando.ExecuteReader();

                while(dr.Read())
                {
                    ListaComp.Add(
                        new Compromissos
                        {
                            IdComp = int.Parse(dr["IDCOMP"].ToString()),
                            Titulo = dr["TITULO"].ToString(),
                            Descricao = dr["DESCRICAO"].ToString(),
                            DataInicio = Convert.ToDateTime(dr["DT_INICIO"].ToString()).ToShortDateString(),
                            DataTermino = Convert.ToDateTime(dr["DT_FIM"].ToString()).ToShortDateString(),
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
        public void EditarCompromisso(Compromissos Compromisso,Usuario Usuario)
        {

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
                    Compromisso.DataInicio = Convert.ToDateTime(dr["DT_INICIO"].ToString()).ToShortDateString();
                    Compromisso.DataTermino = Convert.ToDateTime(dr["DT_FIM"].ToString()).ToShortDateString();
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