using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Easy.Models;

namespace Easy.Controllers
{
    public class CalendarioController : Controller
    {
        //
        // GET: /Calendario/

        public ActionResult Index()
        {
            return View();
        }


        public JsonResult RecuperarEventos(string start, string end)
        {
            //var ApptListForDate = DiaryEvent.LoadAppointmentSummaryInDateRange(start, end);
            //var eventList = from e in 
            //                select new
            //                {
            //                    id = e.ID,
            //                    title = e.Title,
            //                    start = e.StartDateString,
            //                    end = e.EndDateString,
            //                    someKey = e.SomeImportantKeyID,
            //                    allDay = false
            //                };
            DAOCompromissos DAOcomp = new DAOCompromissos();
            var LstComp = DAOcomp.ListarCompromissosTodos();
                         List<Calendario> Lc = new List<Calendario>();

                         int x = 0;

            foreach(var i in LstComp) {
                var dt = string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(LstComp[x].DataInicio));
                var dt2 = string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(LstComp[x].DataTermino));

                var hr = Convert.ToDateTime(LstComp[x].DataInicio).ToShortTimeString();
                var hr2 = Convert.ToDateTime(LstComp[x].DataTermino).ToShortTimeString();

                 var cd = dt+"T"+hr;
                 var cd2 = dt2 + "T"+hr2;

                Lc.Add(
                    new Calendario
                    {
                        id= LstComp[x].IdComp.ToString(), 
                        title=  LstComp[x].Titulo,
                        start = cd,
                        end = cd2,
                        url = "http://localhost:58623/Compromissos/EditarCompromisso?id="+LstComp[x].IdComp.ToString(),
                        allday = true,
                        color = "blue"
                    }
                );
                x++;
                 
            }
            
            var row = Lc.ToArray();
            return Json(row, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult NovoCompromisso(string dia)
        {
            return Json(new
            {
                OK = true,
                Mensagem = "Usuário Autenticado. Redirecionando...",
                Local = "112"
            },JsonRequestBehavior.AllowGet);
            
        }

    }
}
