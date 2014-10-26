using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Easy.Models;

namespace Easy.Controllers
{
    public class TarefasController : Controller
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
            DAOTarefas daoTaf = new DAOTarefas();
            daoTaf.AtualizaTarefa(tar);

            return RedirectToAction("Index", "Tarefas");
        }

    }
}
