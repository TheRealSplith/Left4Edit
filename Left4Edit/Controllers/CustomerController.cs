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

        public PartialViewResult ContactEditor(String targetProperty, String commandName)
        {
            return PartialView(new List<String>() { targetProperty, commandName });
        }

        public PartialViewResult CredentialEditor(String targetProperty, String commandName)
        {
            return PartialView(new List<String>() { targetProperty, commandName });
        }

        public PartialViewResult NodeEditor(String targetProperty, String commandName)
        {
            return PartialView(new List<String>() { targetProperty, commandName });
        }

        public PartialViewResult CustomerEditor(String targetProperty, String commandName)
        {
            return PartialView(new List<String>() { targetProperty, commandName });
        }
    }
}
