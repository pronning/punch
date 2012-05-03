using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Punch.Models;

namespace Punch.Utils
{
    public static class HtmlHelperExt
    {
        public static HtmlString YearLinks( this HtmlHelper html, IEnumerable<ExpenseModel> expenseModels)
        {
            var years = expenseModels.Select(item => item.Date.Year).Distinct().ToList();
            var ret = "";
            years.Sort();
            foreach (int year in years)
            {
                ret += "<a class=\"year\" href=\"#\">" + year + "</a>";
            }
            return new HtmlString(ret);
        }
    }
}