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
        public ActionResult EnviarEmailCompromisso(Compromissos comp)
        {
            int x = 0;
            foreach (var c in comp.Participantes)
            {
                var email = new EmailCompromisso
                {
                    Para = comp.Participantes[x].Email,
                    Quem = comp.Usuario.Nome + " convidou você para o",
                    Assunto = "Compromisso -" + comp.Titulo,
                    Titulo = "Compromisso -" + comp.Titulo,
                    Descricao = comp.Descricao,
                    Quando = "Início em: " + comp.DataInicio + " até " + comp.DataTermino,
                    Mensagem = "Este evento é patrocinado por Easy Peoples",
                    UrlAceita = "http://localhost:58623/Login/UrlConfirma?codigo="+comp.IdComp+"-"+comp.Participantes[x].IdUser,
                    UrlRejeita = "http://localhost:58623/Login/UrlRejeita?codigo="+comp.IdComp+"-"+comp.Participantes[x].IdUser
                
                };
                try
                {
                    email.Send();

                }
                catch(Exception e)
                {  

                }

                x++;

            }
            return RedirectToAction("Index", "Home");

        }
        

        [HttpPost]
        public ActionResult EnviarEmailSistema(Usuario user)
        {
            var email = new EmailSistema
            {
                Para = user.Email,
                Assunto = "Bem Vindo",
                Titulo = "Bem Vindo a "+Empresas.RecuperaEmpresaCookie().NomeEmpresa,
                Descricao = "Você foi convidado, a participar de nosso Sistema. Abaixo seus dados de Acesso Inicial:",
                Login = user.Email,
                Senha = user.Senha,
                Mensagem = "Você pode alterar sua senha a qualquer momento, basta acessar seu perfil"

            };

            try
            {

                email.Send();
                return RedirectToAction("Index", "Home");

            }
            catch
            {

                return RedirectToAction("Index", "Home");

            }
        }

        [HttpPost]
        public ActionResult EnviarEmailRecuperar(Usuario user)
        {

            var email = new EmailRecuperar
            {
                Para = user.Email,
                Assunto = "Recuperar - Nova Senha",
                Titulo = "Bem Vindo ao Easy Peoples",
                Descricao = "Recentemente foi solicitado um pedido de alteração de senha. Abaixo seus novos dados de Acesso:",
                Login = user.Email,
                Senha = user.Senha,
                Mensagem = "Você pode alterar sua senha a qualquer momento, basta acessar seu perfil"

            };

            try
            {

                email.Send();
                return RedirectToAction("Index", "Login");

            }
            catch
            {

                return RedirectToAction("Recuperar", "Login");

            }
        }


        [HttpPost]
        public ActionResult EnviarEmailDesativa(Usuario user)
        {

            var email = new EmailDesativa
            {
                Para = user.Email,
                Assunto = "Sua Conta foi desativada - Sistema",
                Titulo = "Seu acesso ao sistema foi desativado",
                Descricao = "Permissões de Acesso ao sistema foram removidas, sendo assim não tera mais acesso ao sistema",
                Login = user.Email,
                Mensagem = "Caso esteja incorreto esta nova configuração, peço que entre em contato com seu Adminstrador. EASY"

            };

            try
            {

                email.Send();
                return RedirectToAction("Index", "Login");

            }
            catch
            {

                return RedirectToAction("Recuperar", "Login");

            }


        }


        [HttpPost]
        public ActionResult EnviarEmailTarefa(Tarefas tar, string tipo)
        {
            DAOUsuario dUser = new DAOUsuario();
            Usuario userEmail = new Usuario();
            Usuario user = Usuario.VerificaSeOUsuarioEstaLogado();
            Empresas emp = Empresas.RecuperaEmpresaCookie();

            string msg = "";

            string emailDestinatario = user.Email;
            string nome = user.Nome;
            string quem = " direcionou uma Tarefa para você.";
            string assunto = "Direcionamento de Tarefa - ";

            if (tipo == "Alter")
            {
                quem = "alterou esta Tarefa. Verifique as novas informações abaixo.";
                assunto = "Alteração de Tarefa - ";
            }
            else if (tipo == "Cancelada")
            {
                quem = "cancelou esta Tarefa. Verique as informações.";
                assunto = "Cancelamento de Tarefa - ";
            }
            

            if(tar.Relacionado != null)
            {
                userEmail = dUser.RecuperaUsuarioEmail(tar.Relacionado);
                emailDestinatario = tar.Relacionado;
                nome = userEmail.Nome;
            }

            int dia = DateTime.Now.Day;
            int mes = DateTime.Now.Month;
            int ano = DateTime.Now.Year;

            DateTime dataManha = new DateTime(ano, mes, dia, 12, 00, 00);
            DateTime dataTarde = new DateTime(ano, mes, dia, 18, 00, 00);
            DateTime dataNoite = new DateTime(ano, mes, dia, 23, 59, 59);

            DateTime nowHours = DateTime.Now;

            int comparaManha = DateTime.Compare(nowHours, dataManha);
            int comparaTarde = DateTime.Compare(nowHours, dataTarde);
            int comparaNoite = DateTime.Compare(nowHours, dataNoite);

            if (comparaNoite >= 0 && comparaManha < 0 )
            {
                msg = "Bom dia ";
            }
            else if (comparaManha >= 0 && comparaTarde < 0)
            {
                msg = "Boa tarde ";
            }
            else if (comparaTarde >= 0 && comparaNoite < 0)
            {
                msg = "Boa noite ";
            }
            else
            {
                msg = "Olá ";
            }

            var email = new EmailTarefas
            {
                Para = emailDestinatario,
                Bcc = user.Email,
                Quem = user.Nome + quem,
                Assunto = assunto + emp.NomeEmpresa,
                Titulo = assunto + emp.NomeEmpresa,
                Descricao = tar.Descricao,
                Quando = "Início em: " + Convert.ToDateTime(tar.DtInicio).ToShortDateString() +". Encerramento: "+tar.DtFim,
                Mensagem = msg+nome+ "!! Esta atividade foi destina a você, verifique-a pelo link abaixo.",
                UrlAceita = "http://localhost:0000/Tarefas?search="
            };
            try
            {

                email.Send();
                return RedirectToAction("Index", "Home");

            }
            catch
            {

                return Redirect("~/Shared/Error_Email.cshtml");

            }
        }




























        //public ActionResult EmailSistemaPreview()
        //{
        //    var email = new EmailSistema
        //    {
        //        Para = "vitor_hs@live.com",
        //        Assunto = "Bem Vindo",
        //        Titulo = "Bem Vindo ao Easy Peoples",
        //        Descricao = "Você foi convidado, a participar de nosso Sistema. Abaixo seus dados de Acesso Inicial:",
        //        Login = "vitor_hs@live.com",
        //        Senha = "google123",
        //        Mensagem = "Você pode alterar sua senha a qualquer momento, basta acessar seu perfil"
        //    };

        //    return new EmailViewResult(email);
        //}
    }
    }

