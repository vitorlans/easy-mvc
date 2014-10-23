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
            if (Compromisso.Usuario.IdUser != Usuario.IdUser)
            {
            }
            else
            {
                try
                {
                    SqlCommand sqlTerminarCompromisso = new SqlCommand("Update Compromissos set status = ? where idcomp = ?", Connection.Conectar());
                }
                catch (SqlException sqlExcp)
                {
                }
                catch(Exception Erro)
                {
                }

            }
        }
        public void CancelarCompromisso(Compromissos Compromisso, Usuario Usuario)
        {
            //Somente o criador do compromisso poderá alterar o status do mesmo
            if (Compromisso.Usuario.IdUser != Usuario.IdUser)
            {
            }
            else
            {
                try
                {
                    SqlCommand sqlTerminarCompromisso = new SqlCommand("Update Compromissos set status = ? where idcomp = ?", Connection.Conectar());
                }
                catch (SqlException sqlExcp)
                {
                }
                catch (Exception Erro)
                {
                }

            }
        }
    }
}