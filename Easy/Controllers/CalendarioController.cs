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
                         List<Calendario> Lc = new List<Calendario>();

            var k = 0;
            while(k <= 5 ){

                Lc.Add(
                    new Calendario
                    {
                        id=k.ToString(), 
                        title="facebook",
                        start = "2014-10-27T20:00:00",
                        end = "2014-10-27T20:50:00",
                        url = "http://google.com",
                        allday = "true"

                    }
                );

                 k++;
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
