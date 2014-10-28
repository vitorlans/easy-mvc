using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Easy.Models;

namespace Easy.Controllers
{
    public class TarefasController : BaseController
    {
        //
        // GET: /Tarefas/

        public ActionResult Index()
        {
            DAOTarefas daoTaf = new DAOTarefas();
            var listTaf = daoTaf.ListaTarefas();
 
            return View(listTaf);
        }

        public ActionResult AdicionarTarefa()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdicionarTarefa(Tarefas tar)
        {
            if (ModelState.IsValid)
            {
                DAOTarefas daoTaf = new DAOTarefas();

                daoTaf.AddTarefa(tar);
                Session["AddTarefa"] = 1;
                Session.Timeout = 1;
            }
            else
            {
                Session["AddTarefa"] = 0;
                Session.Timeout = 1;
            }

            return RedirectToAction("Index", "Tarefas");
        }

        [HttpPost]
        public ActionResult EditTarefa(int id)
        {
            DAOTarefas tar = new DAOTarefas();

            return View(tar.SelecionaTarefaId(id));
        }

        [HttpPost]
        public ActionResult AtualizaTarefa(Tarefas tar)
        {
            string prior = "";

            if (tar.Prioridade == "B")
                prior = "Baixa";
            else if (tar.Prioridade == "M")
                prior = "Média";
            else if (tar.Prioridade == "A")
                prior = "Alta";

            EnvioEmail email = new EnvioEmail();
            DAOTarefas daoTaf = new DAOTarefas();

            daoTaf.AtualizaTarefa(tar);
            //email.EnviarTarefaAtualizada(tar);
            EmailTarefas eTare = new EmailTarefas();

            eTare.Para = "";
            eTare.Assunto = "Alteração de Tarefa";
            eTare.Titulo = "Alteração Realizada em Tarefa";
            eTare.Descricao = "Algumas alterações foram realizadas em uma Tarefa destinada a você.\nVerifique abaixo as informações:";
            eTare.Quando = Convert.ToString(DateTime.Now);
            eTare.Mensagem = "Descrição: "+tar.Descricao+"\nData de Início: "+tar.DtInicio+"\nData de Término: "+tar.DtFim+"\nPrioridade: "+prior;
            eTare.UrlAceita = "";
            eTare.UrlRejeita = "";

            eTare.Send();

            Session["AddTarefa"] = 2;
            return RedirectToAction("Index", "Tarefas");

        }

    }
}
