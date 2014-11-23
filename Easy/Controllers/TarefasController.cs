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

        public ActionResult Index(string id, string sortOrder, string currentFilter, string search, int? page)
        {
            Usuario user = Usuario.VerificaSeOUsuarioEstaLogado();
            if (user != null)
            {
                ViewBag.Id = user.IdUser;
            }
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

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            if (id != null && id != "")
            {
                DAOTarefas tar = new DAOTarefas();
                List<Tarefas> listTar = new List<Tarefas>();
                listTar.Add(tar.SelecionaTarefaId(int.Parse(id)));

                if (tar.SelecionaTarefaId(int.Parse(id)).IdTarefa != 0)
                    return View(listTar.ToPagedList(1, 1)); //REALIZAR ALTERACAO PARA LISTA!!!!!
                else
                    return View(tar.ListaTarefas().ToPagedList(pageNumber, pageSize));
            }

            return View(x.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public ActionResult Index(string select1, string select2, string select3, string inputDefault1, string inputDefault2, int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            Usuario user = Usuario.VerificaSeOUsuarioEstaLogado();
            ViewBag.Id = user.IdUser;

            DAOTarefas dTar = new DAOTarefas();

            return View(dTar.ListaTarefasAvancada(select1, select2, select3, inputDefault1, inputDefault2).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult AdicionarTarefa()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdicionarTarefa(Tarefas tar)
        {
            DAOTarefas daoTaf = new DAOTarefas();

            if (ModelState.IsValid)
            {
                daoTaf.AddTarefa(tar);

                EmailController emailC = new EmailController();
                emailC.EnviarEmailTarefa(tar, "Insert");
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
        public ActionResult EditTarefa(string id)
        {
            Usuario user = Usuario.VerificaSeOUsuarioEstaLogado();
            DAOUsuario daoUser = new DAOUsuario();

            int idEdit = 0;

            if (id.ToString() != null && int.TryParse(id, out idEdit ))
            {
                DAOTarefas tar = new DAOTarefas();
                Tarefas tar2 = new Tarefas();
                tar2 = tar.SelecionaTarefaId(int.Parse(id));

                if (tar2.IdTarefa != 0)
                {
                    if (tar2.Criador.IdUser == user.IdUser)
                        return View(tar.SelecionaTarefaId(int.Parse(id)));

                    else
                        return RedirectToAction("Index", "Tarefas");

                }
                else
                    return RedirectToAction("Index", "Tarefas");

            }

            return Content("Tarefa inexistente");
        }

        [HttpPost]
        public ActionResult AtualizaTarefa(Tarefas tar)
        {
            Usuario user = Usuario.VerificaSeOUsuarioEstaLogado();
            DAOUsuario daoUser = new DAOUsuario();

            string prior = "";

            if (tar.Prioridade == "B")
                prior = "Baixa";
            else if (tar.Prioridade == "M")
                prior = "Média";
            else if (tar.Prioridade == "A")
                prior = "Alta";

            DAOTarefas daoTaf = new DAOTarefas();

            daoTaf.AtualizaTarefa(tar);

            EmailController emailC = new EmailController();
            emailC.EnviarEmailTarefa(tar, "Alter");

            Session["AddTarefa"] = 2;
            return RedirectToAction("Index", "Tarefas");


        }

        [HttpPost]
        public JsonResult ConcluirTarefa(int idTare)
        {
            DAOTarefas dtar = new DAOTarefas();

            dtar.ConcluirTarefa(idTare);

            return Json( new {success = true});
        }

        [HttpPost]
        public JsonResult BuscaRelacionado(string busca, int max)
        {
            if (busca != "")
            {
                Usuario user = Usuario.VerificaSeOUsuarioEstaLogado();
                DAOUsuario daoUser = new DAOUsuario();

                string id = user.IdUser.ToString();
                var y = daoUser.ListaUsuarios(id).ToList();

                Connection.Desconectar();

                var final = (from search in y where search.Email.Contains(busca.ToUpper()) select new { EmailTeste = search.Email });

                return Json(final);
            }

            return Json("");
        }

        [HttpPost]
        public JsonResult AtivaDesativa(int idTar, string tipo)
        {
            DAOTarefas dTar = new DAOTarefas();

            dTar.AtivaDesativaTarefa(idTar, tipo);

            return Json(new { success = true });
        }

        public ActionResult Comentarios(int id)
        {
            DAOComentarios dComent = new DAOComentarios();
            
            return View(dComent.ListaComentarios(id));
        }


        [HttpPost]
        public ActionResult RetornaPartial2(int id)
        {
            Session.Remove("idTarefa1");
            Session["idTarefa1"] = id;
            DAOComentarios dComent = new DAOComentarios();

            return PartialView("Comentarios", dComent.ListaComentarios(id));
        }


        [HttpPost]
        public ActionResult RetornaPartial(int id)
        {
            Session.Remove("idTarefa1");
            Session["idTarefa1"] = id;
            DAOComentarios dComent = new DAOComentarios();

            return PartialView("ListaComent", dComent.ListaComentarios(id));
        }

        public ActionResult ListaComent(int id)
        {
            DAOComentarios dComent = new DAOComentarios();

            return View(dComent.ListaComentarios(id));
        }

        [HttpPost]
        public JsonResult AddComentario(int idTar, string comentTar)
        {
            DAOComentarios daoComent = new DAOComentarios();

            daoComent.AdicioonarComentario(idTar, comentTar);

            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult AddCal(string data) {

            Tarefas tare = new Tarefas();
            var dt =Convert.ToDateTime(data).ToShortDateString();

            tare.DtInicio = dt.ToString();
            return View("AdicionarTarefa", tare);
        }

        public JsonResult ValidaContatos(string Relacionado)
        {
            DAOTarefas dTar = new DAOTarefas();
            var list = dTar.ListaContatosTarefa();

            if (Relacionado != "")
            {
                if (list.Contains(Relacionado))
                    return Json(true, JsonRequestBehavior.AllowGet);
                else
                    return Json(false, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EnviaEmailAjax(int idTare , string tipo)
        {
            DAOTarefas dtar = new DAOTarefas();
            EmailController emailC = new EmailController();
            emailC.EnviarEmailTarefa(dtar.SelecionaTarefaId(idTare), tipo);

            return Json(new { success = true });
        }

    }
}
