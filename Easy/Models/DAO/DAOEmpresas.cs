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

        public List<Empresas> ListarEmpresas(string login)
        {

            var UserEmp = UserEmpresa(login);

            List<Empresas> lista = new List<Empresas>();

            try
            {
                var x = 0;
                foreach (var i in UserEmp)
                {

                    SqlCommand sqlExec = new SqlCommand("SELECT * FROM TBEMPRESAS where IdEmpr=" + UserEmp[x].IdEmp.ToString(), Connection.Conectar());
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
                    x++;
                }
            }
            catch { }

            return lista;
        }

        private List<VinculoEmpresa> UserEmpresa(string login)
        {
            List<VinculoEmpresa> lista = new List<VinculoEmpresa>();
            SqlCommand sqlExec = new SqlCommand("SELECT * FROM VUSUAEMPR where IDUSER=" + login, Connection.Conectar());
            SqlDataReader dr = sqlExec.ExecuteReader();
            while (dr.Read())
            {
                lista.Add
                        (
                        new VinculoEmpresa
                        {
                            IdUser = int.Parse(dr["IDUSER"].ToString()),
                            IdEmp = int.Parse(dr["IDEMPR"].ToString()),
                        });
            }

            return lista;
        }

        public void VinculaEmpresa(Usuario user)
        {
            SqlCommand sqlExec = new SqlCommand("SELECT * FROM TBUSUARIOS where EMAIL=" + "'" + user.Email + "'", Connection.Conectar());
            SqlDataReader dr = sqlExec.ExecuteReader();
            while (dr.Read())
            {
                user.IdUser = int.Parse(dr["IDUSER"].ToString());
                      

            }
    
            string strInserir = "insert into VUSUAEMPR values (@iduser, @empr)";


            SqlCommand inserirUsuario2 = new SqlCommand(strInserir, Connection.Conectar());


            inserirUsuario2.Parameters.AddWithValue("iduser", user.IdUser);
            inserirUsuario2.Parameters.AddWithValue("empr", 1);

            inserirUsuario2.ExecuteNonQuery();

        }

    }
}