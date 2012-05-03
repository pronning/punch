using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Punch.Data;

namespace Punch.Controllers
{
    public class UploadController : PunchControllerBase
    {
        public ActionResult Index()
        {
            if (!IsKnownUser)
                return Reset();

            ViewBag.Message = "Last opp";
            return View();
        }

        public ActionResult Save()
        {
            if (!IsKnownUser)
                return Reset();

            //return View();
            return RedirectToAction("index", "List");
        }

        public ActionResult Cancel()
        {
            if (!IsKnownUser)
                return Reset();

            return RedirectToAction("index", "List");
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            try
            {
                if (file == null)
                    return View("Index");

                var sbFile = new SkandiabankenInputFile(file.InputStream);
                var parsed = sbFile.Parse(Identity);

                ExpenseDataManager.Insert(parsed);

                return RedirectToAction("Index", "List");//, todo);
            }
            catch (Exception ex)
            {
                throw new Exception("Kunne ikke importere, det er noe krøll med fila!", ex);
            }
        }
    }
}