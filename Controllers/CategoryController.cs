using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Punch.Data;
using Punch.Models;
using Punch.Utils;

namespace Punch.Controllers
{
    public class CategoryController : PunchControllerBase
    {
        public ActionResult Index()
        {
            if (!IsKnownUser)
                Reset();

            var categoryViews = GetCategoryViews();
            return View(categoryViews);
        }

        private static IEnumerable<CategoryViewModel> GetCategoryViews()
        {
            var categories = CategoryDataManager.GetCategories();
            foreach (CategoryModel categoryModel in categories)
            {
                yield return new CategoryViewModel
                                 {
                                     Category = categoryModel,
                                     HasDependencies = ExpenseDataManager.GetExpenses(categoryModel).Count() > 0
                                 };
            }
        }

        public ActionResult EditItem(string id)
        {
            if (!IsKnownUser)
                return Reset();

            var category = ModelCollection<CategoryModel>.GetItem(id);
            return View(category);
        }

        [HttpPost]
        public ActionResult EditItem(CategoryModel category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            if (CategoryDataManager.UpdateItem(category))
                return RedirectToAction("Index");

            return View(category);
        }

        public ActionResult InsertItem()
        {
            ViewBag.Title = "Legg til";
            return View("Insert", new CategoryModel
                                      {
                                          Value = string.Empty,
                                          Filter = string.Empty,
                                          DefaultCommon = true
                                      });
        }

        public ActionResult InsertItemAndSetExpense(string callerid)
        {
            TempData["SetExpenseId"] = callerid;
            ViewBag.Title = "Legg til";
            return View("Insert", new CategoryModel
                                      {
                                          Value = string.Empty,
                                          Filter = string.Empty,
                                          DefaultCommon = true
            });
        }

        [HttpPost]
        public ActionResult InsertItem(CategoryModel category)
        {
            if (!IsKnownUser)
                return Reset();

            if (!ModelState.IsValid)
            {
                return View("Insert", category);
            }

            category = CategoryDataManager.InsertItem(category);

            var expenseId = TempData["SetExpenseId"] ?? string.Empty;
            ExpenseModel expense = ExpenseDataManager.GetExpense(expenseId.ToString());
            if (expense != null)
            {
                expense.Category = category;
                ExpenseDataManager.UpdateItem(expense);
            }

            return RedirectToAction("Index", "List");
        }

        public ActionResult DeleteItem(string id)
        {
            CategoryDataManager.DeleteItem(id);
            return RedirectToAction("Index");
        }

        public ActionResult ConfirmDelete(string id)
        {
            var category = CategoryDataManager.GetCategory(id);
            var dependencies = ExpenseDataManager.GetExpenses(category);

            ViewBag.Category = category;
            return View("ConfirmDelete", dependencies);
        }


        public ActionResult Cancel()
        {
            if (!IsKnownUser)
                return Reset();

            return RedirectToAction("index");
        }


        public ActionResult Back()
        {
            if (!IsKnownUser)
                return Reset();

            return RedirectToAction("index", "List");
        }

        public PartialViewResult ChangeCategory(string expenseId, bool? saveChanges)
        {
            var expense = ExpenseDataManager.GetExpense(expenseId);
            List<CategoryModel> categories = CategoryDataManager.GetCategories().ToList();

            ViewBag.CurrentExpenseCategory = expense.Category ?? new CategoryModel();
            ViewBag.CurrentExpenseId = expense.Id;
            ViewBag.SaveChanges = saveChanges;

            return PartialView("_CategorySelectList", categories);
        }

        public ActionResult ChangeCategoryForAll(string fromId)
        {
            CategoryModel category = CategoryDataManager.GetCategory(fromId);
            var expenseIds = ExpenseDataManager.GetExpenses(category).Select(item => item.Id).ToSeparatedArray(';');
            List<CategoryModel> categories = CategoryDataManager.GetCategories().ToList();

            ViewBag.CurrentExpenseCategory = category;
            ViewBag.CurrentExpenseId = expenseIds;
            
            return PartialView("_CategorySelectList", categories);
        }

        public PartialViewResult SetCategory(string target, string categoryId)
        {
            var category = CategoryDataManager.GetCategory(categoryId);

            var expense = ExpenseDataManager.GetExpense(target);
            expense.Category = category;

            ExpenseDataManager.UpdateItem(expense);

            return PartialView("_Void");
        }
    }
}