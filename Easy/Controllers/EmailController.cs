using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Postal;
using Easy.Models;

namespace Easy.Controllers
{
    public class EmailController : Controller
    {
        //
        // GET: /Email/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EnviarEmailCompromisso()
        {
            var email = new EmailCompromisso
            {
                Para = "nathanbernardes7@gmail.com",
                Assunto = "Bem Vindo",
                Titulo = "Vamos sair sabado",
                Descricao = "Vamos la na praça ",
                Quando = "segunda-feira, 27 de outubro de 2014",
                Mensagem = "Este evento é patrocinado por Easy Peoples",
                UrlAceita = "http://facebook.com",
                UrlRejeita = "http://google.com"
            };
            try
            {

                email.Send();
                return RedirectToAction("Index", "Home");

            }
            catch {

                return RedirectToAction("Index", "Calendario");

            }
        }

        public ActionResult EmailCompromissoPreview()
        {
            var email = new EmailCompromisso
            {
                Para = "vitor_hs@live.com",
                Assunto = "Bem Vindo",
                Titulo = "Vamos sair sabado",
                Descricao = "Vamos la na praça ",
                Quando = "segunda-feira, 27 de outubro de 2014",
                Mensagem = "Este evento é patrocinado por Easy Peoples",
                UrlAceita = "http://facebook.com",
                UrlRejeita = "http://google.com"
            };

            return new EmailViewResult(email);
        }

        [HttpPost]
        public ActionResult EnviarEmailSistema()
        {
            var email = new EmailSistema
            {
                Para = "vitor_hs@live.com",
                Assunto = "Bem Vindo",
                Titulo = "Bem Vindo ao Easy Peoples",
                Descricao = "Você foi convidado, a participar de nosso Sistema. Abaixo seus dados de Acesso Inicial:",
                Login = "vitor_hs@live.com",
                Senha = "google123",
                Mensagem = "Você pode alterar sua senha a qualquer momento, basta acessar seu perfil"

            };

            try
            {

                email.Send();
                return RedirectToAction("Index", "Home");

            }
            catch
            {

                return RedirectToAction("Index", "Calendario");

            }
        }


        public ActionResult EmailSistemaPreview()
        {
            var email = new EmailSistema
            {
                Para = "vitor_hs@live.com",
                Assunto = "Bem Vindo",
                Titulo = "Bem Vindo ao Easy Peoples",
                Descricao = "Você foi convidado, a participar de nosso Sistema. Abaixo seus dados de Acesso Inicial:",
                Login = "vitor_hs@live.com",
                Senha = "google123",
                Mensagem = "Você pode alterar sua senha a qualquer momento, basta acessar seu perfil"
            };

            return new EmailViewResult(email);
        }
    }
    }

