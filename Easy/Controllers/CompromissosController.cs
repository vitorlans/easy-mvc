using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Easy.Models;

namespace Easy.Controllers
{
    public class CompromissosController : Controller
    {
        //
        // GET: /Eventos/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(Compromissos Compromisso)
        {
            if (ModelState.IsValid)
            {
                DAOCompromissos daoCompromisso = new DAOCompromissos();
                daoCompromisso.AddCompromisso(Compromisso);
                Session["AddCompromisso"] = 1;
                Session.Timeout = 1;
            }
            else
            {
                Session["AddCompromisso"] = 0;
                Session.Timeout = 1;
            }
            return RedirectToAction("Index", "Compromissos");
        }
        public ActionResult EditarCompromisso()
        {
            return View();
        }
    }
}
