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
                SqlCommand sqlComando = new SqlCommand("Select * from TBCompromissos order by data", Connection.Conectar());
                SqlDataReader dr = sqlComando.ExecuteReader();

                while(dr.Read())
                {
                    ListaComp.Add(
                        new Compromissos
                        {
                            IdComp = int.Parse(dr["IDCOMP"].ToString()),
                            Titulo = dr["TITULO"].ToString(),
                            Descricao = dr["DESCRICAO"].ToString(),
                            DataInicio = DateTime.Parse(dr["DT_INICIO"].ToString()),
                            DataTermino = DateTime.Parse(dr["DT_TERMINO"].ToString()),
                            Status = (dr["STATUS"].ToString()),
                            Usuario = DUser.RecuperaUsuario(dr["IDUSER"].ToString()),
                            Empresa = DEmpresa.RecuperarEmpresa(dr["IDEMPR"].ToString())
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
            return ListaComp;
        }
        public void AddCompromisso(Compromissos Compromisso)
        {
            try
            {
                SqlCommand sqlExec = new SqlCommand("INSERT INTO TBCOMPROMISSOS VALUES (@IDCOMP, @TITULO, @DESCRICAO, @DT_INICIO, @DT_FIM, @STATUS, @ID_USER, @IDEMPR)", Connection.Conectar());

                sqlExec.Parameters.AddWithValue("IDCOMP", Compromisso.IdComp);
                sqlExec.Parameters.AddWithValue("TITULO", Compromisso.Titulo);
                sqlExec.Parameters.AddWithValue("DESCRICAO", Compromisso.Descricao);
                sqlExec.Parameters.AddWithValue("DT_INICIO", Compromisso.DataInicio);
                sqlExec.Parameters.AddWithValue("DT_TERMINO", Compromisso.DataTermino);
                sqlExec.Parameters.AddWithValue("DT_FIM", Compromisso.DataTermino);
                sqlExec.Parameters.AddWithValue("STATUS", Compromisso.Status);
                sqlExec.Parameters.AddWithValue("IDUSER", Compromisso.Status);
                sqlExec.Parameters.AddWithValue("IDEMPR", Compromisso.Empresa);

                sqlExec.ExecuteNonQuery();
            }
            catch (SqlException sqlEx)
            {
            }
        }
        public void EditarCompromisso(Compromissos Compromisso,Usuario Usuario)
        {

        }
    }
}