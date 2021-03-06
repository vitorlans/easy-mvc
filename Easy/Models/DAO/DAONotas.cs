﻿using System;
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
            DAOCompromissos dComp = new DAOCompromissos();
            Compromissos Comp = new Compromissos();
            Comp = dComp.SelecionarCompromissoId(idComp);
            string strSelect;

            if(Comp.Usuario.IdUser == User.IdUser)
            {
                strSelect = "SELECT NT.* FROM TBNOTAS NT, VCOMPNOTA CN WHERE NT.IDEMPR = "+ Emp.IdEmpresa +" AND CN.IDCOMP = " + idComp + " AND CN.IDNOTA = NT.IDNOTA";
            }
            else
            {
                 strSelect = "SELECT NT.* FROM TBNOTAS NT, VCOMPNOTA CN WHERE NT.IDUSER = "+ User.IdUser +" AND NT.IDEMPR = "+ Emp.IdEmpresa +" AND CN.IDCOMP = " + idComp + " AND CN.IDNOTA = NT.IDNOTA";

            }
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
        public Notas RecuperaNotaID(int idNota)
        {
            Notas Nota = new Notas();
            Nota.IdNota = idNota;
            try
            {
                SqlCommand sqlComando = new SqlCommand("SELECT * FROM TBNOTAS WHERE IDNOTA = @IDNOTA", Connection.Conectar());
                sqlComando.Parameters.AddWithValue("IDNOTA", Nota.IdNota);
                SqlDataReader dr = sqlComando.ExecuteReader();

                if (dr.Read())
                {
                    Nota.DescricaoNota = dr["DESCRICAO"].ToString();
                }
            }
            catch (SqlException e) { }
            Connection.Desconectar();
            return Nota;
        }
        public void DeletarNota(string id)
        {
            try
            {
                string strInsert = "DELETE FROM VCOMPNOTA WHERE IDNOTA = @IDNOTA";
                SqlCommand sqlInsere = new SqlCommand(strInsert, Connection.Conectar());

                sqlInsere.Parameters.AddWithValue("IDNOTA", id);

                sqlInsere.ExecuteNonQuery();
            }
            catch { }
            Connection.Desconectar();
        }
        public void AtualizarNota(string id, string descricao)
        {
            try
            {
                Notas Nota = new Notas();
                Nota.IdNota = int.Parse(id);
                Nota.DescricaoNota = descricao;
                Usuario User = Usuario.VerificaSeOUsuarioEstaLogado();
                Empresas Emp = Empresas.RecuperaEmpresaCookie();
                Nota.Usuario = User;
                Nota.Empresa = Emp;

                string strInsert = "UPDATE TBNOTAS SET DESCRICAO = @DESCRICAO WHERE IDNOTA = @IDNOTA";
                SqlCommand sqlInsere = new SqlCommand(strInsert, Connection.Conectar());

                sqlInsere.Parameters.AddWithValue("DESCRICAO", Nota.DescricaoNota);
                sqlInsere.Parameters.AddWithValue("IDNOTA", Nota.IdNota);

                sqlInsere.ExecuteNonQuery();
            }
            catch { }
            Connection.Desconectar();
        }
    }
}