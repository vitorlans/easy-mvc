using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Easy.Models
{
    public class DAOCompromissos
    {
        //Status será inserido como (T)erminado, (C)ancelado, (P)róximo ou em (O)correndo
        public static string VerificaStatusComp(Compromissos Comp)
        {
            string status = Comp.Status;
            try
            {
                if (Comp.Status.ToUpper() != "C")
                {
                    SqlCommand sqlAlterarStatusComp;
                    if (DateTime.Parse(Comp.DataInicio) < DateTime.Now && DateTime.Parse(Comp.DataTermino) < DateTime.Now)
                    {
                        status = "T";
                        sqlAlterarStatusComp = new SqlCommand("UPDATE TBCOMPROMISSOS SET STATUS = 'T' where idcomp = @IDCOMP", Connection.Conectar());
                    }
                    else
                    {
                        if (DateTime.Parse(Comp.DataInicio) < DateTime.Now && DateTime.Parse(Comp.DataTermino) > DateTime.Now)
                        {
                            status = "A";
                            sqlAlterarStatusComp = new SqlCommand("UPDATE TBCOMPROMISSOS SET STATUS = 'A' where idcomp = @IDCOMP", Connection.Conectar());
                        }
                        else
                        {
                            status = "P";
                            sqlAlterarStatusComp = new SqlCommand("UPDATE TBCOMPROMISSOS SET STATUS = 'P' where idcomp = @IDCOMP", Connection.Conectar());
                        }
                    }
                    if (Comp.Status != status)
                    {
                        sqlAlterarStatusComp.Parameters.AddWithValue("IDCOMP", Comp.IdComp);
                        sqlAlterarStatusComp.ExecuteNonQuery();
                    }
                }
            }
            catch
            {
            }
            return status;
        }
        public void CancelarComp(Compromissos Comp, Usuario User)
        {
            //Somente o criador do compromisso poderá alterar o status do mesmo
            if (Comp.Usuario.IdUser == User.IdUser)
            {
                try
                {
                    SqlCommand sqlTerminarCompromisso = new SqlCommand("UPDATE TBCOMPROMISSOS SET STATUS = 'C' where idcomp = @IDCOMP", Connection.Conectar());
                    sqlTerminarCompromisso.Parameters.AddWithValue("IDCOMP", Comp.IdComp);
                    sqlTerminarCompromisso.ExecuteNonQuery();
                }
                catch (SqlException sqlExcp)
                {
                }
                catch (Exception Erro)
                {
                }
                Connection.Desconectar();
            }
        }
        public void AtivarComp(Compromissos Comp, Usuario User)
        {
            //Somente o criador do compromisso poderá alterar o status do mesmo
            if (Comp.Usuario.IdUser == User.IdUser)
            {
                try
                {
                    SqlCommand sqlTerminarCompromisso = new SqlCommand("UPDATE TBCOMPROMISSOS SET STATUS = 'P' where idcomp = @IDCOMP", Connection.Conectar());
                    sqlTerminarCompromisso.Parameters.AddWithValue("IDCOMP", Comp.IdComp);
                    sqlTerminarCompromisso.ExecuteNonQuery();
                }
                catch (SqlException sqlExcp)
                {
                }
                catch (Exception Erro)
                {
                }
                Connection.Desconectar();
            }
        }
        public List<Compromissos> ListarCompromissosData()
        {
            DAOUsuario DUser = new DAOUsuario();
            DAOEmpresas DEmpresa = new DAOEmpresas();
            List<Compromissos> ListaComp;
            ListaComp = new List<Compromissos>();
            Usuario User = Usuario.VerificaSeOUsuarioEstaLogado();
            Empresas Emp = Empresas.RecuperaEmpresaCookie();

            try
            {
                int y;
                int x = 0;
                for (y = 1; y <= 2; y++)
                {
                    string strSelect;
                    if (y == 1)
                    {
                        strSelect = "SELECT * FROM TBCOMPROMISSOS CO, VCOMPUSER UC, TBUSUARIOS US WHERE CO.IDCOMP = UC.IDCOMP " +
                            "AND UC.IDUSER = US.IDUSER AND UC.IDUSER = " + User.IdUser + " AND CO.IDEMPR = " + Emp.IdEmpresa;
                    }
                    else
                    {
                        strSelect = "SELECT * FROM TBCOMPROMISSOS WHERE IDUSER = " + User.IdUser + " AND IDEMPR = " + Emp.IdEmpresa;
                    }
                    SqlCommand sqlComando = new SqlCommand(strSelect, Connection.Conectar());
                    SqlDataReader dr = sqlComando.ExecuteReader();

                    while (dr.Read())
                    {
                        ListaComp.Add(
                            new Compromissos
                            {
                                IdComp = int.Parse(dr["IDCOMP"].ToString()),
                                Titulo = dr["TITULO"].ToString(),
                                Descricao = dr["DESCRICAO"].ToString(),
                                DataInicio = dr["DT_INICIO"].ToString(),
                                DataTermino = dr["DT_FIM"].ToString(),
                                Status = (dr["STATUS"].ToString()),
                                Usuario = DUser.RecuperaUsuario(dr["IDUSER"].ToString()),
                                Empresa = DEmpresa.RecuperarEmpresaId(dr["IDEMPR"].ToString())
                            }
                            );

                        ListaComp[x].Status = DAOCompromissos.VerificaStatusComp(ListaComp[x]);

                        x++;
                    }
                }
            }
            catch (SqlException sqlErro)
            {
            }
            catch (Exception erro)
            {
            }
            Connection.Desconectar();
            return ListaComp;
        }
        public List<Compromissos> ListarCompromissosTodos()
        {
            DAOUsuario DUser = new DAOUsuario();
            DAOEmpresas DEmpresa = new DAOEmpresas();
            List<Compromissos> ListaComp;
            ListaComp = new List<Compromissos>();

            try
            {
                SqlCommand sqlComando = new SqlCommand("SELECT * FROM TBCOMPROMISSOS", Connection.Conectar());
                SqlDataReader dr = sqlComando.ExecuteReader();

                while (dr.Read())
                {
                    ListaComp.Add(
                        new Compromissos
                        {
                            IdComp = int.Parse(dr["IDCOMP"].ToString()),
                            Titulo = dr["TITULO"].ToString(),
                            Descricao = dr["DESCRICAO"].ToString(),
                            DataInicio = dr["DT_INICIO"].ToString(),
                            DataTermino = dr["DT_FIM"].ToString(),
                            Status = (dr["STATUS"].ToString()),
                            Usuario = DUser.RecuperaUsuario(dr["IDUSER"].ToString()),
                            Empresa = DEmpresa.RecuperarEmpresaId(dr["IDEMPR"].ToString())
                        }
                        );
                } 
            }
            catch(SqlException sqlErro)
            {
            }
            catch(Exception erro)
            {
            }
            Connection.Desconectar();
            return ListaComp;
        }
        public void AddCompromisso(Compromissos Compromisso)
        {
            Compromisso.Status = "P";
            try
            {
                SqlCommand sqlExec = new SqlCommand("INSERT INTO TBCOMPROMISSOS VALUES (@TITULO, @DESCRICAO, @DT_INICIO, @DT_FIM, @STATUS, @IDUSER, @IDEMPR)", Connection.Conectar());

                //sqlExec.Parameters.AddWithValue("IDCOMP", Compromisso.IdComp);
                sqlExec.Parameters.AddWithValue("TITULO", Compromisso.Titulo);
                sqlExec.Parameters.AddWithValue("DESCRICAO", Compromisso.Descricao);
                sqlExec.Parameters.AddWithValue("DT_INICIO", Convert.ToDateTime(Compromisso.DataInicio));
                sqlExec.Parameters.AddWithValue("DT_FIM", Convert.ToDateTime(Compromisso.DataTermino));
                sqlExec.Parameters.AddWithValue("STATUS", Compromisso.Status);
                sqlExec.Parameters.AddWithValue("IDUSER", Compromisso.Usuario.IdUser);
                sqlExec.Parameters.AddWithValue("IDEMPR", (object)Compromisso.Empresa.IdEmpresa ?? DBNull.Value);

                sqlExec.ExecuteNonQuery();
            }
            catch (SqlException sqlEx)
            {
            }
            Connection.Desconectar();
        }
        public void EditarCompromisso(Compromissos Compromisso, Usuario User)
        {
            if (Compromisso.Usuario.IdUser == User.IdUser)
            {
                try
                {
                    SqlCommand sqlExec = new SqlCommand("UPDATE TBCOMPROMISSOS SET TITULO = @TITULO, DESCRICAO = @DESCRICAO, DT_INICIO = @DT_INICIO, DT_FIM = @DT_FIM WHERE IDCOMP = @IDCOMP", Connection.Conectar());

                    sqlExec.Parameters.AddWithValue("IDCOMP", Compromisso.IdComp);
                    sqlExec.Parameters.AddWithValue("TITULO", Compromisso.Titulo);
                    sqlExec.Parameters.AddWithValue("DESCRICAO", Compromisso.Descricao);
                    sqlExec.Parameters.AddWithValue("DT_INICIO", Convert.ToDateTime(Compromisso.DataInicio));
                    sqlExec.Parameters.AddWithValue("DT_FIM", Convert.ToDateTime(Compromisso.DataTermino));

                    sqlExec.ExecuteNonQuery();
                }
                catch (SqlException sqlEx)
                {
                }
                Connection.Desconectar();
            }

        }
        public Compromissos SelecionarCompromissoId(int id)
        {
            Compromissos Compromisso = new Compromissos();
            
            DAOEmpresas DEmpresa = new DAOEmpresas();
            DAOUsuario DUser = new DAOUsuario();
            List<Usuario> lista = new List<Usuario>();
            lista = DAOCompromissos.RecuperaPartComp(id);
            try
            {
                SqlCommand sqlComando = new SqlCommand("SELECT * FROM TBCOMPROMISSOS WHERE IDCOMP = "+ id,Connection.Conectar());
                SqlDataReader dr = sqlComando.ExecuteReader();

                
                lista = DAOCompromissos.RecuperaPartComp(id);
                

                if (dr.Read())
                {
                    Compromisso.IdComp = int.Parse(dr["IDCOMP"].ToString());
                    Compromisso.Titulo = dr["TITULO"].ToString();
                    Compromisso.Descricao = dr["DESCRICAO"].ToString();
                    Compromisso.DataInicio = dr["DT_INICIO"].ToString();
                    Compromisso.DataTermino = dr["DT_FIM"].ToString();
                    Compromisso.Status = (dr["STATUS"].ToString());
                    Compromisso.Usuario = DUser.RecuperaUsuario(dr["IDUSER"].ToString());
                    Compromisso.Empresa = DEmpresa.RecuperarEmpresaId(dr["IDEMPR"].ToString());
                    Compromisso.Participantes = lista;
                }
                
            }
            catch(SqlException e){}
            Connection.Desconectar();

           /* if (lista != null && Compromisso != null)
            {
                int x = 0;
                while (x < lista.Count)
                {
                    DAOUsuario DUserPart = new DAOUsuario();
                    Usuario UserPart = new Usuario();
                    int idUser = Int32.Parse(lista[x].ToString());
                    UserPart.IdUser = idUser;

                    UserPart = DUserPart.RecuperaUsuario(idUser.ToString());
                    string participantes = UserPart.Email.ToString() + ";";
                    x++;
                }
            }*/
            
            return Compromisso;
        }
        public void AddParticipantesCompromisso(Compromissos Comp, Usuario Part)
        {
            try
            {
                SqlCommand sqlExec = new SqlCommand("INSERT INTO VCOMPUSER VALUES (@IDCOMP, @IDUSER)", Connection.Conectar());

                //sqlExec.Parameters.AddWithValue("IDCOMP", Compromisso.IdComp);
                sqlExec.Parameters.AddWithValue("IDCOMP", Comp.IdComp);
                sqlExec.Parameters.AddWithValue("IDUSER", Part.IdUser);

                sqlExec.ExecuteNonQuery();
            }
            catch (SqlException sqlEx)
            {
            }
            Connection.Desconectar();
        }
        public int RecuperaIdUltimoCompromisso(int IdUser)
        {
            int IdComp = 0;
            try
            {
                SqlCommand sqlComando = new SqlCommand("SELECT IDCOMP FROM TBCOMPROMISSOS WHERE IDUSER = " + IdUser + "ORDER BY IDCOMP DESC", Connection.Conectar());
                SqlDataReader dr = sqlComando.ExecuteReader();

                if (dr.Read())
                {
                    IdComp = Int32.Parse(dr["IDCOMP"].ToString());
                }
            }
            catch (SqlException e) { }
            Connection.Desconectar();
            return IdComp;
        }
        public static List<Usuario> RecuperaPartComp(int id)
        {
            DAOUsuario DUser = new DAOUsuario();
            List<Usuario> lista = new List<Usuario>();
            try
            {
                SqlCommand sqlComando = new SqlCommand("SELECT US.* FROM TBUSUARIOS US, VCOMPUSER UC WHERE US.IDUSER = UC.IDUSER AND UC.IDCOMP = " + id, Connection.Conectar());
                SqlDataReader dr = sqlComando.ExecuteReader();
                int x =0;
                while (dr.Read())
                {
                    lista.Add
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
                x++;
            }
            catch(SqlException e){}
            Connection.Desconectar();
            return lista;
        }
        public void RemoverVincPart(int id)
        {
            try
            {
                SqlCommand sqlComando = new SqlCommand("DELETE FROM VCOMPUSER WHERE IDCOMP = " + id, Connection.Conectar());
                sqlComando.ExecuteNonQuery();

            }
            catch (SqlException e) { }
            Connection.Desconectar();

        }

    }
}