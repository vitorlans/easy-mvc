﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace Easy.Models
{
    public class DAOUsuario
    {

        public List<Usuario> ListaUsuarios(string login)
        {

            var meusContatos = MeusContatos(login);
            
            List<Usuario> lista = new List<Usuario>();

            try
            {   var x = 0;
                foreach (var i in meusContatos)
                {
                    
                    SqlCommand sqlExec = new SqlCommand("SELECT * FROM TBUSUARIOS where IdUser=" + meusContatos[x].IdUser2.ToString(), Connection.Conectar());
                    SqlDataReader dr = sqlExec.ExecuteReader();

                    while (dr.Read())
                    {
                        lista.Add
                            (
                            new Usuario
                            {
                                IdUser = int.Parse(dr["IDUSER"].ToString()),
                                Nome = dr["NOME"].ToString(),
                                Sobrenome = dr["SOBRENOME"].ToString(),
                                Senha = dr["SENHA"].ToString(),
                                Endereco = dr["ENDERECO"].ToString(),
                                Bairro = dr["BAIRRO"].ToString(),
                                Cidade = dr["CIDADE"].ToString(),
                                Cep = dr["CEP"].ToString(),
                                Telefone = dr["TELEFONE"].ToString(),
                                UsuarioSistema = dr["USUARIOSISTEMA"].ToString(),
                                LiberaConvite = dr["LIBERACONVITE"].ToString(),
                                Status = dr["STATUS"].ToString(),
                                DataCriacao = dr["DT_CRIACAO"].ToString(),
                                Imagem = dr["IMAGEM"].ToString(),

                            }
                            );
                    }
                    x++;
                }
            }
            catch { }

            return lista;
        }

        public Usuario RecuperaUsuario(string id)
        {

            Usuario user = new Usuario();
            try
            {
                SqlCommand sqlExec = new SqlCommand("SELECT * FROM TBUSUARIOS where IDUSER=" + id, Connection.Conectar());
                SqlDataReader dr = sqlExec.ExecuteReader();

                while (dr.Read())
                {
                    user =
                          (
                          new Usuario
                          {
                              IdUser = int.Parse(dr["IDUSER"].ToString()),
                              Nome = dr["NOME"].ToString(),
                              Sobrenome = dr["SOBRENOME"].ToString(),
                              Email = dr["EMAIL"].ToString(),
                              Senha = dr["SENHA"].ToString(),
                              Endereco = dr["ENDERECO"].ToString(),
                              Bairro = dr["BAIRRO"].ToString(),
                              Cidade = dr["CIDADE"].ToString(),
                              Cep = dr["CEP"].ToString(),
                              Telefone = dr["TELEFONE"].ToString(),
                              UsuarioSistema = dr["USUARIOSISTEMA"].ToString(),
                              LiberaConvite = dr["LIBERACONVITE"].ToString(),
                              Status = dr["STATUS"].ToString(),
                              DataCriacao = dr["DT_CRIACAO"].ToString(),
                              Imagem = dr["IMAGEM"].ToString(),

                          }
                          );
                }


            }
            catch { }
            return user;

        }

        public void CriarUsuario(Usuario user, string login)
        {

            user.DataCriacao = DateTime.Now.ToString();
            user.Status = "A";
            user.Senha = Usuario.GerarSenha();

            try
            {
                if (VerificaExistencia(user) == false)
                {


                    string strInserir = "insert into TBUSUARIOS values (@nome, @sobrenome, @email, @senha, @endereco, @bairro, @cidade, @cep, @telefone, @usuariosistema, @liberaconvite, @status, @datacriacao, @imagem)";

                    SqlCommand inserirUsuario = new SqlCommand(strInserir, Connection.Conectar());



                    inserirUsuario.Parameters.AddWithValue("nome", user.Nome);
                    inserirUsuario.Parameters.AddWithValue("sobrenome", (object)user.Sobrenome ?? DBNull.Value);
                    inserirUsuario.Parameters.AddWithValue("email", user.Email);
                    inserirUsuario.Parameters.AddWithValue("senha", user.Senha);
                    inserirUsuario.Parameters.AddWithValue("endereco", (object)user.Endereco ?? DBNull.Value);
                    inserirUsuario.Parameters.AddWithValue("bairro", (object)user.Bairro ?? DBNull.Value);
                    inserirUsuario.Parameters.AddWithValue("cidade", (object)user.Cidade ?? DBNull.Value);
                    inserirUsuario.Parameters.AddWithValue("cep", (object)user.Cep ?? DBNull.Value);
                    inserirUsuario.Parameters.AddWithValue("telefone", (object)user.Telefone ?? DBNull.Value);
                    inserirUsuario.Parameters.AddWithValue("usuariosistema", user.UsuarioSistema);
                    inserirUsuario.Parameters.AddWithValue("liberaconvite", user.LiberaConvite);
                    inserirUsuario.Parameters.AddWithValue("status", user.Status);
                    inserirUsuario.Parameters.AddWithValue("datacriacao", user.DataCriacao);
                    inserirUsuario.Parameters.AddWithValue("imagem", (object)user.Imagem ?? DBNull.Value);



                    inserirUsuario.ExecuteNonQuery();
                    VinculaUsuario(user, login);

                }
                else
                {
                    VinculaUsuario(user, login);
                }
            }
            catch (SqlException) { }

        }



        private void VinculaUsuario(Usuario user, string login)
        {

            SqlCommand sqlExec = new SqlCommand("SELECT * FROM TBUSUARIOS where EMAIL=" + "'" + user.Email + "'", Connection.Conectar());
            SqlDataReader dr = sqlExec.ExecuteReader();
            while (dr.Read())
            {
                user =
                      (
                      new Usuario
                      {
                          IdUser = int.Parse(dr["IDUSER"].ToString())
                      }
                      );

            }

            string strInserir2 = "insert into VUSUACONT values (@iduser, @iduser2)";



            SqlCommand inserirUsuario2 = new SqlCommand(strInserir2, Connection.Conectar());


            inserirUsuario2.Parameters.AddWithValue("iduser", login);
            inserirUsuario2.Parameters.AddWithValue("iduser2", user.IdUser);

            inserirUsuario2.ExecuteNonQuery();

        }



        private bool VerificaExistencia(Usuario user)
        {

            SqlCommand sqlExec = new SqlCommand("SELECT * FROM TBUSUARIOS where EMAIL=" + "'" + user.Email + "'", Connection.Conectar());
            SqlDataReader dr = sqlExec.ExecuteReader();
            if (dr.Read())
                return true;

            else
                return false;

        }


        private List<VinculoUsuario> MeusContatos(string login)
        {
             List<VinculoUsuario> lista = new List<VinculoUsuario>();
            SqlCommand sqlExec = new SqlCommand("SELECT * FROM VUSUACONT where IDUSER=" + login, Connection.Conectar());
            SqlDataReader dr = sqlExec.ExecuteReader();
            while (dr.Read()) 
            {
                lista.Add
                        (
                        new VinculoUsuario
                        {
                            IdUser = int.Parse(dr["IDUSER"].ToString()),
                            IdUser2 = int.Parse(dr["IDUSER2"].ToString()),
                        });
            }

            return lista;
        }
    }
}
       
      

        //public void Update(Cidade cid)
        //{

        //    string strUpdate = "UPDATE Cidades SET NOME_CIDADE= ?, IMAGEM=?, DESCRICAO=?, STATUS=?, GUIA=? WHERE COD_CIDADE=" + cid.CodigoCidade;

        //    try
        //    {
        //        OleDbCommand updateCidade = new OleDbCommand(strUpdate, Conexao.getConexao());

        //        OleDbParameter paramNome = new OleDbParameter("NOME_CIDADE", OleDbType.VarChar);
        //        OleDbParameter paramImagem = new OleDbParameter("IMAGEM", OleDbType.VarChar);
        //        OleDbParameter paramDescri = new OleDbParameter("DESCRICAO", OleDbType.VarChar);
        //        OleDbParameter paramStatus = new OleDbParameter("STATUS", OleDbType.Integer);
        //        OleDbParameter paramGuia = new OleDbParameter("GUIA", OleDbType.Integer);


        //        paramNome.Value = cid.NomeCidade;
        //        paramImagem.Value = cid.Imagem;
        //        paramDescri.Value = cid.Descricao;
        //        paramStatus.Value = cid.Status;
        //        paramGuia.Value = cid.Guia;


        //        updateCidade.Parameters.Add(paramNome);
        //        updateCidade.Parameters.Add(paramImagem);
        //        updateCidade.Parameters.Add(paramDescri);
        //        updateCidade.Parameters.Add(paramStatus);
        //        updateCidade.Parameters.Add(paramGuia);

        //        updateCidade.ExecuteNonQuery();

        //    }
        //    catch (OleDbException)
        //    {
        //    }
        //}


    
