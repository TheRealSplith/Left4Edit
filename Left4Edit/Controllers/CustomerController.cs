using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Left4Edit.Controllers
{
    public class CustomerController : Controller
    {
        //
        // GET: /Customer/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Detail(Int32? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "id cannot be null");
            return View(id.Value);
        }

        public ActionResult NewPartial()
        {
            return View();
        }

        public PartialViewResult ContactEditor()
        {
            return PartialView();
        }

        public PartialViewResult CredentialEditor()
        {
            return PartialView();
        }

        public PartialViewResult NodeEditor()
        {
            return PartialView();
        }
    }
}
