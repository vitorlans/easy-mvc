using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace Easy.Models
{
    public class DAONotas
    {
        public void AdicionarNota(Notas Nota)
        {
            try
            {
                Usuario User = Usuario.VerificaSeOUsuarioEstaLogado();
                Empresas Emp = Empresas.RecuperaEmpresaCookie();
                Nota.Usuario = User;
                Nota.Empresa = Emp;

                string strInsert = "INSERT INTO TBNOTAS VALUES(@DESCRICAO, @IDUSER, @IDEMPR)";
                SqlCommand sqlInsere = new SqlCommand(strInsert, Connection.Conectar());

                sqlInsere.Parameters.AddWithValue("DESCRICAO", Nota.DescricaoNota);
                sqlInsere.Parameters.AddWithValue("IDUSER", Nota.Usuario.IdUser);
                sqlInsere.Parameters.AddWithValue("IDEMPR", Nota.Empresa.IdEmpresa);

                sqlInsere.ExecuteNonQuery();
            }
            catch { }
            Connection.Desconectar();
        }
        public void VincNotaComp(Notas Nota, Compromissos Comp)
        {
            try
            {
                string strInsert = "INSERT INTO VCOMPNOTA VALUES(@IDCOMP, @IDNOTA)";
                SqlCommand sqlInsere = new SqlCommand(strInsert, Connection.Conectar());

                sqlInsere.Parameters.AddWithValue("IDCOMP", Comp.IdComp);
                sqlInsere.Parameters.AddWithValue("IDNOTA", Nota.IdNota);

                sqlInsere.ExecuteNonQuery();
            }
            catch { }
            Connection.Desconectar();
        }
        
        public List<Notas> ListarNotas(int idComp)
        {
            List<Notas> listNotas = new List<Notas>();
            Usuario User = Usuario.VerificaSeOUsuarioEstaLogado();
            Empresas Emp = Empresas.RecuperaEmpresaCookie();
            DAOUsuario DUser = new DAOUsuario();
            DAOEmpresas DEmp = new DAOEmpresas();
            string strSelect = "SELECT NT.* FROM TBNOTAS NT, VCOMPNOTA CN WHERE IDUSER = " + User.IdUser +" AND IDEMPR = " + Emp.IdEmpresa + " AND CN.IDCOMP = " + idComp;
            SqlCommand sqlComando = new SqlCommand(strSelect, Connection.Conectar());
            SqlDataReader dr = sqlComando.ExecuteReader();

            while (dr.Read())
            {
                listNotas.Add(
                    new Notas
                    {
                        IdNota = int.Parse(dr["IDNOTA"].ToString()),
                        DescricaoNota = dr["DESCRICAO"].ToString(),
                        Usuario = DUser.RecuperaUsuario(dr["IDUSER"].ToString()),
                        Empresa = DEmp.RecuperarEmpresaId(dr["IDEMPR"].ToString())
                    }
                    );
            }
            return listNotas;
        }
        public int RecuperaIdUltimaNota(int IdUser)
        {
            int IdNota = 0;
            try
            {
                SqlCommand sqlComando = new SqlCommand("SELECT IDNOTA FROM TBNOTAS WHERE IDUSER = " + IdUser + "ORDER BY IDNOTA DESC", Connection.Conectar());
                SqlDataReader dr = sqlComando.ExecuteReader();

                if (dr.Read())
                {
                    IdNota = Int32.Parse(dr["IDNOTA"].ToString());
                }
            }
            catch (SqlException e) { }
            Connection.Desconectar();
            return IdNota;
        }
    }
}