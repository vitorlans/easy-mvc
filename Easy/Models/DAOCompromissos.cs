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
                    SqlCommand sqlTerminarCompromisso = new SqlCommand("Update Compromissos set status = T where idcomp = ?", Connection.Conectar());
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
                    SqlCommand sqlComando = new SqlCommand("Update Compromissos set status = C where idcomp = ?", Connection.Conectar());
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
                            IdComp = int.Parse(dr["IdComp"].ToString()),
                            Titulo = dr["Titulo"].ToString(),
                            Descricao = dr["Descricao"].ToString(),
                            DataInicio = DateTime.Parse(dr["Dt_Inicio"].ToString()),
                            DataTermino = DateTime.Parse(dr["Dt_Termino"].ToString()),
                            Status = (dr["Status"].ToString()),

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

        }
        public void EditarCompromisso(Compromissos Compromisso,Usuario Usuario)
        {
        }
    }
}