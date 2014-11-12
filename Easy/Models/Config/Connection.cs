using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace Easy.Models
{
    public abstract class Connection
    {
        private static string strConn = "Data Source=localhost;Initial Catalog=projetopi;User ID=sa;Password=12345; Pooling=false";
        private static SqlConnection conn;

        public Connection()
        {

        }

        public static SqlConnection Conectar()
        {
            try
            {
                conn = new SqlConnection(strConn);

                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
            }
            catch (SqlException sqE)
            {

            }

            return conn;
        }

        public static void Desconectar()
        {
            try
            {
                conn.Close();
            }
            catch (Exception e)
            {

            }
        }
    }
}