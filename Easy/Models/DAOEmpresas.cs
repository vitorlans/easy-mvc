using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace Easy.Models
{
    public class DAOEmpresas
    {
        public Empresas RecuperarEmpresaId(string IdEmpresa)
        {
            Empresas Empresa = new Empresas();
            try
            {
                SqlCommand sqlExec = new SqlCommand("SELECT * FROM TBEMPRESAS where IDEMPR=" + IdEmpresa, Connection.Conectar());
                SqlDataReader dr = sqlExec.ExecuteReader();

                while (dr.Read())
                {
                    Empresa =
                        (
                        new Empresas
                        {
                            IdEmpresa = Int32.Parse(dr["IDEMPR"].ToString()),
                            CnpjEmpresa = dr["EMPR_CNPJ"].ToString(),
                            NomeEmpresa = dr["NOME"].ToString(),
                            StatusEmpresa = dr["STATUS"].ToString()
                        }
                        );
                }
            }
            catch { }

            return Empresa;
        }
    }
}