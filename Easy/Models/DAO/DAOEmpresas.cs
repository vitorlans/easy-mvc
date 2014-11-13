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
            Connection.Desconectar();
            return Empresa;
        }

        public List<Empresas> ListarEmpresas(string email)
        {

            List<Empresas> lista = new List<Empresas>();

            try
            {

                SqlCommand sqlExec = new SqlCommand("SELECT TBEMP.*, TBUSU.EMAIL FROM VUSUAEMPR VEMP	INNER JOIN TBEMPRESAS TBEMP ON TBEMP.IDEMPR = VEMP.IDEMPR INNER JOIN TBUSUARIOS TBUSU ON TBUSU.IDUSER = VEMP.IDUSER WHERE TBEMP.STATUS = 'A' and TBUSU.EMAIL = @email", Connection.Conectar());
                    sqlExec.Parameters.AddWithValue("email", email);
                    SqlDataReader dr = sqlExec.ExecuteReader();

                    while (dr.Read())
                    {
                        lista.Add
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

            return lista;
        }

        public void VinculaEmpresa(Usuario user)
        {
           string strInserir = "insert into VUSUAEMPR values (@iduser, @empr)";


            SqlCommand inserirUsuario2 = new SqlCommand(strInserir, Connection.Conectar());


            inserirUsuario2.Parameters.AddWithValue("iduser", user.IdUser);
            inserirUsuario2.Parameters.AddWithValue("empr", Empresas.RecuperaEmpresaCookie().IdEmpresa);

            inserirUsuario2.ExecuteNonQuery();

        }

    }
}