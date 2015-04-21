using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GitAdmin.Controllers
{
    public class NormalizeStartUrlController : Controller
    {
		public ActionResult Index(string id)
        {
            return new RedirectResult(Url.Action("Index", "Home", new { id = id }));
        }
    }
}
