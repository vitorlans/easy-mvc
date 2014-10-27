using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Easy.Models;
using Easy.Filters;

namespace Easy.Controllers
{
    [AutorizacaoDeAcesso]
    public class BaseController : Controller
    {
        //
        // GET: /Base/
        
            protected override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                base.OnActionExecuting(filterContext);
            }

        }
    }

