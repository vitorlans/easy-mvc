using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Security;

namespace Easy.Models
{
    public class Autenticacao
    {
        public static void RegistraCookieAutenticacao(Usuario user)
        {
            //Criando um objeto cookie
            HttpCookie UserCookie = new HttpCookie("UserCookieAuthentication");

            //Setando o ID do usuário no cookie
            UserCookie.Values["IDUSER"] = Criptografia.Criptografar(user.IdUser.ToString());
            UserCookie.Values["EMAIL"] = Criptografia.Criptografar(user.Email.ToString());

            //Definindo o prazo de vida do cookie
            UserCookie.Expires = DateTime.Now.AddDays(1);

            //Adicionando o cookie no contexto da aplicação
            HttpContext.Current.Response.Cookies.Add(UserCookie);

        }

        public static void BloqueiaAcesso(string block)
        {
            //Criando um objeto cookie
            HttpCookie bloqueio = new HttpCookie("BloqueioAutenticacao");

            //Setando o ID do usuário no cookie
            bloqueio.Values["IPBLOCK"] = Criptografia.Criptografar(block);

            //Definindo o prazo de vida do cookie
            bloqueio.Expires = DateTime.Now.AddHours(1);

            //Adicionando o cookie no contexto da aplicação
            HttpContext.Current.Response.Cookies.Add(bloqueio);

        }

        public static void RegistrarEmpresaCookie(Empresas emp){

            //Criando um objeto cookie
            HttpCookie UserCookie = new HttpCookie("UserCookieEmpresa");

            //Setando o ID do usuário no cookie
            UserCookie.Values["IDEMPRESA"] = Criptografia.Criptografar(emp.NomeEmpresa.ToString());


            //Definindo o prazo de vida do cookie
            UserCookie.Expires = DateTime.Now.AddDays(1);

            //Adicionando o cookie no contexto da aplicação
            HttpContext.Current.Response.Cookies.Add(UserCookie);            
        
        }

        public static void RemoverCookieAutenticacao()
        {
            HttpContext.Current.Response.Cookies.Add(new HttpCookie("UserCookieEmpresa") 
            { Value = string.Empty,
              Expires = DateTime.Now.AddDays(-1) }
            );

            HttpContext.Current.Response.Cookies.Add(new HttpCookie("UserCookieAuthentication")
            {
                Value = string.Empty,
                Expires = DateTime.Now.AddDays(-1)
            }
            );
     
        }


         public static string RecuperaIPCookie()
        {
            var Ip = HttpContext.
            Current.
            Request.
            Cookies["BloqueioAutenticacao"];
            if (Ip == null)
            {
                return null;
            }
            else
            {
                string IpRec = Criptografia.Descriptografar(Ip.Values["IPBLOCK"]);
                return IpRec;
            }
        }

    }

    }

