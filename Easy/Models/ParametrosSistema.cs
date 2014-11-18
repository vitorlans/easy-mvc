using System;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Easy.Models
{
    public class ParametrosSistema
    {
        [Display(Name="Nro")]
        public int idParam       { get; set; }

        [Display(Name = "Parametro")]
        public string NomeParam  { get; set; }

        [Display(Name = "Ativo")]
        public bool ValorParam   { get; set; }


        public List<ParametrosSistema> ListaParametros()
        {
            DAOUsuario dUser = new DAOUsuario();
            Usuario user = Usuario.VerificaSeOUsuarioEstaLogado();
            Empresas emp = Empresas.RecuperaEmpresaCookie();
            bool valor = false;

            List<ParametrosSistema> listParam = new List<ParametrosSistema>();

            try
            {
                SqlCommand sqlBusca = new SqlCommand("SELECT * FROM PARAMETROSSISTEMA", Connection.Conectar());
                SqlCommand sqlVerifica;
                SqlDataReader dr = sqlBusca.ExecuteReader();

                while (dr.Read())
                {
                    sqlVerifica = new SqlCommand("SELECT 1 FROM VPARAMUSUA WHERE IDPARAM = " + dr["IDPARAM"] + " AND IDUSER = " + user.IdUser + " AND IDEMPR = " + emp.IdEmpresa + " AND STATUS = 1", Connection.Conectar());
                    SqlDataReader dr2 = sqlVerifica.ExecuteReader();

                    if (dr2.Read())
                    {
                        valor = true;
                    }

                    listParam.Add(
                        new ParametrosSistema
                        {
                            idParam = int.Parse(dr["IDPARAM"].ToString()),
                            NomeParam = dr["NOMEPARAM"].ToString(),
                            ValorParam = valor
                        }

                        );

                    dr2.Close();
                }
            }
            catch (SqlException sqEx)
            {
            }

            Connection.Desconectar();

            return listParam;
        }

        public void AtualizarParametro(int id, bool valor)
        {
            int val = 0;
            if (valor)
                val = 1;

            SqlCommand sqlUpdate = new SqlCommand("UPDATE VPARAMUSUA SET STATUS = "+val+" WHERE IDPARAM = "+id+"", Connection.Conectar());
            sqlUpdate.ExecuteNonQuery();

            Connection.Desconectar();
        }
    }


}