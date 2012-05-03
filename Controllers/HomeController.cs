using System;
using System.Web.Mvc;
using Punch.Data;
using Punch.Models;

namespace Punch.Controllers
{
    public class HomeController : PunchControllerBase
    {
        public ActionResult Index()
        {
            decimal paalSum = ExpenseDataManager.GetSum(Const.Paal);
            ViewBag.PaalSum = paalSum.ToString("C");

            decimal heleneSum = ExpenseDataManager.GetSum(Const.Helene);
            ViewBag.HeleneSum = heleneSum.ToString("C");

            decimal diff = Math.Abs(heleneSum - paalSum);
            ViewBag.Balance = string.Format(heleneSum > paalSum ? "Helene har betalt {0} mer" : "Pål har betalt {0} mer", diff.ToString("C"));
            return View();
        }

        public ActionResult Enter(string user)
        {
            Identity = user;
            return RedirectToAction("index", "List");
        }

        public ActionResult Import(string user)
        {
            Identity = user;
            var debugModel = new DebugModel {Messages = Importer.Import()}; 

            return View("Import",debugModel);
        }
    }
}
