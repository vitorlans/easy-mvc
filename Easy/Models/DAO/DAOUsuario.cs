using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Easy.Controllers;

namespace Easy.Models
{
    public class DAOUsuario
    {

        public List<Usuario> ListaUsuarios(string login)
        {

            var emp = Empresas.RecuperaEmpresaCookie();
            List<Usuario> lista = new List<Usuario>();

            try
            {
                SqlCommand sqlExec = new SqlCommand("SELECT VCONT.*, USUA.EMAIL, USUA.IMAGEM, USUA.USUARIOSISTEMA, USUA.LIBERACONVITE FROM VUSUACONT vcont INNER JOIN TBUSUARIOS USUA ON USUA.IDUSER = VCONT.IDUSER2 WHERE  VCONT.IDUSER = @iduser and VCONT.IDEMPR = @idemp", Connection.Conectar());
                    sqlExec.Parameters.AddWithValue("iduser", login);
                    sqlExec.Parameters.AddWithValue("idemp", emp.IdEmpresa);
                     SqlDataReader dr = sqlExec.ExecuteReader();

                    while (dr.Read())
                    {
                        lista.Add
                            (
                            new Usuario
                            {
                                IdUser = int.Parse(dr["IDUSER2"].ToString()),
                                Nome = dr["NOME"].ToString(),
                                Email = dr["EMAIL"].ToString(),
                                Endereco = dr["ENDERECO"].ToString(),
                                Bairro = dr["BAIRRO"].ToString(),
                                Cidade = dr["CIDADE"].ToString(),
                                Cep = dr["CEP"].ToString(),
                                Telefone = dr["TELEFONE"].ToString(),
                                UsuarioSistema = dr["USUARIOSISTEMA"].ToString(),
                                LiberaConvite = dr["LIBERACONVITE"].ToString(),
                                Imagem = dr["IMAGEM"].ToString()
                            }
                            );
                    }
                }
            
            catch { }
            Connection.Desconectar();

            return lista;
        }

        public List<Usuario> ListaUsuarios(string login, string letra)
        {

            var emp = Empresas.RecuperaEmpresaCookie();
            List<Usuario> lista = new List<Usuario>();

            try
            {

                SqlCommand sqlExec = new SqlCommand("SELECT VCONT.*, USUA.EMAIL, USUA.IMAGEM, USUA.USUARIOSISTEMA, USUA.LIBERACONVITE FROM VUSUACONT vcont INNER JOIN TBUSUARIOS USUA ON USUA.IDUSER = VCONT.IDUSER2 WHERE  VCONT.IDUSER = @iduser and VCONT.IDEMPR = @idemp" + " and VCONT.NOME like '" + letra + "%'", Connection.Conectar());
                    sqlExec.Parameters.AddWithValue("iduser", login);
                    sqlExec.Parameters.AddWithValue("idemp", emp.IdEmpresa);
                    SqlDataReader dr = sqlExec.ExecuteReader();

                    while (dr.Read())
                    {
                        lista.Add
                            (
                            new Usuario
                            {
                                IdUser = int.Parse(dr["IDUSER2"].ToString()),
                                Nome = dr["NOME"].ToString(),
                                Email = dr["EMAIL"].ToString(),
                                Endereco = dr["ENDERECO"].ToString(),
                                Bairro = dr["BAIRRO"].ToString(),
                                Cidade = dr["CIDADE"].ToString(),
                                Cep = dr["CEP"].ToString(),
                                Telefone = dr["TELEFONE"].ToString(),
                                UsuarioSistema = dr["USUARIOSISTEMA"].ToString(),
                                LiberaConvite = dr["LIBERACONVITE"].ToString(),
                                Imagem = dr["IMAGEM"].ToString()
                            }
                            );
                    }
            }
            catch { }
            Connection.Desconectar();

            return lista;
        }

        public List<Usuario> ListaUsuariosPesquisa(string login, string pesquisa)
        {

            var emp = Empresas.RecuperaEmpresaCookie();
            List<Usuario> lista = new List<Usuario>();

            try
            {
                SqlCommand sqlExec = new SqlCommand("SELECT VCONT.*, USUA.EMAIL, USUA.IMAGEM, USUA.USUARIOSISTEMA, USUA.LIBERACONVITE FROM VUSUACONT vcont INNER JOIN TBUSUARIOS USUA ON USUA.IDUSER = VCONT.IDUSER2 WHERE  VCONT.IDUSER = @iduser and VCONT.IDEMPR = @idemp" + " and VCONT.NOME like '%" + pesquisa + "%'", Connection.Conectar());
                    sqlExec.Parameters.AddWithValue("iduser", login);
                    sqlExec.Parameters.AddWithValue("idemp", emp.IdEmpresa);
                    SqlDataReader dr = sqlExec.ExecuteReader();

                    while (dr.Read())
                    {
                        lista.Add
                            (
                            new Usuario
                            {
                                IdUser = int.Parse(dr["IDUSER2"].ToString()),
                                Nome = dr["NOME"].ToString(),
                                Email = dr["EMAIL"].ToString(),
                                Endereco = dr["ENDERECO"].ToString(),
                                Bairro = dr["BAIRRO"].ToString(),
                                Cidade = dr["CIDADE"].ToString(),
                                Cep = dr["CEP"].ToString(),
                                Telefone = dr["TELEFONE"].ToString(),
                                UsuarioSistema = dr["USUARIOSISTEMA"].ToString(),
                                LiberaConvite = dr["LIBERACONVITE"].ToString(),
                                Imagem = dr["IMAGEM"].ToString()   
                            }
                            );
                    }
                
            }
            catch { }
            Connection.Desconectar();

            return lista;
        }


        public Usuario RecuperaContato(string id)
        {

            Usuario user = new Usuario();
            try
            {
                var emp = Empresas.RecuperaEmpresaCookie();
                SqlCommand sqlExec = new SqlCommand("SELECT VCONT.*, USUA.EMAIL, USUA.IMAGEM, USUA.USUARIOSISTEMA, USUA.LIBERACONVITE FROM VUSUACONT vcont INNER JOIN TBUSUARIOS USUA ON USUA.IDUSER = VCONT.IDUSER2 WHERE  VCONT.IDUSER2 = @iduser2 and VCONT.IDEMPR = @idempr", Connection.Conectar());
                sqlExec.Parameters.AddWithValue("iduser2", id);
                sqlExec.Parameters.AddWithValue("idempr", emp.IdEmpresa);
                SqlDataReader dr = sqlExec.ExecuteReader();

                while (dr.Read())
                {
                    user =
                          (
                          new Usuario
                          {
                              IdUser = int.Parse(dr["IDUSER2"].ToString()),
                              Nome = dr["NOME"].ToString(),
                              Email = dr["EMAIL"].ToString(),
                              Endereco = dr["ENDERECO"].ToString(),
                              Bairro = dr["BAIRRO"].ToString(),
                              Cidade = dr["CIDADE"].ToString(),
                              Cep = dr["CEP"].ToString(),
                              Telefone = dr["TELEFONE"].ToString(),
                              UsuarioSistema = dr["USUARIOSISTEMA"].ToString(),
                              LiberaConvite = dr["LIBERACONVITE"].ToString(),
                              Imagem = dr["IMAGEM"].ToString()
                          }
                          );
                }


            }
            catch { }
            Connection.Desconectar();

            return user;

        }

        public Usuario RecuperaUsuarioEmail(string email)
        {

            Usuario user = new Usuario();
            try
            {
                SqlCommand sqlExec = new SqlCommand("SELECT * FROM TBUSUARIOS where EMAIL="+"'"+email+"'", Connection.Conectar());
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
            Connection.Desconectar();

            return user;

        }

        public Usuario RecuperaUsuario(string id)
        {

            Usuario user = new Usuario();
            try
            {
                SqlCommand sqlExec = new SqlCommand("SELECT * FROM TBUSUARIOS where IDUSER=" + id , Connection.Conectar());
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
            Connection.Desconectar();

            return user;

        }

        public bool CriarUsuario(Usuario user, string login)
        {

         try
            {
                if (VerificaExistencia(user) == false)
                {

                    user.DataCriacao = DateTime.Now.ToString();
                    user.Status = "A";
                    user.Senha = Criptografia.GerarSenha();
                    var random = new Random();
                    var num = random.Next(6);
                    user.Imagem = "http://localhost:58623/Content/img/contato" + num + ".png";

                    string strInserir = "insert into TBUSUARIOS (NOME, EMAIL, SENHA, USUARIOSISTEMA, LIBERACONVITE, STATUS, DT_CRIACAO, IMAGEM) values (@nome, @email, @senha, @usuariosistema, @liberaconvite, @status, @datacriacao, @imagem)";

                    SqlCommand inserirUsuario = new SqlCommand(strInserir, Connection.Conectar());

                    inserirUsuario.Parameters.AddWithValue("nome", user.Nome);
                    inserirUsuario.Parameters.AddWithValue("email", user.Email);
                    inserirUsuario.Parameters.AddWithValue("senha", user.Senha);
                    inserirUsuario.Parameters.AddWithValue("usuariosistema", user.UsuarioSistema);
                    inserirUsuario.Parameters.AddWithValue("liberaconvite", user.LiberaConvite);
                    inserirUsuario.Parameters.AddWithValue("status", user.Status);
                    inserirUsuario.Parameters.AddWithValue("datacriacao", user.DataCriacao);
                    inserirUsuario.Parameters.AddWithValue("imagem", (object)user.Imagem ?? DBNull.Value);

                    inserirUsuario.ExecuteNonQuery();
                    var rec = RecuperaUsuarioEmail(user.Email);
                    user.IdUser = rec.IdUser;
                    DAOEmpresas Demp = new DAOEmpresas();
                    Demp.VinculaEmpresa(user);
                    VinculaUsuario(login, user);

                    if (user.UsuarioSistema == "S")
                    {
                        EmailController ec = new EmailController();
                        ec.EnviarEmailSistema(user);
                    }

                    return true;
                }
                else
                {
                    var rec = RecuperaUsuarioEmail(user.Email);
                    user.IdUser = rec.IdUser;
                    VinculaUsuario(login, user);
                    return true;
                }

            }
            catch (SqlException) {

                return false;
            }

        }

        public bool AlterarSenha(string id, string senha)
        {

            try
            {

                string strInserir = "update TBUSUARIOS  set SENHA = @senha where IDUSER = @iduser";

                SqlCommand updateUsuario = new SqlCommand(strInserir, Connection.Conectar());
                updateUsuario.Parameters.AddWithValue("senha", senha);
                updateUsuario.Parameters.AddWithValue("iduser", id);

                
                updateUsuario.ExecuteNonQuery();
                return true;
            }

            catch (SqlException) { return false; }
        }


        public void AtualizarUsuario(Usuario user)
        {

            try
            {

                string strInserir = "update TBUSUARIOS  set NOME = @nome, SOBRENOME = @sobrenome, ENDERECO = @endereco, BAIRRO = @bairro, CIDADE = @cidade, CEP = @cep, TELEFONE = @telefone where IDUSER = @iduser";

                SqlCommand updateUsuario = new SqlCommand(strInserir, Connection.Conectar());



                updateUsuario.Parameters.AddWithValue("nome", user.Nome);
                updateUsuario.Parameters.AddWithValue("sobrenome", (object)user.Sobrenome ?? DBNull.Value);
                updateUsuario.Parameters.AddWithValue("endereco", (object)user.Endereco ?? DBNull.Value);
                updateUsuario.Parameters.AddWithValue("bairro", (object)user.Bairro ?? DBNull.Value);
                updateUsuario.Parameters.AddWithValue("cidade", (object)user.Cidade ?? DBNull.Value);
                updateUsuario.Parameters.AddWithValue("cep", (object)user.Cep ?? DBNull.Value);
                updateUsuario.Parameters.AddWithValue("telefone", (object)user.Telefone ?? DBNull.Value);
                updateUsuario.Parameters.AddWithValue("iduser", (object)user.IdUser ?? DBNull.Value);


                updateUsuario.ExecuteNonQuery();
            }

            catch (SqlException) { }
        }

        public void AtualizaPermissaoSistema(Usuario user)
        {

            try
            {

                string strInserir = "update TBUSUARIOS  set USUARIOSISTEMA = @usuariosistema where IDUSER = @iduser";

                SqlCommand updateUsuario = new SqlCommand(strInserir, Connection.Conectar());

                updateUsuario.Parameters.AddWithValue("usuariosistema", user.UsuarioSistema);

                updateUsuario.Parameters.AddWithValue("iduser", user.IdUser);

                updateUsuario.ExecuteNonQuery();

                if (user.UsuarioSistema == "S")
                {
                    user = RecuperaUsuario(user.IdUser.ToString());
                    EmailController ec = new EmailController();
                    ec.EnviarEmailSistema(user);
                }
                else {

                    user = RecuperaUsuario(user.IdUser.ToString());
                    EmailController ec = new EmailController();
                    ec.EnviarEmailDesativa(user); 
                }
            }

            catch (SqlException) { }
        }

        public void AtualizaPermissaoConvite(Usuario user)
        {

            try
            {

                string strInserir = "update TBUSUARIOS  set LIBERACONVITE = @liberaconvite where IDUSER = @iduser";

                SqlCommand updateUsuario = new SqlCommand(strInserir, Connection.Conectar());

                updateUsuario.Parameters.AddWithValue("liberaconvite", user.LiberaConvite);

                updateUsuario.Parameters.AddWithValue("iduser", user.IdUser);

                updateUsuario.ExecuteNonQuery();
            }

            catch (SqlException) { }
        }
        public void AtualizarContato(Usuario user)
        {

            try
            {

                string strInserir = "update VUSUACONT set NOME = @nome, ENDERECO = @endereco, BAIRRO = @bairro, CIDADE = @cidade, CEP = @cep, TELEFONE = @telefone where IDUSER2 = @iduser2 and IDEMPR="+Empresas.RecuperaEmpresaCookie().IdEmpresa;

                SqlCommand updateUsuario = new SqlCommand(strInserir, Connection.Conectar());


                updateUsuario.Parameters.AddWithValue("nome", user.Nome);
                updateUsuario.Parameters.AddWithValue("endereco", (object)user.Endereco ?? DBNull.Value);
                updateUsuario.Parameters.AddWithValue("bairro", (object)user.Bairro ?? DBNull.Value);
                updateUsuario.Parameters.AddWithValue("cidade", (object)user.Cidade ?? DBNull.Value);
                updateUsuario.Parameters.AddWithValue("cep", (object)user.Cep ?? DBNull.Value);
                updateUsuario.Parameters.AddWithValue("telefone", (object)user.Telefone ?? DBNull.Value);
                updateUsuario.Parameters.AddWithValue("iduser2", (object)user.IdUser ?? DBNull.Value);


                updateUsuario.ExecuteNonQuery();
            }

            catch (SqlException) { }
        }

        private void VinculaUsuario(string login, Usuario user)
        {

            var emp = Empresas.RecuperaEmpresaCookie();

            string strInsert = "insert into VUSUACONT values (@iduser, @iduser2, @idempr, @nome, @endereco, @bairro, @cidade, @cep, @telefone)";

            SqlCommand inserirUsuario1 = new SqlCommand(strInsert, Connection.Conectar());

            inserirUsuario1.Parameters.AddWithValue("iduser", login);
            inserirUsuario1.Parameters.AddWithValue("iduser2", user.IdUser);
            inserirUsuario1.Parameters.AddWithValue("idempr", emp.IdEmpresa);
            inserirUsuario1.Parameters.AddWithValue("nome", user.Nome);
            inserirUsuario1.Parameters.AddWithValue("endereco", (object)user.Endereco ?? DBNull.Value);
            inserirUsuario1.Parameters.AddWithValue("bairro", (object)user.Bairro ?? DBNull.Value);
            inserirUsuario1.Parameters.AddWithValue("cidade", (object)user.Cidade ?? DBNull.Value);
            inserirUsuario1.Parameters.AddWithValue("cep", (object)user.Cep ?? DBNull.Value);
            inserirUsuario1.Parameters.AddWithValue("telefone", (object)user.Telefone ?? DBNull.Value);

            inserirUsuario1.ExecuteNonQuery();
            Connection.Desconectar();

        }

        public bool ApagarVinculo(Usuario user,string login)
        {
            var emp = Empresas.RecuperaEmpresaCookie();
            SqlCommand sqlExec = new SqlCommand("DELETE FROM VUSUACONT where IDUSER2 = @iduser2 and IDUSER= @iduser and IDEMPR = @idempr", Connection.Conectar());
            sqlExec.Parameters.AddWithValue("iduser2", user.IdUser);
            sqlExec.Parameters.AddWithValue("iduser", login);
            sqlExec.Parameters.AddWithValue("idempr", emp.IdEmpresa);

            sqlExec.ExecuteNonQuery();

            return true;
        }


        private bool VerificaExistencia(Usuario user)
        {
            SqlCommand sqlExec = new SqlCommand("SELECT * FROM TBUSUARIOS where EMAIL = @email", Connection.Conectar());
            sqlExec.Parameters.AddWithValue("email", user.Email);
            SqlDataReader dr = sqlExec.ExecuteReader();
            if (dr.Read())
            {
                return true;
            }
            else
            {
                return false;
            }
        }


      
        public bool AutenticarUsuarioDB(string email, string senha)
        {

            try
            {

                SqlCommand sqlExec = new SqlCommand("SELECT * FROM TBUSUARIOS where EMAIL=@email and SENHA=@senha and USUARIOSISTEMA='S' and (STATUS = 'A'  or STATUS = 'S') ", Connection.Conectar());
                sqlExec.Parameters.AddWithValue("email", (object)email ?? DBNull.Value);
                sqlExec.Parameters.AddWithValue("senha", (object)senha ?? DBNull.Value);

                SqlDataReader dr = sqlExec.ExecuteReader();
                if (dr.Read())
                {

                    Connection.Desconectar();
                    return true;
                }
                else
                {
                    Connection.Desconectar();
                    return false;

                }
            }
            catch (Exception)
            {
                Connection.Desconectar();
                return false;

          }

        }


        public bool AutenticaEmail(string email)
        {

            try
            {

                SqlCommand sqlExec = new SqlCommand("SELECT * FROM TBUSUARIOS where EMAIL=@email and USUARIOSISTEMA='S'", Connection.Conectar());
                sqlExec.Parameters.AddWithValue("email", (object)email ?? DBNull.Value);

                SqlDataReader dr = sqlExec.ExecuteReader();
                if (dr.Read())
                {

                    Connection.Desconectar();
                    return true;
                }
                else
                {
                    Connection.Desconectar();
                    return false;

                }
            }
            catch (Exception)
            {
                Connection.Desconectar();
                return false;

            }
        }

    }

}












   
       
  

    
