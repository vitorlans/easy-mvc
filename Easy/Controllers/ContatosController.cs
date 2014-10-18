using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Easy.Controllers
{
    public class ContatosController : Controller
    {
        //
        // GET: /Contatos/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add() {

            return View();
        
        }

        public ActionResult Detalhes(string id)
        {
            
            return View();

        }
    }
}
