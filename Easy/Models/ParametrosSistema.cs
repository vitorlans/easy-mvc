using System;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Easy.Models
{
    public class ParametrosSistema
    {
        public int idParam       { get; set; }
        public string NomeParam  { get; set; }
        public bool ValorParam   { get; set; }


        public List<ParametrosSistema> ListaParametros()
        {
            List<ParametrosSistema> listParam = new List<ParametrosSistema>();

            SqlCommand sqlBusca = new SqlCommand("SELECT * FROM PARAMETROSSISTEMA", Connection.Conectar());
            SqlCommand sqlVerifica; 
            SqlDataReader dr = sqlBusca.ExecuteReader();

            while(dr.Read())
            {
                sqlVerifica = new SqlCommand("SELECT 1 FROM VPARAMUSUA WHRE IDPARAM = AND IDUSER = AND IDEMPR = ");
                listParam.Add(
                    new ParametrosSistema
                    {
                        idParam = int.Parse(dr["IDPARAM"].ToString()),
                        NomeParam = dr["NOMEPARAM"].ToString(),
                        ValorParam = true
                    }

                    );
            }

            return listParam;
        }
    }


}