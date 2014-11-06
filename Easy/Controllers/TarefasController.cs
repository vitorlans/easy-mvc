using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Easy.Models;
using Postal;
using PagedList;

namespace Easy.Controllers
{
    public class TarefasController : BaseController
    {
        //
        // GET: /Tarefas/

        public ActionResult Index(string sortOrder, string currentFilter, string search, int? page)
        {
            ViewBag.Sort = sortOrder;

            if (search != null)
                page = 1;

            else
                search = currentFilter;

            ViewBag.Filtro = search;

            DAOTarefas daoTaf = new DAOTarefas();
            var listTaf = daoTaf.ListaTarefas();
            var x = from lista in listTaf select lista;

            if (!String.IsNullOrEmpty(search))
            {
                x = x.Where(lista => lista.Descricao.ToUpper().Contains(search.ToUpper())); //PODE-SE ACRESCENTAR MAIS CLAUSULAS
            }

            switch (sortOrder)
            {
                case "icresc":
                    x = x.OrderBy(lista => lista.DtInicio);
                    break;
                case "idesc":
                    x = x.OrderByDescending(lista => lista.DtInicio);
                    break;
                case "tcresc":
                    x = x.OrderBy(lista => lista.DtFim);
                    break;
                case "tdesc":
                    x = x.OrderByDescending(lista => lista.DtFim);
                    break;
                case "baixa":
                    x = x.Where(lista => lista.Prioridade.ToUpper().Equals("B"));
                    break;
                case "media":
                    x = x.Where(lista => lista.Prioridade.ToUpper().Equals("M"));
                    break;
                case "alta":
                    x = x.Where(lista => lista.Prioridade.ToUpper().Equals("A"));
                    break;
                default:
                    x = x.OrderBy(lista => lista.DtFim);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);

            return View(x.ToPagedList(pageNumber, pageSize));
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

            DAOTarefas daoTaf = new DAOTarefas();

            daoTaf.AtualizaTarefa(tar);
            //email.EnviarTarefaAtualizada(tar);

           EmailTarefas eTare = new EmailTarefas();

            eTare.Para = "gigabite.info@gmail.com";
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
        
        [HttpPost]
        public JsonResult BuscaRelacionado(string busca, int max)
        {
            Usuario user = Usuario.VerificaSeOUsuarioEstaLogado();
            DAOUsuario daoUser = new DAOUsuario();

            string id = user.IdUser.ToString();
            var y = daoUser.ListaUsuarios(id).ToList();

            var final = (from search in y where search.Email.StartsWith(busca.ToUpper()) select new {EmailTeste = search.Email });

            return Json(final);
        }

        public ActionResult Comentarios()
        {
            return View();
        }

    }
}
