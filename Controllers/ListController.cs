using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MongoDB.Bson;
using Punch.Data;
using Punch.Models;

namespace Punch.Controllers
{
    public class ListController : PunchControllerBase
    {
        public IEnumerable<CategoryModel> CategoryRepository
        {
            get { return (TempData[Const.Category] as IEnumerable<CategoryModel>) ?? new List<CategoryModel>(); }
            set { TempData[Const.Category] = value; }
        }

        public ActionResult Index()
        {
            if (!IsKnownUser)
                return Reset();

            ViewBag.Title = "Utgifter";

            var expenses = ExpenseDataManager.GetExpenses(Identity);

            ViewBag.Sum = GetSum(expenses);

            return View(expenses);
        }


        public PartialViewResult DeleteItem(string id)
        {
            ExpenseDataManager.DeleteItem(id);
            IEnumerable<ExpenseModel> expenses = ExpenseDataManager.GetExpenses(Identity);
            ViewBag.Sum = GetSum(expenses);
            return PartialView("_PartialList", expenses);
        }


        public PartialViewResult ToggleItem(string id)
        {
            ExpenseDataManager.ToggleItem(id);
            IEnumerable<ExpenseModel> expenses = ExpenseDataManager.GetExpenses(Identity);
            ViewBag.Sum = GetSum(expenses);
            return PartialView("_PartialList", expenses);
        }


        private static string GetSum(IEnumerable<ExpenseModel> expenses)
        {
            return expenses
                .Where(m => m.IsCommon)
                .Sum(m => m.Amount)
                .ToString("C");
        }


        public ActionResult Insert()
        {
            if (!IsKnownUser)
                return Reset();

            ViewBag.Title = "Legg til";

            return View(new ExpenseModel
            {
                Date = DateTime.Now,
                Owner = Identity,
                IsCommon = true,
                IsPossibleDuplicate = false,
                Means = "kontant"
            });

        }


        [HttpPost]
        public ActionResult Insert(ExpenseModel expense)
        {
            if (!ModelState.IsValid)
            {
                return View(expense);
            }

            ExpenseDataManager.InsertItem(expense);

            return RedirectToAction("Index", "List");
        }


        public ActionResult Edit(string id)
        {
            if (!IsKnownUser)
                return Reset();

            var expense = ExpenseDataManager.GetExpense(id);
            return View(expense);
        }


        [HttpPost]
        public ActionResult Edit(ExpenseModel expense)
        {
            if (!ModelState.IsValid)
            {
                return View(expense);
            }

            if (ExpenseDataManager.UpdateItem(expense))
                return RedirectToAction("Index", "List");

            return View(expense);
        }


        public ActionResult Cancel()
        {
            if (!IsKnownUser)
                return Reset();

            return RedirectToAction("index", "List");
        }
    }
}
