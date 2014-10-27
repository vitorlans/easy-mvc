using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Easy.Models;
using System.Web.Mvc;
using System.Web;
using System.Web.UI;

namespace Easy.Models
{
    public class Usuario
    {
        public int IdUser { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Senha1 { get; set; }
        public string Endereco { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Cep { get; set; }
        public string Telefone { get; set; }
        public string UsuarioSistema { get; set; }
        public string LiberaConvite { get; set; }
        public string Status { get; set; }
        public string DataCriacao {get;set;}
        public string Imagem { get; set; }

        public static bool AutenticarUsuario(string email, string senha) { 
        
            DAOUsuario DUser = new DAOUsuario();

            var result = DUser.AutenticarUsuarioDB(email, senha);
            if (result == true)
            {
                var user = DUser.RecuperaUsuarioEmail(email);

                Autenticacao.RegistraCookieAutenticacao(user);

                return true;

            }
            else {

                return false;
            }
        }



        public static Usuario VerificaSeOUsuarioEstaLogado()
        {
            DAOUsuario DUser = new DAOUsuario();
            var Usuario = HttpContext.
            Current.
            Request.
            Cookies["UserCookieAuthentication"];
            if (Usuario == null)
            {
                return null;
            }
            else
            {
                string IDUsuario = Criptografia.Descriptografar(Usuario.Values["IDUSER"]);
                var UsuarioRetornado = DUser.RecuperaUsuario(IDUsuario);
                return UsuarioRetornado;
            }
        }

    }
















    public class VinculoUsuario {

        public int IdUser { get; set; }
        public int IdUser2 { get; set; }

       
    }
}