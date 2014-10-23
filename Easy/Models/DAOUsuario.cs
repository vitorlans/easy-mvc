using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace Easy.Models
{
    public class DAOUsuario
    {
 
        public List<Usuario> ListaUsuarios()
        {
            List<Usuario> lista = new List<Usuario>();

            try
            {
                SqlCommand sqlExec = new SqlCommand("SELECT * FROM TBUSUARIOS", Connection.Conectar());
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

            }
            catch { }

            return lista;
        }

        public Usuario RecuperaUsuario(string id) {

            Usuario user = new Usuario();
            try
            {
            SqlCommand sqlExec = new SqlCommand("SELECT * FROM TBUSUARIOS where IDUSER="+id, Connection.Conectar());
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

        public void CriarUsuario(Usuario user, string login) {

            user.DataCriacao = DateTime.Now.ToString();
            user.Status = "A";
            user.Senha = Usuario.GerarSenha();

        string strInserir = "insert into TBUSUARIOS values (@nome, null, @email, @senha, @endereco, @bairro, @cidade, @cep, @telefone, @usuariosistema, @liberaconvite, @status, @datacriacao, null)";


        try
        {
            SqlCommand inserirUsuario = new SqlCommand(strInserir, Connection.Conectar());



            inserirUsuario.Parameters.Add(new SqlParameter("nome", user.Nome));
            //inserirUsuario.Parameters.Add(new SqlParameter("sobrenome", user.Sobrenome));
            inserirUsuario.Parameters.Add(new SqlParameter("email", user.Email));
            inserirUsuario.Parameters.Add(new SqlParameter("senha", user.Senha));
            inserirUsuario.Parameters.Add(new SqlParameter("endereco", user.Endereco));
            inserirUsuario.Parameters.Add(new SqlParameter("bairro", user.Bairro));
            inserirUsuario.Parameters.Add(new SqlParameter("cidade", user.Cidade));
            inserirUsuario.Parameters.Add(new SqlParameter("cep", user.Cep));
            inserirUsuario.Parameters.Add(new SqlParameter("telefone", user.Telefone));
            inserirUsuario.Parameters.Add(new SqlParameter("usuariosistema", user.UsuarioSistema));
            inserirUsuario.Parameters.Add(new SqlParameter("liberaconvite", user.LiberaConvite));
            inserirUsuario.Parameters.Add(new SqlParameter("status", user.Status));
            inserirUsuario.Parameters.Add(new SqlParameter("datacriacao", user.DataCriacao));
            //inserirUsuario.Parameters.Add(new SqlParameter("imagem", user.Imagem));


            inserirUsuario.ExecuteNonQuery();


            SqlCommand sqlExec = new SqlCommand("SELECT * FROM TBUSUARIOS where EMAIL=" + user.Email, Connection.Conectar());
            SqlDataReader dr = sqlExec.ExecuteReader();
            Usuario UserID = new Usuario();
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

            string strInserir2 = "insert into TBUSUAUSUA values (@iduser, @iduser2)";


        
            SqlCommand inserirUsuario2 = new SqlCommand(strInserir2, Connection.Conectar());



            inserirUsuario2.Parameters.Add(new SqlParameter("iduser", login));
            inserirUsuario2.Parameters.Add(new SqlParameter("iduser2", user.IdUser));

            inserirUsuario.ExecuteNonQuery();

            //fim do try
        }

        catch (SqlException)
        {
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


    }
